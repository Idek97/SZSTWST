using Org.BouncyCastle.Bcpg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;
using SZSTWST_Shared.SqlTableName;

namespace SZSTWST_server
{
    public delegate void NotifyTcpServerControllerDelegate(HandleClient sender, Enums.HandleClientCommand command);

    internal class TCPServerController
    {
        private int _liseningPort = 100;

        TcpListener _serverSocket;
        TcpClient _tcpClient = new TcpClient();
        List<HandleClient> clientList = new List<HandleClient>();
        DataHolder dataHolder;

        public TCPServerController()
        {
            Console.WriteLine("Server is ready...");
            _serverSocket = new TcpListener(_liseningPort);
            _serverSocket.Start();
            Console.Title = "SZSTWST - server";
            dataHolder = DataHolder.GetInstance();
            dataHolder.FillUpAssetList();
            dataHolder.FillUpUserList();
        }

        public void _child_NotifyParentEvent(HandleClient sender, Enums.HandleClientCommand commandId)
        {
            if (commandId == Enums.HandleClientCommand.RemoveObject)
            {
                clientList.Remove(sender);
                Console.Title = $"SZSTWST - server, Number of connected clients: {clientList.Count}";
            }
            else if (commandId == Enums.HandleClientCommand.RefreshAssetList)
            {
                dataHolder.FillUpAssetList();
                foreach (HandleClient client in clientList)
                {
                    client.SendResponse(string.Format(TCPServerCommand.CurrentAssetList, JsonSerializer.Serialize(dataHolder.AssetList)));
                }
            }
            else if (commandId == Enums.HandleClientCommand.CheckUserIsRepeated)
            {
                if (clientList.FindAll(c => c._authorizedUserId == sender._authorizedUserId).Count > 1)
                {
                    sender._authorizedUserId = null;
                    sender._isUserAdmin = false;
                    sender.SendResponse(TCPServerCommand.LoginDenied);
                }
                else
                {
                    Console.WriteLine(string.Format("{2}: Client No. {0} login as user with {1} ID", sender._serverClientNumber, sender._authorizedUserId, DateTime.Now.ToString(StringFormater.DateTimeFormat)));
                    sender.SendResponse(string.Format(TCPServerCommand.LoginAccepted, sender._authorizedUserId.ToString(), sender._isUserAdmin ? '1' : '0'));
                }
            }
        }

        public void SetupServer()
        {
            int clientCounter = 0;

            while (true)
            {
                if (clientCounter == int.MaxValue)
                    clientCounter = 0;
                clientCounter++;
                _tcpClient = _serverSocket.AcceptTcpClient();
                HandleClient handleClient = new HandleClient(_tcpClient, clientCounter);
                handleClient.NotifyParentEvent += new NotifyTcpServerControllerDelegate(_child_NotifyParentEvent);
                Console.WriteLine(string.Format("Client No. {0} Connected", clientCounter));
                clientList.Add(handleClient);
                Console.Title = $"SZSTWST - server, Number of connected clients: {clientList.Count}";
            }
        }
    }

    public class HandleClient
    {
        TcpClient _client;
        public int? _authorizedUserId;
        public bool _isUserAdmin = false;
        public int _serverClientNumber;
        NetworkStream _networkStream;
        DateTime _loginTimeOut;
        DataHolder dataHolder;
        DateTime _ping;

        public event NotifyTcpServerControllerDelegate NotifyParentEvent;

        public HandleClient(TcpClient client, int clientNumber)
        {
            _client = client;
            _serverClientNumber = clientNumber;
            Thread clientThread = new Thread(ClientThread);
            clientThread.Start();
            _ping = DateTime.Now;
            dataHolder = DataHolder.GetInstance();
        }

