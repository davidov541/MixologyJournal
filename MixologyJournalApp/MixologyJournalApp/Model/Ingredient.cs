using Newtonsoft.Json;
using System;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Ingredient
    {
        [JsonProperty("ingredient")]
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

        public String Id
        {
            get;
            private set;
        }

        public Ingredient(String id)
        {
            Id = id;
        }
    }
}
