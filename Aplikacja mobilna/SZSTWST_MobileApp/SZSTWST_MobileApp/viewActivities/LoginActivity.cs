using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;
using Xamarin.Essentials;

namespace SZSTWST_MobileApp
{
    [Activity(Label = "@string/app_name", Theme ="@style/AppTheme", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        AutoCompleteTextView LoginTextView;
        EditText PasswordTextView;
        CheckBox RememberLoginCheckBox;
        Button LoginButton;

        string _userLogin;
        string _userPassword;

        TcpService _tcpService;
        private Thread _tcpCallbackThread;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.LoginLayout);
            

            LoginTextView = FindViewById<AutoCompleteTextView>(Resource.Id.LoginTextView);
            PasswordTextView = FindViewById<EditText>(Resource.Id.PasswordTextView);
            RememberLoginCheckBox = FindViewById<CheckBox>(Resource.Id.RememberLoginCheckBox);
            LoginButton = FindViewById<Button>(Resource.Id.LoginButton);

            _tcpService = TcpService.GetInstance();
            if (_tcpService.networkAccess == NetworkAccess.Internet
                || _tcpService.networkAccess == NetworkAccess.ConstrainedInternet
                || _tcpService.networkAccess == NetworkAccess.Local)
            {
                if (_tcpService._tcpClient.Connected)
                    LoginButton.Enabled = true;
            }

            string rememberLoginValue = StorageDataService.GetDataAsync("RememberLogin").Result;
            if (!string.IsNullOrEmpty(rememberLoginValue))
            {
                RememberLoginCheckBox.Checked = true;
                LoginTextView.Text = StorageDataService.GetDataAsync("RememberedLogin").Result;
            }

            LoginButton.Click += (sender, e) =>
            {
                if (LoginTextView.Text != null && LoginTextView.Text.Trim().Length > 0 && PasswordTextView.Text != null && PasswordTextView.Text.Trim().Length > 0)
                {
                    if (RememberLoginCheckBox.Checked)
                    {
                        StorageDataService.StorageDataAsync("RememberLogin", "true");
                        StorageDataService.StorageDataAsync("RememberedLogin", LoginTextView.Text);
                    }
                    else
                    {
                        SecureStorage.Remove("RememberLogin");
                        SecureStorage.Remove("RememberedLogin");
                    }
                    _userLogin = LoginTextView.Text;
                    _userPassword = PasswordTextView.Text;
                    _tcpService.SendTcpCommand(string.Format(TCPClientCommand.Login, _userLogin, _userPassword));

                    _userLogin = string.Empty;
                    _userPassword = string.Empty;
                    PasswordTextView.Text = string.Empty;
                }
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [Register("onDestroy", "()V", "GetOnDestroyHandler")]
        protected override void OnDestroy()
        {
            _tcpService.KillTcpConnection();
            base.OnDestroy();
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }
        public void TcpCallback()
        {
            while (true)
            {
                if (_tcpService._recievedMessege != null)
                {
                    if (Regex.IsMatch(_tcpService._recievedMessege, ClientRegexPattern.LoginAccepted))
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, "Successfully logged in", ToastLength.Short).Show();
                        });

                        Intent intent = new Intent(this, typeof(MenuActivity));
                        StartActivity(intent);
                    }

                    else if (_tcpService._recievedMessege.Equals(TCPServerCommand.LoginDenied))
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, "Wrong Login or Password!", ToastLength.Short).Show();
                        });
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
