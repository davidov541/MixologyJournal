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
                Id = ""
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
                Name = Name
            };
            return clone;
        }
    }
}
