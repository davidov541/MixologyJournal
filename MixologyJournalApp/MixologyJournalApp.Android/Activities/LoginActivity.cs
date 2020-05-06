using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using MixologyJournalApp.Model;
using System;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid
{
    [Activity(Label = "Logging In...", MainLauncher = false, LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "mixologyjournalapp.droid",
        DataHost = "mixologyjournal.auth0.com",
        DataPathPrefix = "/android/mixologyjournalapp.droid/callback")]
    public class LoginActivity : Auth0ClientActivity
    {
        private Auth0Client _auth0Client;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = AppConfigManager.Settings["Auth0Domain"],
                ClientId = AppConfigManager.Settings["ClientID"],
                Scope = "openid profile email offline_access"

            });

            Intent result = await LoginAsync(this, new EventArgs());
            SetResult(Android.App.Result.Ok, result);
            Finish();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            ActivityMediator.Instance.Send(intent.DataString);
        }

        private async Task<Intent> LoginAsync(object sender, EventArgs eventArgs)
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
            result.PutExtra("authToken", loginResult.AccessToken);
            result.PutExtra("refreshToken", loginResult.RefreshToken);
            return result;
        }
    }
}