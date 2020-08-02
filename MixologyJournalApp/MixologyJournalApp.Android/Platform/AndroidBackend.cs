using MixologyJournalApp.Model;
using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MixologyJournalApp.Droid.Platform
{
    internal class AndroidBackend : IBackend
    {
        private readonly String _basePath = AppConfigManager.Settings["BackendAddress"];

        private readonly AuthenticationManager _authentication;

        public AndroidBackend(AuthenticationManager authentication)
        {
            _authentication = authentication;
        }

        public async Task<String> GetResult(String path)
        {
            using (HttpClient client = new HttpClient())
            {
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

                return await response.Content.ReadAsStringAsync();
            }
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
            using (HttpClient client = new HttpClient())
            {
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

        public async Task<QueryResult> SendFile(Byte[] fileContents, String remotePath)
        {
            using (HttpClient client = new HttpClient())
            {
                String fullPath = _basePath + "/" + remotePath;
                using (MultipartFormDataContent formData = new MultipartFormDataContent("--------------------------848882407475721692347387"))
                {
                    if (_authentication.IsAuthenticated)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authentication.User.AuthToken);
                    }
                    client.DefaultRequestHeaders.Add("apiversion", "1");
                    formData.Add(new ByteArrayContent(fileContents), "file", "test.txt");
                    String resultForm = await formData.ReadAsStringAsync();
                    HttpResponseMessage response = await client.PostAsync(fullPath, formData);
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
    }
}