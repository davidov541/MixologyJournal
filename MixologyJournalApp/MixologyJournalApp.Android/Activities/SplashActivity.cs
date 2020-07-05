using Android.App;
using Android.Content;

namespace MixologyJournalApp.Droid
{
    [Activity(Label = "Mixology Journal", Icon = "@mipmap/icon", RoundIcon = "@mipmap/icon_round", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity: global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();

            Xamarin.Forms.Forms.SetFlags("IndicatorView_Experimental");

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}