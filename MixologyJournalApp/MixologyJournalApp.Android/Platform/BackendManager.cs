using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using MixologyJournalApp.Droid.Security;
using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid.Platform
{
    internal class BackendManager: IBackend
    {
        private readonly Context _context;
        private readonly MobileServiceClient _client;
        private readonly SecureStorageAccountStore _accountStore;

        private const String _basePath = "https://mixologyjournalfunction.azurewebsites.net/api";

        public Boolean IsAuthenticated
        {
            get
            {
                return GetActiveLoginMethod() != null;
            }
        }

        public Boolean HasBeenSetup
        {
            get
            {
                return false;
            }
        }

        private List<ILoginMethod> _loginMethods = new List<ILoginMethod>()
        {
            new GoogleLoginMethod()
        };
        public IEnumerable<ILoginMethod> LoginMethods
        {
            get
            {
                return _loginMethods;
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
            _client = new MobileServiceClient(_basePath);
            _accountStore = new SecureStorageAccountStore();
        }

        public async Task Init()
        {
            List<MobileServiceUser> users = await _accountStore.FindAccountsForServiceAsync(SecureStorageAccountStore.GoogleServiceId);
            if (users.Any())
            {
                User = users.First();
            }
        }

        private ILoginMethod GetActiveLoginMethod()
        {
            return _loginMethods.FirstOrDefault(l => l.IsLoggedIn);
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
                    await _accountStore.SaveAsync(User, SecureStorageAccountStore.GoogleServiceId);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Console.WriteLine(message);

            return success;
        }

        public async Task<String> GetResult(String path)
        {
            HttpClient client = new HttpClient();
            String fullPath = _basePath + "/" + path;
            String token = (User == null) ? string.Empty : User.MobileServiceAuthenticationToken;
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(fullPath),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("X-ZUMO-AUTH", token);
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request failed. See below for the error message.");
                Console.WriteLine(response.ReasonPhrase);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> PostResult(string path, object body)
        {
            HttpClient client = new HttpClient();
            String fullPath = _basePath + "/" + path;
            String token = (User == null) ? string.Empty : User.MobileServiceAuthenticationToken;
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(fullPath),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(body))
            };
            request.Headers.Add("X-ZUMO-AUTH", token);
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request failed. See below for the error message.");
                Console.WriteLine(response.ReasonPhrase);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }

            return response.IsSuccessStatusCode;
        }

        public async Task LogOffAsync()
        {
            await _client.LogoutAsync();
            User = null;
            _accountStore.Delete(SecureStorageAccountStore.GoogleServiceId);
        }
    }
}