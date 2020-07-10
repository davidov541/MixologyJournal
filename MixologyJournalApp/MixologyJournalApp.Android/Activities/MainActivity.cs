using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MixologyJournalApp.Droid.Platform;
using MixologyJournalApp.Platform;
using System;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid
{
    [Activity(Label = "MixologyJournalApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private App _app;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);

            _app = App.Create(new AndroidPlatform(this));

            await _app.InitAsync();

            LoadApplication(_app);

            await _app.LoadAsync();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private TaskCompletionSource<User> LoginCompletionState;

        internal async Task<User> RunLoginActivity()
        {
            LoginCompletionState = new TaskCompletionSource<User>();
            Intent intent = new Intent(Application.Context, typeof(LoginActivity));
            intent.PutExtra(LoginActivity.ModeKey, LoginActivity.LoginActivityMode);
            StartActivityForResult(intent, 100);

            User result = await LoginCompletionState.Task;
            LoginCompletionState = null;
            return result;
        }

        internal async Task<User> RunRenewalActivity(String renewalToken)
        {
            LoginCompletionState = new TaskCompletionSource<User>();
            Intent intent = new Intent(Application.Context, typeof(LoginActivity));
            intent.PutExtra(LoginActivity.ModeKey, LoginActivity.RenewalActivityMode);
            intent.PutExtra(LoginActivity.RenewalToken, renewalToken);
            StartActivityForResult(intent, 100);

            User result = await LoginCompletionState.Task;
            LoginCompletionState = null;
            return result;
        }

        internal async Task<User> RunLogOffActivity()
        {
            LoginCompletionState = new TaskCompletionSource<User>();
            Intent intent = new Intent(Application.Context, typeof(LoginActivity));
            intent.PutExtra(LoginActivity.ModeKey, LoginActivity.RenewalActivityMode);
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
                if (data != null)
                {
                    String name = data.GetStringExtra("name");
                    String iconPath = data.GetStringExtra("iconPath");
                    String authToken = data.GetStringExtra("authToken");
                    String refreshToken = data.GetStringExtra("refreshToken");
                    User result = new User(name, new Uri(iconPath), authToken, refreshToken);
                    LoginCompletionState.SetResult(result);
                }
                else
                {
                    LoginCompletionState.SetResult(null);
                }
            }
        }
    }
}