using Android.App;
using Android.Content;
using System;

namespace MixologyJournalApp.Droid
{
    [Activity(Label = "Mixology Journal", Icon = "@mipmap/icon", RoundIcon = "@mipmap/icon_round", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        // Launches the startup task
        protected override void OnResume()
        {
            try
            {
                base.OnResume();

                Xamarin.Forms.Forms.SetFlags("IndicatorView_Experimental");

                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Uncaught Exception: \n" + e.ToString());
                throw;
            }
        }
    }
}