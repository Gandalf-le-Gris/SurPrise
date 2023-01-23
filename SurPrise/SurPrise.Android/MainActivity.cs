using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.Content;
using Android;
using AndroidX.Core.App;

namespace SurPrise.Droid
{
    [Activity(Label = "SurPrise", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            // Couleur de la barre supérieure pour les écrans étendus
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#3A5C22"));

            // Obtention de la permission Bluetooth
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission_group.BluetoothNetwork) == (int)Permission.Granted)
            {

            }
            else
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission_group.BluetoothNetwork }, 1);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}