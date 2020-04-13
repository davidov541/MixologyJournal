using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using MixologyJournalApp.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid.Model
{
    internal class BackendManager: IBackend
    {
        private Context _context;
        private MobileServiceClient _client;

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

        public BackendManager(Context context)
        {
            _context = context;
            _client = new MobileServiceClient("https://mixologyjournal.azurewebsites.net");
        }

        public async Task<String> GetResult(String path)
        {
            Dictionary<String, String> headers = new Dictionary<String, String>() { { "authorization", "bearer " + User.MobileServiceAuthenticationToken } };
            HttpResponseMessage response = await _client.InvokeApiAsync(path, new StringContent(""), HttpMethod.Get, headers, new Dictionary<String, String>());
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Google login using a server-managed flow.
                User = await _client.LoginAsync(_context, MobileServiceAuthenticationProvider.Google, "mixologyjournal");
                if (User != null)
                {
                    message = string.Format("you are now signed-in as {0}.", User.UserId);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Console.WriteLine(message);

            return success;
        }

        public async Task LogOffAsync()
        {
            await _client.LogoutAsync();
            User = null;
        }
    }
}