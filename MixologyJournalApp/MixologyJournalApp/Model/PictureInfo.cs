using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class PictureInfo
    {
        private const String DefaultIconPath = "creation-pics/default.png";

        [JsonProperty("path")]
        public String Path
        {
            get;
            set;
        }

        [JsonProperty("url")]
        public String Url
        {
            get;
            set;
        }

        public ImageSource Image
        {
            get
            {
                if (String.IsNullOrEmpty(Path) || Path.Equals("null") || Path.Equals(DefaultIconPath))
                {
                    return ImageSource.FromFile("@drawable/DefaultContentPic.png");
                }
                return new UriImageSource()
                {
                    CachingEnabled = true,
                    CacheValidity = TimeSpan.FromDays(1.0),
                    Uri = new Uri(Url)
                };
            }
        }

        public PictureInfo()
        {
            Path = DefaultIconPath;
        }

        public PictureInfo(String path, String url) : this()
        {
            Path = path;
            Url = url;
        }
    }
}
