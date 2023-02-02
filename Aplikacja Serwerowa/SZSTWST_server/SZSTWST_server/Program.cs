using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace SZSTWST_server
{
    class Server

    //hasło do bazy "SZSTWST_server"
    {
        public bool _isShutdown = false;
        static void Main(string[] args)
        {
            MySQLConnector.CheckConnectionToDB();
            TCPServerController tcpServer = new TCPServerController();
            tcpServer.SetupServer();
            Console.ReadLine();
        }
    }
}
