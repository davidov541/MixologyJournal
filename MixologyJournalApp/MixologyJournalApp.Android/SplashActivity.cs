using Android.App;
using Android.Content;

namespace MixologyJournalApp.Droid
{
    [Activity(Label = "SplashActivity", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity: global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}