using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using MixologyJournalApp.Security;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid.Security
{
    internal class AuthenticationManager: IAuthenticate
    {
        // Define an authenticated user.
        private MobileServiceUser user;
        private Context _context;

        public AuthenticationManager(Context context)
        {
            _context = context;
        }

        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Google login using a server-managed flow.
                MobileServiceClient client = new MobileServiceClient("https://mixologyjournal.azurewebsites.net");
                user = await client.LoginAsync(_context, MobileServiceAuthenticationProvider.Google, "mixologyjournal");
                if (user != null)
                {
                    message = string.Format("you are now signed-in as {0}.", user.UserId);
                    success = true;
                }
                Dictionary<String, String> headers = new Dictionary<String, String>() { { "authorization", "bearer " + user.MobileServiceAuthenticationToken } };
                HttpResponseMessage response = await client.InvokeApiAsync("/insecure/recipes", new StringContent(""), HttpMethod.Get, headers, new Dictionary<String, String>());
                String responseStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseStr);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Console.WriteLine(message);
            /*
            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();
            */

            return success;
        }
    }
}