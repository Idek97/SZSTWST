using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;

namespace SZSTWST_MobileApp.viewActivities
{
    [Activity(Label = "Asset Detail", Theme = "@style/AppTheme")]
    public class AssetDetailActivity : Activity
    {
        AssetTable thisAsset;

        TextView AssetName;
        TextView AssetDescription;
        TextView AssetSerialNumber;
        ImageButton EditAsset;

        TcpService _tcpService;
        private Thread _tcpCallbackThread;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            thisAsset = JsonSerializer.Deserialize<AssetTable>(Intent.GetStringExtra("assetData"));
            SetContentView(Resource.Layout.AssetDetailLayout);
            _tcpService = TcpService.GetInstance();

            AssetName = FindViewById<TextView>(Resource.Id.NameDetailAssetTextView);
            AssetDescription = FindViewById<TextView>(Resource.Id.DescriptionDetailAssetTextView);
            AssetSerialNumber = FindViewById<TextView>(Resource.Id.SerialNumberDetailAssetTextView);

            AssetName.Text = thisAsset.Name;
            if (!string.IsNullOrEmpty(thisAsset.Description)) AssetDescription.Text = thisAsset.Description;
            if (!string.IsNullOrEmpty(thisAsset.SerialNumber)) AssetSerialNumber.Text = thisAsset.SerialNumber;

            EditAsset = FindViewById<ImageButton>(Resource.Id.AssetDetailEditButton);
            EditAsset.Visibility = _tcpService._isAdmin? Android.Views.ViewStates.Visible: Android.Views.ViewStates.Gone;

            EditAsset.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(AddEditAssetActivity));
                intent.PutExtra("AssetOperation", (int)Enums.AssetOperation.EditAsset);
                intent.PutExtra("AssetId", thisAsset.Id);
                StartActivity(intent);
            };
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
            Intent intent = new Intent(this, typeof(AssetListActivity));
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

                    else if (Regex.IsMatch(_tcpService._recievedMessege, TCPServerCommand.UniversalToast))
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, 
                                Regex.Match(_tcpService._recievedMessege, TCPServerCommand.UniversalToast).Groups[1].Value,
                                ToastLength.Short).Show();
                        });
                    }

                    _tcpService._recievedMessege = null;
                }
            }
        }
    }
}