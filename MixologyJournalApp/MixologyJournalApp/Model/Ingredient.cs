using Newtonsoft.Json;
using System;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Ingredient
    {
        [JsonProperty("name")]
        public String Name
        {
            get;
            set;
        }

        [JsonProperty("plural")]
        public String Plural
        {
            get;
            set;
        }

        [JsonProperty("id")]
        public String Id
        {
            get;
            set;
        }

        public static Ingredient CreateEmpty()
        {
            Ingredient ingredient = new Ingredient
            {
                Name = "",
                Id = "",
                Plural = ""
            };
            return ingredient;
        }

        public Ingredient()
        {
        }

        public Ingredient Clone()
        {
            Ingredient clone = new Ingredient
            {
                Id = Id,
                Name = Name,
                Plural = Plural
            };
            return clone;
        }
    }
}
