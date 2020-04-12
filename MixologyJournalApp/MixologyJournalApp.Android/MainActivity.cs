using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Microsoft.WindowsAzure.MobileServices;
using MixologyJournalApp.Droid.Security;

namespace MixologyJournalApp.Droid
{
    [Activity(Label = "MixologyJournalApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            MobileServiceClient client = new MobileServiceClient("https://mixologyjournal.azurewebsites.net");
            client.LoginAsync(this, MobileServiceAuthenticationProvider.Google, "mixologyjournal");

            App.Init(new AuthenticationManager(this));

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}