using Newtonsoft.Json;
using System;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class IngredientUsage
    {
        [JsonProperty("ingredientname")]
        public String Name
        {
            get;
            set;
        }

        [JsonProperty]
        public String Unit
        {
            get;
            set;
        }

        [JsonProperty]
        public String Amount
        {
            get;
            set;
        }

        [JsonProperty("ingredientid")]
        public String Id
        {
            get;
            set;
        }

        public IngredientUsage()
        {
        }
    }
}
