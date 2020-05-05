using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Drink CreateEmptyDrink(Recipe basis)
        {
            Drink drink = new Drink();

            drink.Name = basis.Name;
            drink.Steps.AddRange(basis.Steps);
            drink.Ingredients.AddRange(basis.Ingredients.Select(i => i.Clone()));
            drink.SourceRecipe = basis;
            drink.SourceRecipeID = basis.Id;

            return drink;
        }

        public Drink()
        {
            Steps = new List<String>();
            Ingredients = new List<IngredientUsage>();
        }
    }
}
