namespace SZSTWST_Shared.SqlTableName
{
    public static class UserTable
    {
        public static string _tableName = "users";
        public static string _idColumn = "id";
        public static string _nameColumn = "name";
        public static string _surnameColumn = "surname";
        public static string _phoneColumn = "phone";
        public static string _privateEmailColumn = "privateEmail";
        public static string _collegeEmailColumn = "collegeEmail";
        public static string _collegeIndexColumn = "collegeIndex";
        public static string _isAdminColumn = "isAdmin";
        public static string _passwordColumn = "password";
        public static string _createDateColumn = "createDate";
        public static string _updateDateColumn = "updateDate";
        public static string _isActiveColumn = "isActive";
    }

    public static class AssignmentTable
    {
        public static string _tableName = "assignment";
        public static string _idColumn = "id";
        public static string _idCreatorUserColumn = "idCreatorUser";
        public static string _idUserColumn = "idUser";
        public static string _creationDateColumn = "creationDate";
        public static string _startDateColumn = "startDate";
        public static string _stopDateColumn = "stopDate";
        public static string _isFinishedColumn = "isFinished";
        public static string _finishDateColumn = "finishDate";
        public static string _idFinishedByUserColumn = "idFinishedByUser";
    }

    public static class AssignmentAssetTable
    {
        public static string _tableName = "assignment_asset";
        public static string _idColumn = "id";
        public static string _idAssignmentColumn = "idAssignment";
        public static string _idAssetColumn = "idAsset";
    }

    public static class AssetTable
    {
        public static string _tableName = "asset";
        public static string _idColumn = "id";
        public static string _codeNameColumn = "codeName";
        public static string _serialNumberColumn = "serialNumber";
        public static string _nameColumn = "name";
        public static string _descriptionColumn = "description";
        public static string _isActiveColumn = "isActive";
        public static string _creationDateColumn = "creationDate";
    }
}
