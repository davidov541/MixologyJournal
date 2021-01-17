using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class PictureInfo
    {
        private const String DefaultIconPath = "creation-pics/default.png";

        private String _path;
        [JsonProperty("path")]
        public String Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _path = value;
                }
            }
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
                if (String.IsNullOrEmpty(Path) || Path.Equals("null"))
                {
                    return ImageSource.FromFile(Url);
                }
                else if (Path.Equals(DefaultIconPath))
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

        public static PictureInfo CreateRemotePicture(String path, String url)
        {
            PictureInfo info = new PictureInfo();
            info.Path = path;
            info.Url = url;
            return info;
        }

        public static PictureInfo CreateLocalPicture(String url)
        {
            PictureInfo info = new PictureInfo();
            info._path = null;
            info.Url = url;
            return info;
        }

        public PictureInfo()
        {
            Path = DefaultIconPath;
        }
    }
}
