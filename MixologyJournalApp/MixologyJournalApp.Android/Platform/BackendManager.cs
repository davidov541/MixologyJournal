using MixologyJournalApp.Model;
using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid.Platform
{
    internal class BackendManager: IBackend
    {
        private readonly String _basePath = AppConfigManager.Settings["BackendAddress"];

        public Boolean IsAuthenticated
        {
            get
            {
                return GetActiveLoginMethod() != null;
            }
        }

        private readonly List<ILoginMethod> _loginMethods = new List<ILoginMethod>();

        public IEnumerable<ILoginMethod> LoginMethods
        {
            get
            {
                return _loginMethods;
            }
        }

        public User User 
        { 
            get
            {
                ILoginMethod method = GetActiveLoginMethod();
                return method?.CurrentUser;
            }
        }

        public BackendManager(MainActivity mainActivity)
        {
            _loginMethods.Add(new Auth0LoginMethod(mainActivity));
            _loginMethods.ForEach(l => l.PropertyChanged += LoginMethod_PropertyChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoginMethod_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ILoginMethod.IsLoggedIn):
                    OnPropertyChanged(nameof(IsAuthenticated));
                    OnPropertyChanged(nameof(User));
                    break;
                default:
                    break;
            }
        }

        public async Task Init(bool setupMode)
        {
            IEnumerable<Task> initializationTasks = _loginMethods.Select(l => l.Init(setupMode));
            await Task.WhenAll(initializationTasks);
        }

        private ILoginMethod GetActiveLoginMethod()
        {
            return _loginMethods.FirstOrDefault(l => l.IsLoggedIn);
        }

        public async Task<String> GetResult(String path)
        {
            HttpClient client = new HttpClient();
            String fullPath = _basePath + "/" + path;
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(fullPath),
                Method = HttpMethod.Get,
            };
            if (IsAuthenticated)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", User.AuthToken);
            }
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                String message = "Request failed. See below for the error message.\n" + response.ReasonPhrase + "\n" + await response.Content.ReadAsStringAsync();
                Console.WriteLine(message);
                throw new HttpRequestException(message);
            }

            String result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<QueryResult> PostResult(string path, object body)
        {
            return await RunQuery(path, body, HttpMethod.Post);
        }

        public async Task<QueryResult> DeleteResult(string path, object body)
        {
            return await RunQuery(path, body, HttpMethod.Delete);
        }

        private async Task<QueryResult> RunQuery(string path, object body, HttpMethod method)
        {
            HttpClient client = new HttpClient();
            String fullPath = _basePath + "/" + path;
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(fullPath),
                Method = method,
                Content = new StringContent(JsonConvert.SerializeObject(body))
            };
            if (IsAuthenticated)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", User.AuthToken);
            }
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request failed. See below for the error message.");
                Console.WriteLine(response.ReasonPhrase);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }

            return await QueryResult.Create(response);
        }
    }
}