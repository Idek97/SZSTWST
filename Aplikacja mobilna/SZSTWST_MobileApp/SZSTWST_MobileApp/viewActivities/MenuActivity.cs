using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System.Threading;
using SZSTWST_MobileApp.viewActivities;
using SZSTWST_Shared;

namespace SZSTWST_MobileApp
{
    [Activity(Label = "MenuActivity", Theme = "@style/AppThemeWithoutTitleBar")]
    public class MenuActivity : AppCompatActivity
    {
        Button NewAssignmentButton;
        Button MyAssignmentButton;
        Button FixedAssetsListButton;
        Button LogoutButton;

        TcpService _tcpService;
        private Thread _tcpCallbackThread;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MenuLayout);

            NewAssignmentButton = FindViewById<Button>(Resource.Id.NewAssignmentButton);
            MyAssignmentButton = FindViewById<Button>(Resource.Id.MyAssignmentButton);
            FixedAssetsListButton = FindViewById<Button>(Resource.Id.FixedAssetsListButton);
            LogoutButton = FindViewById<Button>(Resource.Id.LogoutButton);

            NewAssignmentButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(NewAssignmentActivity));
                StartActivity(intent);
            };

            MyAssignmentButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(MyAssignmentActivity));
                StartActivity(intent);
            };

            FixedAssetsListButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(AssetListActivity));
                StartActivity(intent);
            };

            LogoutButton.Click += (sender, e) =>
            {
                OnBackPressed();
            };

            _tcpService = TcpService.GetInstance();
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
            _tcpService.SendTcpCommand(TCPClientCommand.Logout);

            Intent intent = new Intent(this, typeof(LoginActivity));
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