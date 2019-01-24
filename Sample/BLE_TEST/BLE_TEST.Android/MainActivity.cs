using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Content;
using Android;
using Android.Bluetooth;
using Android.Locations;
using Xamarin.Forms;
using System.Runtime.Remoting.Contexts;
using Android.Content;

namespace BLE_TEST.Droid
{
    [Activity(Label = "BLE_TEST", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        BluetoothManager _manager;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            _manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
            _manager.Adapter.Enable();
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            int add = 0;
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
            {
                add++;
            }
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                add++;
            } 
            if (add != 0)
            {
                Android.Support.V4.App.ActivityCompat.RequestPermissions(this,
                          new string[] {
                    Android.Manifest.Permission.AccessCoarseLocation,
                    Android.Manifest.Permission.AccessFineLocation,
                    Android.Manifest.Permission.Bluetooth,
                  }, 4);
            }
            OpenLocationSettings();
        }
        public void OpenLocationSettings()
        {


            LocationManager LM = (LocationManager)Forms.Context.GetSystemService(Android.Content.Context.LocationService);
            if (LM.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                AlertDialog ad = new AlertDialog.Builder(this).Create();

                ad.SetMessage("Please open location");
                ad.SetCancelable(false);
                ad.SetCanceledOnTouchOutside(false);
                ad.SetButton("ok", delegate
                {
                    Android.Content.Context ctx = Forms.Context;
                    ctx.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                });

                ad.SetButton2("cancle", delegate
                {

                });
                ad.Show();

            } 
        }
    }
}