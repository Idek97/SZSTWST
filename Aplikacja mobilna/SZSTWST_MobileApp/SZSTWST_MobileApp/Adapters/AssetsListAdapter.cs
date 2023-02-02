using Android.Views;
using System;
using System.Collections.Generic;
using SZSTWST_Shared.SqlTable;
using AndroidX.RecyclerView.Widget;
using Android.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace SZSTWST_MobileApp.Adapters
{
    public class AssetListAdapter : RecyclerView.Adapter
    {
        public static List<AssetTable> assetsList = new List<AssetTable>();
        public override int ItemCount => assetsList.Count;

        public event EventHandler<int> DetailAssetEvent;

        public AssetListAdapter(List<AssetTable> currentAssetsList)
        {
            assetsList = currentAssetsList;
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            AssetListHolder listHolder = (AssetListHolder)holder;
            listHolder.thisAsset = assetsList[position];
            listHolder.AssetNameTextView.Text = assetsList[position].Name;
            listHolder.AssetDetailButton.Click += (sender, args) =>
            {
                DetailAssetEvent.Invoke(this, listHolder.thisAsset.Id);
            };
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AssetListItemLayout, parent, false);
            return new AssetListHolder(view);
        }
    }

    public class AssetListHolder : RecyclerView.ViewHolder
    {
        public TextView AssetNameTextView;
        public ImageButton AssetDetailButton;

        public AssetTable thisAsset;

        public AssetListHolder(View itemView) : base(itemView)
        {
            AssetNameTextView = itemView.FindViewById<TextView>(Resource.Id.AssetNameTextView);
            AssetNameTextView.Selected = true;
            AssetDetailButton = itemView.FindViewById<ImageButton>(Resource.Id.AssetDetailButton);
        }
    }
}