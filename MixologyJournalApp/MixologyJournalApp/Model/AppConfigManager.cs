using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MixologyJournalApp.Model
{
    public class AppConfigManager
    {
        private static AppConfigManager _instance;
        private readonly JObject _secrets;

        private const string Namespace = "MixologyJournalApp";
#if DEBUG
        private const string FileName = "appsettings.dev.json";
#else
        private const string FileName = "appsettings.prod.json";
#endif

        private AppConfigManager()
        {
            try
            {
                Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(AppConfigManager)).Assembly;
                Stream stream = assembly.GetManifestResourceStream($"{Namespace}.{FileName}");
                using (var reader = new StreamReader(stream))
                {
                    String json = reader.ReadToEnd();
                    _secrets = JObject.Parse(json);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Unable to load secrets file");
            }
        }

        public static AppConfigManager Settings
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppConfigManager();
                }

                return _instance;
            }
        }

        public string this[String name]
        {
            get
            {
                try
                {
                    String[] path = name.Split(':');

                    JToken node = _secrets[path[0]];
                    for (int index = 1; index < path.Length; index++)
                    {
                        node = node[path[index]];
                    }

                    return node.ToString();
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Unable to retrieve secret '{name}'");
                    return string.Empty;
                }
            }
        }
    }
}
