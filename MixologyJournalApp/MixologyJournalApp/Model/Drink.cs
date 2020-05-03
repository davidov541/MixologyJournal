using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Drink
    {
        [JsonProperty("name")]
        public String Name
        {
            get;
            set;
        }

        [JsonProperty("steps")]
        public List<String> Steps
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

        [JsonProperty("ingredients")]
        public List<IngredientUsage> Ingredients
        {
            get;
            set;
        }

        [JsonProperty("sourceRecipeID")]
        public String SourceRecipeID
        {
            get;
            set;
        }

        public Recipe SourceRecipe
        {
            get;
            private set;
        }

        public static Drink CreateEmptyDrink()
        {
            Drink drink = new Drink();

            drink.Steps.Add("");
            drink.Ingredients.Add(IngredientUsage.CreateEmpty());

            return drink;
        }

        public Drink()
        {
            Steps = new List<String>();
            Ingredients = new List<IngredientUsage>();
        }
    }
}
