using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace MixologyJournalApp.Droid.View
{
    [Activity(Theme = "@style/AppTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        private MixologyApplication _application;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();

            _application = Application as MixologyApplication;
            _application.SetNewActivity(this);
            Task startupWork = new Task(RunStartup);
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        private async void RunStartup()
        {
            await _application.App.InitializeAsync();
            Intent intent = new Intent(Application.Context, typeof(MainActivity));
            StartActivity(intent);
        }

        public override void OnBackPressed() { }
    }
}
