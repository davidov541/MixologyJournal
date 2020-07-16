using Newtonsoft.Json;
using System;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class PictureInfo
    {
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

        public PictureInfo()
        {
        }

        public PictureInfo(String path, String url): this()
        {
            Path = path;
            Url = url;
        }
    }
}
