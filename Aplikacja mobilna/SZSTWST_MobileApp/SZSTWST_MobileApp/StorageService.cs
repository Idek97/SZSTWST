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
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SZSTWST_MobileApp
{
    public static class StorageDataService
    {
        public static async Task StorageDataAsync(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (System.Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>If key isn't exist then return null</returns>
        public static async Task<string> GetDataAsync(string key)
        {
            string value = string.Empty;
            try
            {
                value = await SecureStorage.GetAsync(key);
            }
            catch (System.Exception ex)
            {
                value = string.Empty;
                // Possible that device doesn't support secure storage on device.
            }
            return value;
        }
    }
}