        private void ClientThread()
        {
            byte[] bytesFrom = new byte[1024];
            if (_client.Connected)
            {
                _networkStream = _client.GetStream();
                _loginTimeOut = DateTime.Now;
            }
            while (_client.Connected)
            {
                try
                {
                    //if (_ping.AddSeconds(1) < DateTime.Now)
                    //{
                    //    SendResponse(TCPServerCommand.Ping);
                    //    _ping = DateTime.Now.AddSeconds(1);
                    //}
                    if (_authorizedUserId != null && DateTime.Now > _loginTimeOut)
                    {
                        SendResponse(TCPServerCommand.SessionTimeout);
                        Console.WriteLine(string.Format("{2}: Client No. {0} User ID {1} Login Timeout Expired!", _serverClientNumber, _authorizedUserId, DateTime.Now.ToString(StringFormater.DateTimeFormat)));
                        _authorizedUserId = null;
                    }
                    if (_networkStream.DataAvailable == true)
                    {
                        _networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                        string message = Encoding.Unicode.GetString(bytesFrom);
                        _networkStream.Flush();
                        if (!string.IsNullOrEmpty(message))
                        {
                            _loginTimeOut = DateTime.Now.AddMinutes(15);
                            SendResponse(DecodeCommand(message));
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(string.Format("{1}: Client No. {0}: The connection was aborted locally!", _serverClientNumber, DateTime.Now.ToString(StringFormater.DateTimeFormat)));
                    _networkStream.Close();
                    _client.Close();
                    NotifyParentEvent(this, Enums.HandleClientCommand.RemoveObject);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format($"{DateTime.Now.ToString(StringFormater.DateTimeFormat)}: {ex}"));
                }
                finally
                {
                    Array.Clear(bytesFrom, 0, bytesFrom.Length);
                }
            }
        }

        private string DecodeCommand(string recievedText)
        {
            #region Connection and Login Operations

            if (Regex.IsMatch(recievedText, ServerRegexPattern.TryToLogin))
            {
                string loginText = Regex.Match(recievedText, ServerRegexPattern.TryToLogin).Groups[1].Value;
                string passwordText = Regex.Match(recievedText, ServerRegexPattern.TryToLogin).Groups[2].Value;

                SZSTWST_Shared.SqlTable.UserTable user = dataHolder.UserList.Find(u => u.CollegeIndex.Equals(int.Parse(loginText)));

                if (user != null && user.Password.Equals(passwordText) && user.IsActive && _authorizedUserId == null)
                {
                    _authorizedUserId = user.Id;
                    _isUserAdmin = user.IsAdmin;
                    NotifyParentEvent(this, Enums.HandleClientCommand.CheckUserIsRepeated);

                    return string.Empty;
                }
                return TCPServerCommand.LoginDenied;
            }

            else if (Regex.IsMatch(recievedText, TCPClientCommand.Disconnect))
            {
                Console.WriteLine(string.Format("{1}: Client No. {0} Disconnected", _serverClientNumber, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
                _networkStream.Close();
                _client.Close();
                NotifyParentEvent(this, Enums.HandleClientCommand.RemoveObject);
                return string.Empty;
            }

            else if (Regex.IsMatch(recievedText, TCPClientCommand.Logout))
            {
                if (_authorizedUserId != null)
                {
                    Console.WriteLine(string.Format("{2}: Client No. {0} logout from {1} user", _serverClientNumber, _authorizedUserId, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
                    _authorizedUserId = null;
                    _isUserAdmin = false;
                }
                return string.Empty;
            }

            #endregion Connection and Login Operations

            #region Asset Operations

            else if (Regex.IsMatch(recievedText, TCPClientCommand.RefreshAssetList))
            {
                NotifyParentEvent(this, Enums.HandleClientCommand.RefreshAssetList);
                return string.Empty;
            }

            else if (Regex.IsMatch(recievedText, TCPClientCommand.GetAssetList))
            {
                return string.Format(TCPServerCommand.CurrentAssetList, JsonSerializer.Serialize(dataHolder.AssetList));
            }

            else if (Regex.IsMatch(recievedText, ServerRegexPattern.AssetOperation))
            {
                if (_authorizedUserId != null && _isUserAdmin)
                {
                    //NotifyParentEvent(this, Enums.HandleClientCommand.RefreshAssetList);

                    Enums.AssetOperation assetOperation = (Enums.AssetOperation)Enum.Parse(typeof(Enums.AssetOperation), Regex.Match(recievedText, ServerRegexPattern.AssetOperation).Groups[1].Value);
                    SZSTWST_Shared.SqlTable.AssetTable asset = JsonSerializer.Deserialize<SZSTWST_Shared.SqlTable.AssetTable>(Regex.Match(recievedText, ServerRegexPattern.AssetOperation).Groups[2].Value);
                    string operationString = string.Empty;
                    if (asset != null)
                    {
                        if (assetOperation == Enums.AssetOperation.CreateAsset && dataHolder.AssetList.FindAll(a => a.CodeName.Equals(asset.CodeName)).Count == 0)
                        {
                            operationString = $"INSERT INTO {SZSTWST_Shared.SqlTableName.AssetTable._tableName}" +
                                $"({SZSTWST_Shared.SqlTableName.AssetTable._codeNameColumn}, {SZSTWST_Shared.SqlTableName.AssetTable._serialNumberColumn}," +
                                $" {SZSTWST_Shared.SqlTableName.AssetTable._nameColumn}, {SZSTWST_Shared.SqlTableName.AssetTable._descriptionColumn}," +
                                $" {SZSTWST_Shared.SqlTableName.AssetTable._isActiveColumn}, {SZSTWST_Shared.SqlTableName.AssetTable._creationDateColumn})" +
                                $" VALUES('{asset.CodeName}', {(asset.SerialNumber != null ? $"'{asset.SerialNumber}'" : "null")}," +
                                $" '{asset.Name}', {(asset.Description != null ? $"'{asset.Description}'" : "null")}," +
                                $" '{(asset.IsActive ? 1 : 0)}', '{DateTime.Now.ToString(StringFormater.DateTimeFormat)}');";
                        }
                        else if (assetOperation == Enums.AssetOperation.EditAsset
                            && dataHolder.AssetList.FindAll(a => a.Id == asset.Id).Count == 1
                            && !dataHolder.AssetList.FindAll(a => a.Id == asset.Id).Equals(asset))
                        {
                            SZSTWST_Shared.SqlTable.AssetTable editedAsset = dataHolder.AssetList.Find(a => a.Id == asset.Id);

                            operationString = $"UPDATE {SZSTWST_Shared.SqlTableName.AssetTable._tableName} SET" +
                                $"{(!asset.CodeName.Equals(editedAsset.CodeName) ? $" {SZSTWST_Shared.SqlTableName.AssetTable._codeNameColumn} = '{asset.CodeName}'," : string.Empty)}" +

                             $" {SZSTWST_Shared.SqlTableName.AssetTable._serialNumberColumn} = {(asset.SerialNumber == null ? "null" : $"'{asset.SerialNumber}'")}," +

                            $"{(!asset.Name.Equals(editedAsset.Name) ? $" {SZSTWST_Shared.SqlTableName.AssetTable._nameColumn} = '{asset.Name}'," : string.Empty)}" +

                            $" {SZSTWST_Shared.SqlTableName.AssetTable._descriptionColumn} = {(asset.Description == null ? "null" : $"'{asset.Description}")}'," +


                            $"{(asset.IsActive != editedAsset.IsActive ? $" {SZSTWST_Shared.SqlTableName.AssetTable._isActiveColumn} = '{(asset.IsActive ? 1 : 0)}'" : string.Empty)}";

                            operationString = operationString.TrimEnd(',');
                            operationString += $" WHERE {SZSTWST_Shared.SqlTableName.AssetTable._idColumn} = {asset.Id};";
                        }
                        else
                            return string.Format(TCPServerCommand.AssetOperationResponse, (int)assetOperation, (int)Enums.OperationResult.Failure);

                        MySQLConnector.ExecuteWrite(operationString);

                        return string.Format(TCPServerCommand.AssetOperationResponse, (int)assetOperation, (int)Enums.OperationResult.Success);
                    }
                }
                return string.Empty;
            }

            #endregion Asset Operations

            #region Assignment Operations

            else if (Regex.IsMatch(recievedText, ServerRegexPattern.CreateNewAssignment))
            {
                int selectedUserId = int.Parse(Regex.Match(recievedText, ServerRegexPattern.CreateNewAssignment).Groups[1].Value);
                if (!_isUserAdmin && selectedUserId != _authorizedUserId)
                    return string.Format(TCPServerCommand.CreateNewAssignmentResponse, (int)Enums.OperationResult.Failure, null);

                DateTime startDate = DateTime.Parse(Regex.Match(recievedText, ServerRegexPattern.CreateNewAssignment).Groups[2].Value);
                string startDateText = startDate.ToString(StringFormater.DateTimeFormat);

                DateTime stopDate = DateTime.Parse(Regex.Match(recievedText, ServerRegexPattern.CreateNewAssignment).Groups[3].Value);
                string stopDateText = stopDate.ToString(StringFormater.DateTimeFormat);

                List<int> assetIds = JsonSerializer.Deserialize<List<int>>(Regex.Match(recievedText, ServerRegexPattern.CreateNewAssignment).Groups[4].Value.Trim('\0'));
                List<int> idsWithError = new List<int>();

                string query;
                foreach (int assetId in assetIds)
                {
                    query = string.Format($"Select count(aa.{SZSTWST_Shared.SqlTableName.AssignmentAssetTable._idColumn})" +
                        $" from {SZSTWST_Shared.SqlTableName.AssignmentAssetTable._tableName} aa join {SZSTWST_Shared.SqlTableName.AssignmentTable._tableName} a" +
                        $" on aa.{SZSTWST_Shared.SqlTableName.AssignmentAssetTable._idAssignmentColumn} = a.{SZSTWST_Shared.SqlTableName.AssignmentTable._idColumn}" +
                        $" where (a.{SZSTWST_Shared.SqlTableName.AssignmentTable._startDateColumn} BETWEEN '{startDateText}' and '{stopDateText}' " +
                        $"or a.{SZSTWST_Shared.SqlTableName.AssignmentTable._stopDateColumn} BETWEEN '{startDateText}' and '{stopDateText}') " +
                        $"and a.{SZSTWST_Shared.SqlTableName.AssignmentTable._isFinishedColumn}=false and aa.{SZSTWST_Shared.SqlTableName.AssignmentAssetTable._idAssetColumn} = '{assetId}';");

                    int result = MySQLConnector.ExecuteReadCommandInt(query).SingleOrDefault();

                    if (result > 0)
                        idsWithError.Add(assetId);
                }

                if (idsWithError.Count > 0)
                    return string.Format(TCPServerCommand.CreateNewAssignmentResponse, (int)Enums.OperationResult.Failure, JsonSerializer.Serialize(idsWithError));

                var tempDate = DateTime.Now.ToString(StringFormater.DateTimeFormat);

                query = string.Format($"INSERT INTO {SZSTWST_Shared.SqlTableName.AssignmentTable._tableName}" +
                   $"({SZSTWST_Shared.SqlTableName.AssignmentTable._idCreatorUserColumn},{SZSTWST_Shared.SqlTableName.AssignmentTable._idUserColumn}," +
                   $"{SZSTWST_Shared.SqlTableName.AssignmentTable._creationDateColumn},{SZSTWST_Shared.SqlTableName.AssignmentTable._startDateColumn}," +
                   $"{SZSTWST_Shared.SqlTableName.AssignmentTable._stopDateColumn},{SZSTWST_Shared.SqlTableName.AssignmentTable._isFinishedColumn})" +
                   $" VALUES" +
                   $"({_authorizedUserId}, {selectedUserId}, '{tempDate}', '{startDateText}', '{stopDateText}', {false});");

                MySQLConnector.ExecuteWrite(query);

                query = string.Format($"select {SZSTWST_Shared.SqlTableName.AssignmentTable._idColumn} from {SZSTWST_Shared.SqlTableName.AssignmentTable._tableName}" +
                    $" where {SZSTWST_Shared.SqlTableName.AssignmentTable._idCreatorUserColumn}={_authorizedUserId} " +
                    $"and {SZSTWST_Shared.SqlTableName.AssignmentTable._creationDateColumn} = '{tempDate}' order by {SZSTWST_Shared.SqlTableName.AssignmentTable._creationDateColumn} desc limit 1;");
                int tempAssignmentId = MySQLConnector.ExecuteReadCommandInt(query).FirstOrDefault();

                foreach (int assetId in assetIds)
                {
                    query = string.Format($"INSERT INTO {SZSTWST_Shared.SqlTableName.AssignmentAssetTable._tableName}" +
                        $"({SZSTWST_Shared.SqlTableName.AssignmentAssetTable._idAssignmentColumn}, {SZSTWST_Shared.SqlTableName.AssignmentAssetTable._idAssetColumn})" +
                        $" VALUES({tempAssignmentId}, {assetId});");
                    MySQLConnector.ExecuteWrite(query);
                }

                return string.Format(TCPServerCommand.CreateNewAssignmentResponse, (int)Enums.OperationResult.Success, string.Empty);
            }

            else if (Regex.IsMatch(recievedText, TCPClientCommand.GetAssignments))
            {
                int? selectedUserId = int.Parse(Regex.Match(recievedText, ServerRegexPattern.GetAssignments).Groups[1].Value);
                if (selectedUserId != null && dataHolder.UserList.Exists(u => u.Id == selectedUserId))
                {
                    string command = string.Format($"Select * from {SZSTWST_Shared.SqlTableName.AssignmentTable._tableName}" +
                    $" Where {SZSTWST_Shared.SqlTableName.AssignmentTable._idUserColumn}={selectedUserId};");

                    List<SZSTWST_Shared.SqlTable.AssignmentTable> assignmentTable = MySQLConnector.ExecuteReadCommandAssignment(command).ToList();

                    command = JsonSerializer.Serialize(assignmentTable);

                    return string.Format(TCPServerCommand.GetAssignmentsResponse, command);
                }
            }

            else if (Regex.IsMatch(recievedText, ServerRegexPattern.GetAssignmentDetail))
            {
                int? AssignmentId = int.Parse(Regex.Match(recievedText, ServerRegexPattern.GetAssignmentDetail).Groups[1].Value);
                if (AssignmentId != null)
                {
                    string tempString = $"Select * from {SZSTWST_Shared.SqlTableName.AssignmentAssetTable._tableName}" +
                       $" where {SZSTWST_Shared.SqlTableName.AssignmentAssetTable._idAssignmentColumn}={AssignmentId};";

                    List<SZSTWST_Shared.SqlTable.AssignmentAssetTable> assignmentAssets = MySQLConnector.ExecuteReadCommandAssignmentAsset(tempString).ToList();
                    tempString = string.Empty;
                    if (assignmentAssets.Count > 0)
                    {
                        List<int> assetIds = assignmentAssets.Select(aa => aa.IdAsset).ToList();
                        tempString = JsonSerializer.Serialize(assetIds);
                    }
                    return string.Format(TCPServerCommand.GetAssignmentDetailResponse, tempString);
                }
                return string.Empty;
            }

            else if (Regex.IsMatch(recievedText, ServerRegexPattern.FinishAssignment))
            {
                int assignmentId = int.Parse(Regex.Match(recievedText, ServerRegexPattern.FinishAssignment).Groups[1].Value);
                string tempString = $"select * from {SZSTWST_Shared.SqlTableName.AssignmentTable._tableName}" +
                    $" where {SZSTWST_Shared.SqlTableName.AssignmentTable._idColumn} = {assignmentId}";

                SZSTWST_Shared.SqlTable.AssignmentTable assignment = MySQLConnector.ExecuteReadCommandAssignment(tempString).First();

                if (assignment != null)
                {
                    if (!assignment.IsFinished && (_isUserAdmin || _authorizedUserId == assignment.IdUser || _authorizedUserId == assignment.IdCreatorUser))
                    {
                        tempString = $"Update {SZSTWST_Shared.SqlTableName.AssignmentTable._tableName}" +
                            $" set {SZSTWST_Shared.SqlTableName.AssignmentTable._isFinishedColumn} = {true}," +
                            $" {SZSTWST_Shared.SqlTableName.AssignmentTable._finishDateColumn} = '{DateTime.Now.ToString(StringFormater.DateTimeFormat)}'," +
                            $" {SZSTWST_Shared.SqlTableName.AssignmentTable._idFinishedByUserColumn} = {_authorizedUserId}" +
                            $" where {SZSTWST_Shared.SqlTableName.AssignmentTable._idColumn} = '{assignment.Id}'";
                        MySQLConnector.ExecuteWrite(tempString);
                        return string.Format(TCPServerCommand.FinishAssignmentResponse, (int)Enums.OperationResult.Success);
                    }
                    return string.Format(TCPServerCommand.FinishAssignmentResponse, (int)Enums.OperationResult.Failure);
                }
                return string.Empty;
            }

            #endregion Assignment Operations

            else if (Regex.IsMatch(recievedText, TCPClientCommand.GetListOfUsers))
            {
                return string.Format(TCPServerCommand.ListOfUsers, JsonSerializer.Serialize(dataHolder.UsersString));
            }

            return TCPServerCommand.InvalidCommand;
        }

        public void SendResponse(string response)
        {
            if (!string.IsNullOrEmpty(response))
            {
                byte[] tempBytes = Encoding.Unicode.GetBytes(response);
                byte[] length = BitConverter.GetBytes((Int16)tempBytes.Length);
                byte[] sendBytes = new byte[length.Length + tempBytes.Length];
                sendBytes[0] = length[0];
                sendBytes[1] = length[1];
                for (int i = 2, j = 0; i < sendBytes.Length; i++)
                {
                    sendBytes[i] = tempBytes[j++];
                }
                _networkStream.WriteAsync(sendBytes, 0, sendBytes.Length);
                _networkStream.Flush();
            }
        }
    }
}
