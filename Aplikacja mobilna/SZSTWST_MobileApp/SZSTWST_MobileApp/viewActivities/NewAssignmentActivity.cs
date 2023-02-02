using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using SZSTWST_MobileApp.Adapters;
using SZSTWST_MobileApp.viewActivities;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;
using ZXing.Mobile;

namespace SZSTWST_MobileApp
{
    [Activity(Label = "Create New Assignment", Theme = "@style/AppTheme")]
    public class NewAssignmentActivity : Activity
    {
        Button ScanButton;
        RecyclerView SelectedItemsListRecyclerView;
        Button CreateRequestButton;
        Button startDateButton;
        Button stopDateButton;
        Spinner UserSelectSpinner;

        TcpService _tcpService;
        private Thread _tcpCallbackThread;

        MobileBarcodeScanner scanner;
        NewAssignmentAssetListAdapter adapter;

        List<AssetTable> selectedAssets = new List<AssetTable>();
        DateTime startDate = DateTime.MinValue;
        DateTime stopDate = DateTime.MinValue;

        int selectedUserId;
        ArrayAdapter spinnerAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.NewAssignmentLayout);

            _tcpService = TcpService.GetInstance();

            MobileBarcodeScanner.Initialize(Application);
            ScanButton = FindViewById<Button>(Resource.Id.scanButton);
            CreateRequestButton = FindViewById<Button>(Resource.Id.createRequestButton);
            startDateButton = FindViewById<Button>(Resource.Id.startDateButton);
            stopDateButton = FindViewById<Button>(Resource.Id.stopDateButton);

            if (_tcpService.assetTableList != null && _tcpService.assetTableList.Count > 0)
                ScanButton.Enabled = true;
            else
            {
                _tcpService.SendTcpCommand(TCPClientCommand.GetAssetList);
                Intent intent = new Intent(this, typeof(MenuActivity));
                StartActivity(intent);
                Toast.MakeText(this, "There is no data on the available assortment.\nPlease try again or contact your database manager!", ToastLength.Long).Show();
            }

            SelectedItemsListRecyclerView = FindViewById<RecyclerView>(Resource.Id.newAssignmentItemsList);
            SelectedItemsListRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            adapter = new NewAssignmentAssetListAdapter(selectedAssets);
            adapter.assetsListSizeEvent += AssetTableListEvent;
            SelectedItemsListRecyclerView.SetAdapter(adapter);

            UserSelectSpinner = FindViewById<Spinner>(Resource.Id.UserSelectSpinner);
            selectedUserId = _tcpService._userId;
            List<string> strings = new List<string>();
            foreach (var item in _tcpService.userStrings)
            {
                strings.Add(item.Value);
            }

            spinnerAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, strings);
            UserSelectSpinner.Adapter = spinnerAdapter;
            UserSelectSpinner.SetSelection(spinnerAdapter.GetPosition(_tcpService.userStrings.Find(u => u.Key == selectedUserId).Value));
            if (!_tcpService._isAdmin)
                UserSelectSpinner.Enabled = false;
            UserSelectSpinner.ItemSelected += (sender, e) =>
            {
                selectedUserId = _tcpService.userStrings[e.Position].Key;
            };

            scanner = new MobileBarcodeScanner();

            startDateButton.Click += StartDateSelect_OnClick;

            stopDateButton.Click += StopDateSelect_OnClick;

            ScanButton.Click += async (sender, e) =>
            {
                var options = new MobileBarcodeScanningOptions();
                options.PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.CODE_128 };
                var result = await scanner.Scan();

                AssetTable temp;
                if (result != null && !string.IsNullOrEmpty(result.Text))
                {
                    temp = _tcpService.assetTableList.Find(i => i.CodeName.Equals(result.Text));
                    if (temp != null && !selectedAssets.Contains(temp))
                    {
                        if (_tcpService._isAdmin == true || temp.IsActive)
                        {
                            selectedAssets.Add(temp);

                            AssetTableListEvent(this, 0);
                        }
                        else
                            Toast.MakeText(this, "This asset is Avaible only for Admin Users!", ToastLength.Short).Show();
                    }
                }
            };

            CreateRequestButton.Click += (sender, e) =>
            {
                if (selectedAssets.Count > 0)
                {
                    List<int> assetIds = selectedAssets.Select(o => o.Id).ToList();
                    string command = string.Format(TCPClientCommand.CreateNewAssignment, selectedUserId, startDate.ToString(), stopDate.ToString(), JsonSerializer.Serialize(assetIds));

                    Toast.MakeText(this, "Request Sended", ToastLength.Short).Show();
                    _tcpService.SendTcpCommand(command);
                }

            };
        }

        private void StartDateSelect_OnClick(object sender, EventArgs e)
        {
            DateTime tempDateTime = DateTime.MinValue;

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tempDateTime = time;

                TimePickerFragment timePickerDialog = TimePickerFragment.NewInstance(delegate (DateTime time)
                {
                    tempDateTime = tempDateTime.AddHours(time.Hour).AddMinutes(time.Minute);
                    if (stopDate == DateTime.MinValue || time <= stopDate)
                        startDateButton.Text = string.Format("Start: {0}", time.ToString(StringFormater.ShortDateTimeFormatInv));
                    startDate = time;
                    if (!startDate.Equals(DateTime.MinValue))
                        stopDateButton.Enabled = true;
                    AssetTableListEvent(this, 0);
                });
                timePickerDialog.Show(FragmentManager, TimePickerFragment.TAG);
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);

        }

        private void StopDateSelect_OnClick(object sender, EventArgs e)
        {
            DateTime tempDateTime = DateTime.MinValue;
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                tempDateTime = time;

                TimePickerFragment timePickerDialog = TimePickerFragment.NewInstance(delegate (DateTime time)
                {
                    tempDateTime = tempDateTime.AddHours(time.Hour).AddMinutes(time.Minute);

                    if (startDate <= tempDateTime)
                    {
                        stopDateButton.Text = string.Format("End: {0}", tempDateTime.ToString(StringFormater.ShortDateTimeFormatInv));
                        stopDate = tempDateTime;
                    }
                    else
                    {
                        Toast.MakeText(this, "End Date must be after a Start Date!", ToastLength.Short).Show();
                    }
                    AssetTableListEvent(this, 0);
                });
                timePickerDialog.Show(FragmentManager, TimePickerFragment.TAG);

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void AssetTableListEvent(object sender, int e)
        {
            adapter.NotifyDataSetChanged();
            CreateRequestButton.Enabled = selectedAssets.Count > 0 && (startDate != DateTime.MinValue) && (stopDate != DateTime.MinValue && stopDate >= startDate);
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

                    else if (System.Text.RegularExpressions.Regex.IsMatch(_tcpService._recievedMessege, ClientRegexPattern.CreateNewAssignmentResponse))
                    {
                        if (System.Text.RegularExpressions.Regex.Match(_tcpService._recievedMessege, ClientRegexPattern.CreateNewAssignmentResponse).Groups[1].Value.Equals("1"))
                        {
                            RunOnUiThread(() =>
                            {
                                selectedAssets.Clear();
                                startDate = DateTime.MinValue;
                                stopDate = DateTime.MinValue;
                                startDateButton.Text = "Start:";
                                stopDateButton.Text = "End: ";
                                CreateRequestButton.Enabled = false;
                                AssetTableListEvent(this, 0);

                                Toast.MakeText(this, "Assignment Added", ToastLength.Short).Show();
                            });

                            Intent intent = new Intent(this, typeof(MyAssignmentActivity));
                            StartActivity(intent);
                        }
                        else
                        {
                            List<int> ids = JsonSerializer.Deserialize<List<int>>(System.Text.RegularExpressions.Regex
                                .Match(_tcpService._recievedMessege, ClientRegexPattern.CreateNewAssignmentResponse).Groups[2].Value);
                            foreach (int id in ids)
                            {
                                AssetTable temp = selectedAssets.Find(i => i.Id.Equals(id));
                                selectedAssets.Remove(temp);
                            }

                            RunOnUiThread(() =>
                            {
                                if (ids.Count > 0)
                                    Toast.MakeText(this, "Error!\nAll items unavailable in the given time period have been removed from the list!", ToastLength.Short).Show();
                                else
                                {
                                    Toast.MakeText(this, "Error!\nYour account don't have permission to create assignments for other users!", ToastLength.Short).Show();
                                    selectedUserId = _tcpService._userId;
                                    UserSelectSpinner.SetSelection(spinnerAdapter.GetPosition(_tcpService.userStrings.Find(u => u.Key == selectedUserId).Value));
                                    UserSelectSpinner.Enabled=false;
                                }
                            });

                            RunOnUiThread(() =>
                            {
                                AssetTableListEvent(this, selectedAssets.Count);
                            });
                        }
                    }
                    _tcpService._recievedMessege = null;
                }
            }
        }
    }
}