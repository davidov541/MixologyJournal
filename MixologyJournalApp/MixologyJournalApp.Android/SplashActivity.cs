using Android.App;
using Android.Content;
using Android.OS;
using Plugin.GoogleClient;

namespace MixologyJournalApp.Droid
{
    [Activity(Label = "SplashActivity", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity: global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            GoogleClientManager.Initialize(this);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}