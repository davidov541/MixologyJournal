using MixologyJournalApp.Model;
using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid.Platform
{
    internal class AndroidBackend: IBackend
    {
        private readonly String _basePath = AppConfigManager.Settings["BackendAddress"];

        private readonly AuthenticationManager _authentication;

        public AndroidBackend(AuthenticationManager authentication)
        {
            _authentication = authentication;
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
            if (_authentication.IsAuthenticated)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authentication.User.AuthToken);
            }
            request.Headers.Add("apiversion", "1");
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
            if (_authentication.IsAuthenticated)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authentication.User.AuthToken);
            }
            request.Headers.Add("apiversion", "1");
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