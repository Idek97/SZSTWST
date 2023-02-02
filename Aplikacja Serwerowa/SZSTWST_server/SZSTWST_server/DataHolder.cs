using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;

namespace SZSTWST_server
{
    public sealed class DataHolder
    {
        public List<AssetTable> AssetList { get; set; }
        public List<UserTable> UserList { get; set; }
        public List<KeyValuePair<int, string>> UsersString { get; set; }

        private static DataHolder _instance;
        private static readonly object InstanceLock = new object();

        public static DataHolder GetInstance()
        {
            if (_instance == null)
            {
                lock (InstanceLock)
                {
                    if (_instance == null)
                        _instance = new DataHolder();
                }
            }
            return _instance;
        }


        public void FillUpAssetList()
        {
            IEnumerable<AssetTable> assetList = MySQLConnector.ExecuteReadCommandAsset(string.Format($"SELECT * FROM {SZSTWST_Shared.SqlTableName.AssetTable._tableName};"));
            AssetList = assetList.ToList();
        }

        public void FillUpUserList()
        {
            IEnumerable<UserTable> userList = MySQLConnector.ExecuteReadCommandUser(string.Format($"SELECT * FROM {SZSTWST_Shared.SqlTableName.UserTable._tableName};"));
            UserList = userList.ToList();
            UsersString = new List<KeyValuePair<int, string>>();
            foreach (UserTable user in UserList)
            {
                UsersString.Add(new KeyValuePair<int, string>(user.Id, string.Format($"{user.Name} {user.Surname}")));
            }
        }
    }
}
