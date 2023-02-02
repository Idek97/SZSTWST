using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using DotLiquid.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using SZSTWST_MobileApp.Adapters;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;

namespace SZSTWST_MobileApp.viewActivities
{
    [Activity(Label = "MyAssignmentActivity", Theme = "@style/AppThemeWithoutTitleBar")]
    public class MyAssignmentActivity : Activity
    {
        TcpService _tcpService;
        private Thread _tcpCallbackThread;

        List<AssignmentTable> _assignments = new List<AssignmentTable>();
        List<AssignmentTable> _visibleAssignments = new List<AssignmentTable>();

        RecyclerView AssignmentRecyclerView;
        CheckBox ShowFinishedCheckBox;
        Spinner SelectedUserSpinner;

        AssignmentListAdapter adapter;
        int selectedUserId;
        ArrayAdapter spinnerAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyAssignmentLayout);

            _tcpService = TcpService.GetInstance();
            selectedUserId = Intent.GetIntExtra("selectedUserId", _tcpService._userId);
            _tcpService.SendTcpCommand(string.Format(TCPClientCommand.GetAssignments, selectedUserId));

            SelectedUserSpinner = FindViewById<Spinner>(Resource.Id.SelectedUserSpinner);
            List<string> strings = new List<string>();
            foreach (var item in _tcpService.userStrings)
            {
                strings.Add(item.Value);
            }

            ShowFinishedCheckBox = FindViewById<CheckBox>(Resource.Id.ShowFinishedCheckBox);
            ShowFinishedCheckBox.Checked = Intent.GetBooleanExtra("showFinishedBool", false);

            spinnerAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, strings);
            SelectedUserSpinner.Adapter = spinnerAdapter;
            SelectedUserSpinner.SetSelection(spinnerAdapter.GetPosition(_tcpService.userStrings.Find(u => u.Key == selectedUserId).Value));
            
            SelectedUserSpinner.ItemSelected += (sender, e) =>
            {
                selectedUserId = _tcpService.userStrings[e.Position].Key;
                _tcpService.SendTcpCommand(string.Format(TCPClientCommand.GetAssignments, selectedUserId));
            };

            AssignmentRecyclerView = FindViewById<RecyclerView>(Resource.Id.MyAssignmentListRecyclerView);
            AssignmentRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            adapter = new AssignmentListAdapter(_visibleAssignments);
            adapter.DetailAssignmentEvent += DetailAssignmentEvent;
            AssignmentRecyclerView.SetAdapter(adapter);

            ShowFinishedCheckBox.CheckedChange += (sender, e) =>
                {
                    AssignmentListChangeEvent();
                };
        }

        private void AssignmentListChangeEvent()
        {

            RunOnUiThread(() =>
            {
                _visibleAssignments.Clear();
                adapter.NotifyDataSetChanged();

                if (_assignments.Count > 0)
                {
                    ShowFinishedCheckBox.Enabled = true;

                    _visibleAssignments.AddRange(_assignments.FindAll(a => a.IsFinished == false));
                    adapter.NotifyDataSetChanged();
                    if (ShowFinishedCheckBox.Checked)
                    {
                        List<AssignmentTable> invertedList = Enumerable.Reverse(_assignments).ToList();
                        _visibleAssignments.AddRange(invertedList.FindAll(a => a.IsFinished == true)); 
                    }

                    adapter.NotifyDataSetChanged();
                }
            });
        }

        private void DetailAssignmentEvent(object sender, int e)
        {
            AssignmentTable selectedAssignment = _visibleAssignments.Find(a => a.Id == e);

            if (selectedAssignment != null)
            {
                Intent intent = new Intent(this, typeof(AssigmentDetailActivity));
                string serializedObject = JsonSerializer.Serialize(selectedAssignment);
                intent.PutExtra("assignmentData", serializedObject);
                intent.PutExtra("selectedUserId", selectedUserId);
                intent.PutExtra("showFinishedBool", ShowFinishedCheckBox.Checked);
                StartActivity(intent);
            }
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
            Intent intent = new Intent(this, typeof(MenuActivity));
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

                    else if (Regex.IsMatch(_tcpService._recievedMessege, ClientRegexPattern.GetAssignmentsResponse))
                    {
                        string tempString = Regex.Match(_tcpService._recievedMessege, ClientRegexPattern.GetAssignmentsResponse).Groups[1].Value;

                        if (!string.IsNullOrEmpty(tempString))
                        {
                            try
                            {
                                _assignments = JsonSerializer.Deserialize<List<AssignmentTable>>(tempString).ToList();
                                AssignmentListChangeEvent();
                            }
                            catch(JsonException)
                            {
                                _tcpService.SendTcpCommand(string.Format(TCPClientCommand.GetAssignments, selectedUserId));
                            }
                        }
                    }

                    _tcpService._recievedMessege = null;
                }
            }
        }
    }
}