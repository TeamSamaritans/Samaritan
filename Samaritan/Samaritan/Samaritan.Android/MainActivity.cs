using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Acr.UserDialogs;
using Plugin.Permissions;
using Android.Locations;
using Android.Runtime;
using System.Collections.Generic;
using System.Linq;
using Samaritan.Helper;
using Android.Content;

namespace Samaritan.Droid
{
    [Activity(Label = "Samaritan", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ILocationListener
    {
        private LocationManager locationManager;
        private string locationProvider;
        public double Latitude { get; set; }


        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            UserDialogs.Init(this);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            InitializeLocationManager();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var error = e.ExceptionObject;
        }

        private void InitializeLocationManager()
        {
            try
            {
                locationManager = (LocationManager)GetSystemService(LocationService);
                Criteria criteriaForLocationService = new Criteria
                {
                    Accuracy = Accuracy.Fine
                };
                IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);
                if (acceptableLocationProviders.Any())
                {
                    locationProvider = acceptableLocationProviders.First();
                }
                else
                {
                    locationProvider = string.Empty;
                }
            }
            catch (Exception)
            {
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();
            try
            {
                locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
            }
            catch (Exception)
            {

            }
        }
        public void OnLocationChanged(Location location)
        {
            AppConstant.Latitude = location.Latitude;
            AppConstant.Longitude = location.Longitude;
            AppConstant.Source = "MainActivity";
            locationManager.RemoveUpdates(this);
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
    }
}

