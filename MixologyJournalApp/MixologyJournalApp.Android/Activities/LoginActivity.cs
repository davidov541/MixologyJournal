using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using MixologyJournalApp.Model;
using System;
using System.Threading.Tasks;
using System.Linq;
using IdentityModel.OidcClient.Browser;
using Android.Widget;
using MixologyJournalApp.Droid.Platform;

namespace MixologyJournalApp.Droid
{
    [Activity(Label = "Logging In...", MainLauncher = false, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MainTheme.Login")]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "net.davidmcginnis.mixologyjournal.android",
        DataHost = "mixologyjournal.auth0.com",
        DataPathPrefix = "/android/net.davidmcginnis.mixologyjournal.android/callback")]
    public class LoginActivity : Auth0ClientActivity
    {
        private Auth0Client _auth0Client;

        public const String ModeKey = "mode";
        public const String LoginActivityMode = "login";
        public const String RenewalActivityMode = "renewal";
        public const String LogOffActivityMode = "logoff";
        public const String RenewalToken = "renewalToken";

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.LoginScreen);
            TextView captionText = FindViewById<TextView>(Resource.Id.CaptionText);

            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = AppConfigManager.Settings["Auth0Domain"],
                ClientId = AppConfigManager.Settings["ClientID"],
                Scope = "openid profile email offline_access permissions read:recipes delete:recipes modify:recipes",
                LoadProfile = true
            });

            Intent result = null;
            switch (Intent.GetStringExtra(ModeKey))
            {
                case LoginActivityMode:
                    captionText.Text = "Logging In...";
                    result = await LoginAsync();
                    break;
                case RenewalActivityMode:
                    captionText.Text = "Retrieving User Information...";
                    String renewalToken = Intent.GetStringExtra(RenewalToken);
                    result = await RenewAsync(renewalToken);
                    break;
                case LogOffActivityMode:
                    captionText.Text = "Logging Off...";
                    result = await LogOffAsync();
                    break;
                default:
                    break;
            }
            SetResult(Android.App.Result.Ok, result);
            Finish();
       }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            ActivityMediator.Instance.Send(intent.DataString);
        }

        private async Task<Intent> LoginAsync()
        {
            LoginResult loginResult = await _auth0Client.LoginAsync();

            if (loginResult.IsError)
            {
                Console.WriteLine($"An error occurred during login: {loginResult.Error}");
                return null;
            }

#if DEBUG
            Console.WriteLine($"id_token: {loginResult.IdentityToken}");
            Console.WriteLine($"access_token: {loginResult.AccessToken}");
            Console.WriteLine($"refresh_token: {loginResult.RefreshToken}");

            Console.WriteLine($"name: {loginResult.User.FindFirst(c => c.Type == "name")?.Value}");
            Console.WriteLine($"email: {loginResult.User.FindFirst(c => c.Type == "email")?.Value}");

            foreach (var claim in loginResult.User.Claims)
            {
                Console.WriteLine($"{claim.Type} = {claim.Value}");
            }
#endif

            Intent result = new Intent();
            result.PutExtra("name", loginResult.User.FindFirst(c => c.Type == "name")?.Value);
            result.PutExtra("iconPath", loginResult.User.FindFirst(c => c.Type == "picture")?.Value);
            result.PutExtra("authToken", loginResult.IdentityToken);
            result.PutExtra("refreshToken", loginResult.RefreshToken);
            return result;
        }

        private async Task<Intent> RenewAsync(String renewalToken)
        {
            RefreshTokenResult refreshResult = await _auth0Client.RefreshTokenAsync(renewalToken);
            UserInfoResult user = await _auth0Client.GetUserInfoAsync(refreshResult.AccessToken);

#if DEBUG
            Console.WriteLine($"id_token: {refreshResult.IdentityToken}");
            Console.WriteLine($"access_token: {refreshResult.AccessToken}");
            Console.WriteLine($"refresh_token: {refreshResult.RefreshToken}");

            Console.WriteLine($"name: {user.Claims.First(c => c.Type == "name")?.Value}");
            Console.WriteLine($"email: {user.Claims.First(c => c.Type == "email")?.Value}");

            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"{claim.Type} = {claim.Value}");
            }
#endif

            Intent result = new Intent();
            result.PutExtra("name", user.Claims.First(c => c.Type == "name")?.Value);
            result.PutExtra("iconPath", user.Claims.First(c => c.Type == "picture")?.Value);
            result.PutExtra("authToken", refreshResult.IdentityToken);
            result.PutExtra("refreshToken", refreshResult.RefreshToken);
            return result;
        }

        private async Task<Intent> LogOffAsync()
        {
            BrowserResultType resultType = await _auth0Client.LogoutAsync();
            Intent result = new Intent();
            result.PutExtra("resultCode", resultType.ToString());
            return result;
        }
    }
}