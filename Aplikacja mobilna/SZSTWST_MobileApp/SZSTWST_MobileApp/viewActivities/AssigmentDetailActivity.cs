using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using SZSTWST_MobileApp.Adapters;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;
using AndroidX.RecyclerView.Widget;
using AndroidX.Annotations;
using System.Net.NetworkInformation;

namespace SZSTWST_MobileApp.viewActivities
{
    [Activity(Label = "Assigment Detail", Theme = "@style/AppTheme")]
    public class AssigmentDetailActivity : Activity
    {
        TcpService _tcpService;
        private Thread _tcpCallbackThread;

        AssignmentTable thisAssignment;
        List<AssetTable> assets = new List<AssetTable>();

        Button FinishAssignmentButton;
        Button GenerateAssignmentReport;
        TextView AssignmentStartDateTextView;
        TextView AssignmentStopDateTextView;
        TextView CreatedByUser;
        TextView ForUser;

        RecyclerView AssignmentDetailAssetListRecyclerView;
        AssignmentDetailAssetListAdapter adapter;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            thisAssignment = JsonSerializer.Deserialize<AssignmentTable>(Intent.GetStringExtra("assignmentData"));
            SetContentView(Resource.Layout.AssignmentDetailLayout);

            _tcpService = TcpService.GetInstance();
            _tcpService.SendTcpCommand(string.Format(TCPClientCommand.GetAssignmentDetail, thisAssignment.Id));

            FinishAssignmentButton = FindViewById<Button>(Resource.Id.FinishAssignmentButton);
            GenerateAssignmentReport = FindViewById<Button>(Resource.Id.GenerateAssignmentReport);
            AssignmentStartDateTextView = FindViewById<TextView>(Resource.Id.AssignmentDetailStartDateTextView);
            AssignmentStopDateTextView = FindViewById<TextView>(Resource.Id.AssignmentDetailStopDateTextView);

            CreatedByUser = FindViewById<TextView>(Resource.Id.CreatedByUserTextView);
            ForUser = FindViewById<TextView>(Resource.Id.ForUserTextView);
            CreatedByUser.Text = $"Created by User: {_tcpService.userStrings.Find(u => u.Key == thisAssignment.IdCreatorUser).Value}";
            ForUser.Text = $"For User:        {_tcpService.userStrings.Find(u => u.Key == thisAssignment.IdUser).Value}";

            AssignmentStartDateTextView.Text = string.Format(StringFormater.StartDate, thisAssignment.StartDate.ToString(StringFormater.ShortDateTimeFormat));
            AssignmentStopDateTextView.Text = string.Format(StringFormater.StopDate, thisAssignment.StopDate.ToString(StringFormater.ShortDateTimeFormat));
            AssignmentDetailAssetListRecyclerView = FindViewById<RecyclerView>(Resource.Id.AssignmentDetailAssetListRecyclerView);
            AssignmentDetailAssetListRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            adapter = new AssignmentDetailAssetListAdapter(assets);
            AssignmentDetailAssetListRecyclerView.SetAdapter(adapter);

            if (!thisAssignment.IsFinished)
                FinishAssignmentButton.Enabled = true;
            FinishAssignmentButton.Click += delegate
            {
                _tcpService.SendTcpCommand(string.Format(TCPClientCommand.FinishAssignment, thisAssignment.Id));
            };

            GenerateAssignmentReport.Click += async (sender, e) =>
            {
                await new AssignmentPDFReport(thisAssignment, assets, _tcpService.userStrings.Find(t => t.Key == thisAssignment.IdUser).Value,
                    _tcpService.userStrings.Find(t => t.Key == thisAssignment.IdCreatorUser).Value).CreateReport();
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _tcpCallbackThread = new Thread(TcpCallback);
            _tcpCallbackThread.Start();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _tcpCallbackThread.Abort();
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent(this, typeof(MyAssignmentActivity));
            intent.PutExtra("selectedUserId", Intent.GetIntExtra("selectedUserId", _tcpService._userId));
            intent.PutExtra("showFinishedBool", Intent.GetBooleanExtra("showFinishedBool", false));
            StartActivity(intent);
        }

        public void TcpCallback()
        {
            while (true)
            {
                if (_tcpService._recievedMessege != null)
                {
                    if (_tcpService._recievedMessege.Equals(TCPServerCommand.SessionTimeout))
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, "Session Expired!", ToastLength.Long).Show();
                        });
                        Intent intent = new Intent(this, typeof(LoginActivity));
                        StartActivity(intent);
                    }

                    else if (Regex.IsMatch(_tcpService._recievedMessege, ClientRegexPattern.FinishAssignmentResponse))
                    {
                        string toast = Regex.Match(_tcpService._recievedMessege, ClientRegexPattern.FinishAssignmentResponse).Groups[1].Value.Equals("1")
                                ? "The Assignment has been comleted Successfully!"
                                : "The Assignment is already Finished or you don't have permission to end this assignment!";
                        RunOnUiThread(() =>
                        {
                            FinishAssignmentButton.Enabled = false;
                            Toast.MakeText(this, toast, ToastLength.Short).Show();
                        });
                    }

                    else if (Regex.IsMatch(_tcpService._recievedMessege, ClientRegexPattern.GetAssignmentDetailResponse))
                    {
                        List<int> ids = JsonSerializer.Deserialize<List<int>>(Regex.Match(_tcpService._recievedMessege, ClientRegexPattern.GetAssignmentDetailResponse).Groups[1].Value);
                        RunOnUiThread(() =>
                        {
                            assets.Clear();
                            adapter.NotifyDataSetChanged();
                            assets.AddRange(_tcpService.assetTableList.Where(a => ids.Contains(a.Id)));
                            adapter.NotifyDataSetChanged();
                        });
                    }

                    _tcpService._recievedMessege = null;
                }
            }
        }
    }
}