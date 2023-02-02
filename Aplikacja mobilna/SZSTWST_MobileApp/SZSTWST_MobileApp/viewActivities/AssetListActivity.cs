using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using DotLiquid.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using SZSTWST_MobileApp.Adapters;
using SZSTWST_MobileApp.viewActivities;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;

namespace SZSTWST_MobileApp
{
    [Activity(Label = "List of Avaible Assets", Theme = "@style/AppTheme")]
    public class AssetListActivity : AppCompatActivity
    {
        RecyclerView AllAssetsRecyclerView;
        LinearLayout AdminSettings;
        CheckBox OnlyActiveCheckBox;
        Button CreateAssetButton;

        AssetListAdapter adapter;

        TcpService _tcpService;
        private Thread _tcpCallbackThread;
        List<AssetTable> showedAssetList = new List<AssetTable>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AssetListLayout);

            _tcpService = TcpService.GetInstance();
            if (_tcpService.assetTableList == null || _tcpService.assetTableList.Count == 0)
            {
                _tcpService.SendTcpCommand(TCPClientCommand.GetAssetList);
                Intent intent = new Intent(this, typeof(MenuActivity));
                StartActivity(intent);
                Toast.MakeText(this, "There is no data on the available assortment.\nPlease try again or contact your database manager!", ToastLength.Long).Show();
            }

            AdminSettings = FindViewById<LinearLayout>(Resource.Id.AssetListAdminSettingLayout);
            OnlyActiveCheckBox = FindViewById<CheckBox>(Resource.Id.OnlyActiveAssetsCheckBox);
            CreateAssetButton = FindViewById<Button>(Resource.Id.CreteAssetButton);
            if (_tcpService._isAdmin)
                AdminSettings.Visibility = Android.Views.ViewStates.Visible;
            CreateAssetButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(AddEditAssetActivity));
                intent.PutExtra("AssetOperation", (int)Enums.AssetOperation.CreateAsset);
                StartActivity(intent);
            };

            showedAssetList = _tcpService.assetTableList.FindAll(a => a.IsActive == true);

            AllAssetsRecyclerView = FindViewById<RecyclerView>(Resource.Id.AllAssetsRecyclerView);
            AllAssetsRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            adapter = new AssetListAdapter(showedAssetList);
            adapter.DetailAssetEvent += DetailAssetEvent;
            AllAssetsRecyclerView.SetAdapter(adapter);
            //adapter.NotifyDataSetChanged();

            OnlyActiveCheckBox.CheckedChange += (sender, e) =>
            {
                showedAssetList.Clear();
                adapter.NotifyDataSetChanged();
                if (OnlyActiveCheckBox.Checked)
                    showedAssetList.AddRange(_tcpService.assetTableList.FindAll(a => a.IsActive == true));
                else
                    showedAssetList.AddRange(_tcpService.assetTableList);
                adapter.NotifyDataSetChanged();
            };
        }

        private void DetailAssetEvent(object sender, int e)
        {
            AssetTable selectedAsset = showedAssetList.Find(a => a.Id == e);

            if (selectedAsset != null)
            {
                Intent intent = new Intent(this, typeof(AssetDetailActivity));
                string serializedObject = JsonSerializer.Serialize(selectedAsset);
                intent.PutExtra("assetData", serializedObject);
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

                    _tcpService._recievedMessege = null;
                }
            }
        }
    }
}