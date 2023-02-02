using Android.Views;
using System;
using System.Collections.Generic;
using SZSTWST_Shared.SqlTable;
using AndroidX.RecyclerView.Widget;
using Android.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace SZSTWST_MobileApp.Adapters
{
    public class NewAssignmentAssetListAdapter : RecyclerView.Adapter
    {
        public static List<AssetTable> assetsList = new List<AssetTable>();
        public override int ItemCount => assetsList.Count;

        public event EventHandler<int> assetsListSizeEvent;

        public NewAssignmentAssetListAdapter(List<AssetTable> currentAssetsList)
        {
            assetsList = currentAssetsList;
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            NewAssignmentAssetListHolder listHolder = (NewAssignmentAssetListHolder)holder;
            listHolder.thisAsset = assetsList[position];
            listHolder.assetNameTextView.Text = assetsList[position].Name;

            listHolder.deleteAssetButton.Click += (sender, args) =>
            {
                assetsList.Remove(listHolder.thisAsset);
                assetsListSizeEvent.Invoke(this, assetsList.Count);
            };
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.NewAssignmentItemLayout, parent, false);
            return new NewAssignmentAssetListHolder(view);
        }
    }

    public class NewAssignmentAssetListHolder : RecyclerView.ViewHolder
    {
        public TextView assetNameTextView;
        public Button deleteAssetButton;

        public AssetTable thisAsset;

        public NewAssignmentAssetListHolder(View itemView) : base(itemView)
        {
            assetNameTextView = itemView.FindViewById<TextView>(Resource.Id.assetNameTextView);
            assetNameTextView.Selected = true;
            deleteAssetButton = itemView.FindViewById<Button>(Resource.Id.deleteAssetButton);
        }
    }
}