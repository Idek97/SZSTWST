using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Apache.Http.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using SZSTWST_Shared;
using SZSTWST_Shared.SqlTable;

namespace SZSTWST_MobileApp.viewActivities
{
    [Activity(Label = "Create New Asset", Theme = "@style/AppTheme")]
    public class AddEditAssetActivity : Activity
    {
        TcpService _tcpService;
        private Thread _tcpCallbackThread;
        AssetTable thisAsset;
        AssetTable newAsset;
        Enums.AssetOperation operationEnum;

        TextView AssetName;
        TextView AssetCodeName;
        TextView AssetSerialNumber;
        TextView AssetDescription;
        CheckBox IsActive;
        Button ConfirmOperationButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            operationEnum = (Enums.AssetOperation)Intent.GetIntExtra("AssetOperation", 0);
            SetContentView(Resource.Layout.AssetFormLayout);

            _tcpService = TcpService.GetInstance();

            AssetName = FindViewById<TextView>(Resource.Id.AssetFormNameTextView);
            AssetCodeName = FindViewById<TextView>(Resource.Id.AssetFormCodeNameTextView);
            AssetSerialNumber = FindViewById<TextView>(Resource.Id.AssetFormSerialNumberTextView);
            AssetDescription = FindViewById<TextView>(Resource.Id.AssetFormDescriptionTextView);
            IsActive = FindViewById<CheckBox>(Resource.Id.AssetFormIsActiveCheckBox);
            ConfirmOperationButton = FindViewById<Button>(Resource.Id.AssetFormSaveButton);

            if (operationEnum == Enums.AssetOperation.EditAsset)
            {
                thisAsset = _tcpService.assetTableList.Find(a => a.Id == Intent.GetIntExtra("AssetId", 0));
                Title = $"Edit Asset with Id: {thisAsset.Id}";


                AssetName.Text = thisAsset.Name;
                AssetCodeName.Text = thisAsset.CodeName;
                AssetSerialNumber.Text = thisAsset.SerialNumber;
                AssetDescription.Text = thisAsset.Description;
                IsActive.Checked = thisAsset.IsActive;
                ConfirmOperationButton.Text = "Save Changes";
            }

            ConfirmOperationButton.Click += (sender, e) =>
            {
                newAsset = thisAsset == null ? new AssetTable() : thisAsset;

                if (AssetName.Text.Trim().Length > 0)
                    newAsset.Name = AssetName.Text.Trim().Length > 0 ? AssetName.Text.Trim() : string.Empty;
                newAsset.CodeName = AssetCodeName.Text.Trim().Length > 0 ? AssetCodeName.Text.Trim() : string.Empty;
                newAsset.SerialNumber = AssetSerialNumber.Text.Trim().Length > 0 ? AssetSerialNumber.Text.Trim() : null;
                newAsset.Description = AssetDescription.Text.Trim().Length > 0 ? AssetDescription.Text.Trim() : null;
                newAsset.IsActive = IsActive.Checked;

                string tempString = JsonSerializer.Serialize(newAsset);

                if (newAsset.Name.Length > 0 && newAsset.CodeName.Length > 0)
                {
                    _tcpService.SendTcpCommand(string.Format(TCPClientCommand.AssetOperation, (int)operationEnum, tempString));

                    Toast.MakeText(this, "Operation has been sent!", ToastLength.Short).Show();
                }

                else
                    Toast.MakeText(this, "Name or Codename textboxes are empty!", ToastLength.Short).Show();
                ConfirmOperationButton.Enabled = false;
            };

            #region Required TextViews Events

            AssetName.FocusChange += (sender, e) => { RequiredTextBoxCheckEvent(); };
            AssetCodeName.FocusChange += (sender, e) => { RequiredTextBoxCheckEvent(); };
            AssetSerialNumber.FocusChange += (sender, e) => { RequiredTextBoxCheckEvent(); };
            AssetDescription.FocusChange += (sender, e) => { RequiredTextBoxCheckEvent(); };
            IsActive.FocusChange += (sender, e) => { RequiredTextBoxCheckEvent(); };

            #endregion
        }

        public void RequiredTextBoxCheckEvent()
        {
            if (AssetName.Text.Trim().Length > 0 && AssetCodeName.Text.Trim().Length > 0)
                ConfirmOperationButton.Enabled = true;
            else
                ConfirmOperationButton.Enabled = false;
        }

        #region Activity Overrided Methods

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

        #endregion Activity Overrided Methods

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

                    else if (Regex.IsMatch(_tcpService._recievedMessege, ClientRegexPattern.AssetOperationResponse))
                    {
                        int temp = int.Parse(Regex.Match(_tcpService._recievedMessege, ClientRegexPattern.AssetOperationResponse).Groups[2].Value);
                        if (temp==(int)Enums.OperationResult.Success)
                        {
                            _tcpService.SendTcpCommand(TCPClientCommand.RefreshAssetList);
                            temp = int.Parse(Regex.Match(_tcpService._recievedMessege, ClientRegexPattern.AssetOperationResponse).Groups[1].Value);
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this,temp==(int)Enums.AssetOperation.EditAsset
                                ? "The asset has been successfully modified." : "The asset has been successfully created.",
                                    ToastLength.Short).Show();
                            });

                            Intent intent = new Intent(this, typeof(AssetListActivity));
                            StartActivity(intent);
                        }
                        else
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "There is already asset with this Codename!", ToastLength.Short).Show();
                            });
                    }

                    else if (Regex.IsMatch(_tcpService._recievedMessege, ClientRegexPattern.UniversalToast))
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this,
                                Regex.Match(_tcpService._recievedMessege, ClientRegexPattern.UniversalToast).Groups[1].Value,
                                ToastLength.Short).Show();
                        });

                    _tcpService._recievedMessege = null;
                }
            }
        }
    }


}