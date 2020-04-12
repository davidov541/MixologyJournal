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
        private Context _context;

        public Boolean IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public MobileServiceUser User 
        { 
            get; 
            private set; 
        }

        public MobileServiceClient Client
        {
            get;
            private set;
        }

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
                Client = new MobileServiceClient("https://mixologyjournal.azurewebsites.net");
                User = await Client.LoginAsync(_context, MobileServiceAuthenticationProvider.Google, "mixologyjournal");
                if (User != null)
                {
                    message = string.Format("you are now signed-in as {0}.", User.UserId);
                    success = true;
                }
                Dictionary<String, String> headers = new Dictionary<String, String>() { { "authorization", "bearer " + User.MobileServiceAuthenticationToken } };
                HttpResponseMessage response = await Client.InvokeApiAsync("/insecure/recipes", new StringContent(""), HttpMethod.Get, headers, new Dictionary<String, String>());
                String responseStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseStr);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Console.WriteLine(message);

            return success;
        }
    }
}