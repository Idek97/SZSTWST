using Org.Apache.Http.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;
using Xamarin.Essentials;

namespace SZSTWST_MobileApp
{
    public sealed class TcpService
    {
        public TcpClient _tcpClient;
        public NetworkStream _networkStream;
        public string _recievedMessege;
        public Xamarin.Essentials.NetworkAccess networkAccess;
        public bool _isAdmin;
        public int _userId;

        public List<AssetTable> assetTableList = new List<AssetTable>();
        public List<KeyValuePair<int, string>> userStrings = new List<KeyValuePair<int, string>>();

        private TcpService()
        {
            networkAccess = Connectivity.NetworkAccess;
            _tcpClient = new TcpClient();
            while (!_tcpClient.Connected)
            {
                try
                {
                    _tcpClient.Connect(IPAddress.Parse("192.168.0.80"), 100);
                }
                catch { }
            }
            _networkStream = _tcpClient.GetStream();
            Thread tcpReaderThread = new Thread(ReceiveTcpCommand);
            tcpReaderThread.Start();
        }
        private static TcpService _instance;
        private static readonly object InstanceLock = new object();
        public static TcpService GetInstance()
        {
            if (_instance == null)
            {
                lock (InstanceLock)
                {
                    if (_instance == null)
                        _instance = new TcpService();
                }
            }
            return _instance;
        }

        public void ReceiveTcpCommand()
        {
            byte[] buffer = new byte[0];
            byte[] tempBuffer = new byte[2];
            int packageLength = 0;
            while (true)
            {
                if (_tcpClient.Connected)
                {
                    try
                    {
                        if (packageLength > 0)
                        {
                            _networkStream.Read(buffer, 0, packageLength);
                            packageLength = 0;
                            string recieved = Encoding.Unicode.GetString(buffer);
                            buffer = new byte[1];
                            if (!IsInternalCommand(recieved))
                                _recievedMessege = recieved;
                        }
                        else
                        {
                            _networkStream.Read(tempBuffer, 0, tempBuffer.Length);
                            packageLength = BitConverter.ToInt16(tempBuffer);
                            tempBuffer = new byte[2];
                            buffer = new byte[packageLength];
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        tempBuffer = new byte[2];
                        buffer = new byte[1];
                    }
                }
            }
        }

        public void SendTcpCommand(string command)
        {
            if (_tcpClient.Connected && !string.IsNullOrEmpty(command))
            {
                try
                {
                    byte[] tempBytes = Encoding.Unicode.GetBytes(command);
                    byte[] length = BitConverter.GetBytes((Int16)tempBytes.Length);
                    byte[] sendBytes = new byte[length.Length + tempBytes.Length];
                    sendBytes[0] = length[0];
                    sendBytes[1] = length[1];
                    for (int i = 2, j = 0; i < sendBytes.Length; i++)
                    {
                        sendBytes[i] = tempBytes[j++];
                    }
                    _networkStream.Write(sendBytes, 0, sendBytes.Length);
                    _networkStream.Flush();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            };
        }

        public void KillTcpConnection()
        {
            SendTcpCommand(string.Format("Disconnect\n"));
            _networkStream.Close();
            _tcpClient.Close();
        }

        private bool IsInternalCommand(string command)
        {
            if (Regex.IsMatch(command, ClientRegexPattern.LoginAccepted))
            {
                _userId = int.Parse(Regex.Match(command, ClientRegexPattern.LoginAccepted).Groups[1].Value);
                _isAdmin = Regex.Match(command, ClientRegexPattern.LoginAccepted).Groups[2].Value.Equals("1") ? true : false;

                SendTcpCommand(TCPClientCommand.GetAssetList);
                return false;
            }

            else if (Regex.IsMatch(command, ClientRegexPattern.CurrentAssetList))
            {
                try
                {
                    assetTableList = JsonSerializer.Deserialize<List<AssetTable>>(Regex.Match(command, ClientRegexPattern.CurrentAssetList).Groups[1].Value);
                    SendTcpCommand(TCPClientCommand.GetListOfUsers);
                }
                catch (JsonException)
                {
                    Debug.WriteLine("Błąd przychodzącej listy AssetTable!");
                    SendTcpCommand(TCPClientCommand.RefreshAssetList);
                }
                return true;
            }

            else if (Regex.IsMatch(command, ClientRegexPattern.ListOfUsers))
            {
                try
                {
                    userStrings = JsonSerializer.Deserialize<List<KeyValuePair<int, string>>>(Regex.Match(command, ClientRegexPattern.ListOfUsers).Groups[1].Value);
                }
                catch (JsonException)
                {
                    Debug.WriteLine("Błąd przychodzącej listy UserStrings!");
                    SendTcpCommand(TCPClientCommand.GetListOfUsers);
                }
                return true;
            }

            else if (Regex.IsMatch(command, TCPServerCommand.Ping))
            {
                return true;
            }

            else if (Regex.IsMatch(command, TCPServerCommand.InvalidCommand))
            {
                return true;
            }

            return false;
        }
    }
}
