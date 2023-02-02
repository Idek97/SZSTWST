using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using SZSTWST_Shared.SqlTable;

namespace SZSTWST_server
{
    public static class MySQLConnector
    {
        private static string _serverIp = "localhost";
        private static string _databaseName = "szstwst";
        private static string _databaseUserName = "root";
        private static string _databaseUserPassword = "zaq1@WSX";
        private static string _versionCommand = "select Version()";
        public static string ConnectionString
        {
            get
            {
                return string.Format("server={0};database={1};uid={2};pwd=\"{3}\"", _serverIp, _databaseName,
                    _databaseUserName, _databaseUserPassword);
            }
        }

        public static void CheckConnectionToDB()
        {
            using(MySqlConnection cnn=new MySqlConnection(ConnectionString))
            {
                Console.WriteLine(String.Format("DB is Connected, version: {0}", cnn.Query(_versionCommand).First()));
            }
        }

        public static IEnumerable<UserTable> ExecuteReadCommandUser(string command)
        {
            using (MySqlConnection cnn = new MySqlConnection(ConnectionString))
            {
                var returnList = cnn.Query<UserTable>(command);
                return returnList;
            }
        }

        public static IEnumerable<AssetTable> ExecuteReadCommandAsset(string command) 
        {
            using (MySqlConnection cnn = new MySqlConnection(ConnectionString))
            {
                var returnList = cnn.Query<AssetTable>(command);
                return returnList;
            }
        }

        public static IEnumerable<int> ExecuteReadCommandInt(string command)
        {
            using (MySqlConnection cnn = new MySqlConnection(ConnectionString))
            {
                var returnList = cnn.Query<int>(command);
                return returnList;
            }
        }

        public static IEnumerable<AssignmentTable> ExecuteReadCommandAssignment(string command)
        {
            using (MySqlConnection cnn = new MySqlConnection(ConnectionString))
            {
                var returnList = cnn.Query<AssignmentTable>(command);
                return returnList;
            }
        }

        public static IEnumerable<AssignmentAssetTable> ExecuteReadCommandAssignmentAsset(string command)
        {
            using (MySqlConnection cnn = new MySqlConnection(ConnectionString))
            {
                var returnList = cnn.Query<AssignmentAssetTable>(command);
                return returnList;
            }
        }

        public static void ExecuteWrite(string command)
        {
            using (MySqlConnection cnn = new MySqlConnection(ConnectionString))
            {
                cnn.Execute(command);
            }
        }
    }
}
