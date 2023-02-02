using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;

namespace SZSTWST_MobileApp.Adapters
{
    public class AssignmentDetailAssetListAdapter : RecyclerView.Adapter
    {
        List<AssetTable> assetList = new List<AssetTable>();

        public AssignmentDetailAssetListAdapter(List<AssetTable> assetList)
        {
            this.assetList = assetList;
        }

        public override int ItemCount => assetList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AssignmentDetailAssetListViewHolder ListHolder = (AssignmentDetailAssetListViewHolder)holder;

            ListHolder.AssetNameTextView.Text = assetList[position].Name;
            ListHolder.AssetCodeNameTextView.Text = assetList[position].CodeName;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AssignmentDetailAssetListItemLayout, parent, false);
            return new AssignmentDetailAssetListViewHolder(view);
        }
    }

    public class AssignmentDetailAssetListViewHolder : RecyclerView.ViewHolder
    {
        public TextView AssetNameTextView;
        public TextView AssetCodeNameTextView;

        public AssignmentDetailAssetListViewHolder(View itemView) : base(itemView)
        {
            AssetNameTextView = itemView.FindViewById<TextView>(Resource.Id.AssignmentDetailAssetNameTextView);
            AssetNameTextView.Selected = true;
            AssetCodeNameTextView = itemView.FindViewById<TextView>(Resource.Id.AssignmentDetailAssetCodeNameTextView);
        }
    }
}