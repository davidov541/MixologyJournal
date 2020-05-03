using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using MixologyJournalApp.Droid.Security;
using MixologyJournalApp.Model;
using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid.Platform
{
    internal class BackendManager: IBackend
    {
        private const String _basePath = "https://mixologyjournalfunction.azurewebsites.net/api";

        public Boolean IsAuthenticated
        {
            get
            {
                return GetActiveLoginMethod() != null;
            }
        }

        private readonly List<ILoginMethod> _loginMethods = new List<ILoginMethod>()
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

        public User User 
        { 
            get
            {
                ILoginMethod method = GetActiveLoginMethod();
                return method?.CurrentUser;
            }
        }

        public BackendManager()
        {
            _loginMethods.ForEach(l => l.PropertyChanged += LoginMethod_PropertyChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoginMethod_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ILoginMethod.IsLoggedIn))
            {
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public async Task Init()
        {
            IEnumerable<Task> initializationTasks = _loginMethods.Select(l => l.Init());
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
            String token = AppConfigManager.Settings["AppSecret"];
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(fullPath),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("app-secret", token);
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
            String token = (User == null) ? string.Empty : User.AuthToken;
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
    }
}