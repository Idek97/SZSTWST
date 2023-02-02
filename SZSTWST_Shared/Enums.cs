namespace SZSTWST_Shared
{
    public static class Enums
    {
        public enum HandleClientCommand
        {
            None,
            RemoveObject,
            RefreshAssetList,
            CheckUserIsRepeated
        }

        public enum AssetOperation
        {
            None,
            CreateAsset,
            EditAsset
        }

        public enum OperationResult
        {
            Failure,
            Success
        }

    }
}
