using Android.App;
using Android.Content;
//using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;

namespace SZSTWST_MobileApp.Adapters
{
    public class AssignmentListAdapter : RecyclerView.Adapter
    {
        List<AssignmentTable> assignmentList = new List<AssignmentTable>();

        public event EventHandler<int> DetailAssignmentEvent;

        public AssignmentListAdapter(List<AssignmentTable> assignmentList)
        {
            this.assignmentList = assignmentList;
        }

        public override int ItemCount => assignmentList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AssignmentListHolder listHolder = (AssignmentListHolder)holder;

            listHolder.thisAssignment = assignmentList[position];
            Debug.WriteLine($"{listHolder.thisAssignment.Id}, {listHolder.thisAssignment.IsFinished}, {position}");
            listHolder.AssignmentNameTextView.Text = string.Format($"Assignment No. {listHolder.thisAssignment.Id}");

            if (listHolder.thisAssignment.IsFinished)
                listHolder.AssignmentNameTextView.SetTextColor(Android.Graphics.Color.Green);

            else if (listHolder.thisAssignment.StopDate < DateTime.Now)
                listHolder.AssignmentNameTextView.SetTextColor(Android.Graphics.Color.Red);

            else
                listHolder.AssignmentNameTextView.SetTextColor(Android.Graphics.Color.White);

            listHolder.StartDateTimeTextView.Text = string.Format(StringFormater.StartDate,
                listHolder.thisAssignment.StartDate.ToString(StringFormater.ShortDateTimeFormat));
            listHolder.StopDateTimeTextView.Text = string.Format(StringFormater.StopDate,
                listHolder.thisAssignment.StopDate.ToString(StringFormater.ShortDateTimeFormat));

            listHolder.AssignmentDetailButton.Click += (sender, args) =>
            {
                DetailAssignmentEvent.Invoke(this, listHolder.thisAssignment.Id);
            };
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AssignmentListItemLayout, parent, false);
            return new AssignmentListHolder(view);
        }
    }

    public class AssignmentListHolder : RecyclerView.ViewHolder
    {
        public TextView AssignmentNameTextView;
        public TextView StartDateTimeTextView;
        public TextView StopDateTimeTextView;
        public ImageButton AssignmentDetailButton;

        public AssignmentTable thisAssignment;

        public AssignmentListHolder(View itemView) : base(itemView)
        {
            AssignmentNameTextView = itemView.FindViewById<TextView>(Resource.Id.AssignmentNameTextView);
            AssignmentNameTextView.Selected = true;

            StartDateTimeTextView = itemView.FindViewById<TextView>(Resource.Id.AssigmentStartDateTimeTextView);
            StopDateTimeTextView = itemView.FindViewById<TextView>(Resource.Id.AssigmentStopDateTimeTextView);

            AssignmentDetailButton = itemView.FindViewById<ImageButton>(Resource.Id.AssigmentDetailButton);
        }
    }
}