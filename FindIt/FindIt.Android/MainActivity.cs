﻿using Android.App;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Compass;
using Plugin.Compass.Abstractions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Threading.Tasks;

namespace FindIt.Droid
{
    [Activity(Label = "FindIt.Android", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        IGeolocator _locator = CrossGeolocator.Current;
        ICompass _compass = CrossCompass.Current;
        public Location LastKnownLocation;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            App.Init(_locator, _compass, (IAuthenticate)this);

            Xamarin.FormsGoogleMaps.Init(this, bundle); // initialize for Xamarin.Forms.GoogleMaps

            LoadApplication(new App());
        }

        protected async override void OnPause()
        {
            base.OnPause();

            await _locator.StopListeningAsync();
            _compass.Stop();
        }

        protected async override void OnResume()
        {
            base.OnResume();

            await _locator.StartListeningAsync(1, 1);
            _compass.Start(SensorSpeed.UI);
        }

        // Define a authenticated user.
        private MobileServiceUser user;

        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await ItemManager.DefaultManager.CurrentClient.LoginAsync(this,
                    MobileServiceAuthenticationProvider.Google, "findithack");
                if (user != null)
                {
                    message = string.Format("you are now signed-in as {0}.",
                        user.UserId);
                    success = true;
                }
                else
                {
                    message = "unable to sign in.";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            if (!success)
            {
                // Display the success or failure message.
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage(message);
                builder.SetTitle("Sign-in result");
                builder.Create().Show();
            }

            return success;
        }
    }
}