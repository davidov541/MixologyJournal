using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MixologyJournalApp.Droid.Platform;
using MixologyJournalApp.Platform;
using Plugin.GoogleClient;
using System;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid
{
    [Activity(Label = "MixologyJournalApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(
        new[] { Android.Content.Intent.ActionView },
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
        DataScheme = "com.auth0.quickstart",
        DataHost = "mixologyjournal.auth0.com",
        DataPathPrefix = "/android/com.auth0.quickstart/callback")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private App _app;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            GoogleClientManager.Initialize(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            _app = App.Create(new AndroidPlatform(this));
            LoadApplication(_app);

            await _app.LoadAsync();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private TaskCompletionSource<User> LoginCompletionState;

        internal async Task<User> StartLoginActivity()
        {
            LoginCompletionState = new TaskCompletionSource<User>();
            Intent intent = new Intent(Application.Context, typeof(LoginActivity));
            StartActivityForResult(intent, 100);

            User result = await LoginCompletionState.Task;
            LoginCompletionState = null;
            return result;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 100)
            {
                String name = data.GetStringExtra("name");
                String iconPath = data.GetStringExtra("iconPath");
                String authToken = data.GetStringExtra("authToken");
                String refreshToken = data.GetStringExtra("refreshToken");
                User result = new User(name, new Uri(iconPath), authToken, refreshToken);
                LoginCompletionState.TrySetResult(result);
            }
        }
    }
}