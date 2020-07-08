using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MixologyJournalApp.Platform
{
    public struct QueryResult
    {
        public bool Result;
        public Dictionary<String, String> Content;

        private static Dictionary<String, String> ConvertJPropertiesToDictionary(IEnumerable<JProperty> properties)
        {
            return properties.Select(prop => new Tuple<String, String>(prop.Name, prop.Value.ToString())).ToDictionary(t => t.Item1, t => t.Item2);
        }

        public static async Task<QueryResult> Create(HttpResponseMessage response)
        {
            String content = await response.Content.ReadAsStringAsync();
            Dictionary<String, String> parsedContent = new Dictionary<String, String>();
            try
            {
                parsedContent = ConvertJPropertiesToDictionary(JObject.Parse(content).Properties());
            } catch (Exception)
            {
                // Unable to parse the message. May not be in JSON format. Just return an empty dictionary.
            }
            return new QueryResult()
            {
                Result = response.IsSuccessStatusCode,
                Content = parsedContent
            };
        }
    }
}
