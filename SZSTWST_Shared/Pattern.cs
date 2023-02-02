using System.Text;
using System;

namespace SZSTWST_Shared
{
    public static class StringFormater
    {
        public static string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public static string ShortDateTimeFormat= "yyyy/MM/dd HH:mm";
        public static string ShortDateTimeFormatInv = "HH:mm dd/MM/yyyy";
        public static string DateFormat = "dd/MM/yyyy";

        public static string StartDate = "Start:    {0}";
        public static string StopDate = "End:      {0}";
    }

    /// <summary>
    /// Commands sended by Client.
    /// </summary>
    public static class TCPClientCommand
    {
        public static string Login = "TryToLogin\n\"{0}\"\n\"{1}\"\n";
        public static string Logout = "Logout\n";
        public static string Disconnect = "Disconnect\n";
        public static string GetListOfUsers = "GetListOfUsers\n";

        public static string GetAssetList = "GetAssetList\n";
        public static string RefreshAssetList = "RefreshAssetList\n";
        public static string AssetOperation = "AssetOperation\n{0}\n{1}\n";

        public static string CreateNewAssignment = "CreateNewAssignment\n{0}\n{1}\n{2}\n{3}";
        public static string GetAssignments = "GetAssignments\n{0}";
        public static string GetAssignmentDetail = "GetAssignmentDetail\n{0}";
        public static string FinishAssignment = "FinishAssignment\n{0}";
    }

    /// <summary>
    /// Patterns used by Server to match Client's commands.
    /// </summary>
    public static class ServerRegexPattern
    {
        public static string TryToLogin = "TryToLogin\n\"(.+)\"\n\"(.+)\"\n";

        public static string AssetOperation = "AssetOperation\n(.)\n(.+)\n";

        public static string CreateNewAssignment = "CreateNewAssignment\n(.+)\n(.+)\n(.+)\n(.+)";
        public static string GetAssignments = "GetAssignments\n(.+)";
        public static string GetAssignmentDetail = "GetAssignmentDetail\n(.*)";
        public static string FinishAssignment = "FinishAssignment\n(.+)";
    }

    /// <summary>
    /// Patterns used by Client to match Server's commands.
    /// </summary>
    public static class ClientRegexPattern
    {
        public static string LoginAccepted = "TryToLogin:Accepted\n(.+)\n(.)";
        public static string UniversalToast = "UniversalToast\n(.+)";
        public static string ListOfUsers = "ListOfUsers\n(.+)";

        public static string CurrentAssetList = "CurrentAssetList\n(.+)";
        public static string AssetOperationResponse = "AssetOperationResponse\n(.)\n(.)\n";

        public static string CreateNewAssignmentResponse = "CreateNewAssignmentResponse\n(.)\n(.*)";
        public static string GetAssignmentsResponse = "GetAssignmentsResponse\n(.*)";
        public static string GetAssignmentDetailResponse = "GetAssignmentDetailResponse\n(.+)";
        public static string FinishAssignmentResponse = "FinishAssignmentResponse\n(.)";
    }

    /// <summary>
    /// Commands sended by Server.
    /// </summary>
    public static class TCPServerCommand
    {
        public static string LoginAccepted = "TryToLogin:Accepted\n{0}\n{1}";
        public static string LoginDenied = "TryToLogin:Denied\n";
        public static string InvalidCommand = "Invalid Command\n";
        public static string SessionTimeout = "SessionExpired\n";
        public static string Ping = "Ping\n";
        public static string UniversalToast = "UniversalToast\n{0}";
        public static string ListOfUsers = "ListOfUsers\n{0}";

        public static string CurrentAssetList = "CurrentAssetList\n{0}";
        public static string AssetOperationResponse = "AssetOperationResponse\n{0}\n{1}\n";

        public static string CreateNewAssignmentResponse = "CreateNewAssignmentResponse\n{0}\n{1}";
        public static string GetAssignmentsResponse = "GetAssignmentsResponse\n{0}";
        public static string GetAssignmentDetailResponse = "GetAssignmentDetailResponse\n{0}";
        public static string FinishAssignmentResponse = "FinishAssignmentResponse\n{0}";
        
    }

    /// <summary>
    /// Checking checksums
    /// </summary>
    public static class ChecksumService
    {
        public static string GetChecksum(string data)
        {
            string hash = string.Empty;
            if (data == null) return string.Empty;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = BitConverter.ToString(
                  md5.ComputeHash(Encoding.UTF8.GetBytes(data))
                ).Replace("-", String.Empty);
            }
            return hash;
        }
    }
}