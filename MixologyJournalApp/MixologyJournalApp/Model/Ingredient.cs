using Newtonsoft.Json;
using System;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Ingredient
    {
        [JsonProperty]
        public String Name
        {
            get;
            set;
        }

        [JsonProperty]
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
    }
}
