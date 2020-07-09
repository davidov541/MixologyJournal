using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Recipe
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

        public Drink FavoriteDrink
        {
            get;
            set;
        }

        public static Recipe CreateEmptyRecipe()
        {
            Recipe recipe = new Recipe();

            recipe.Steps.Add("");
            recipe.Ingredients.Add(IngredientUsage.CreateEmpty());

            return recipe;
        }

        public Recipe()
        {
            Steps = new List<String>();
            Ingredients = new List<IngredientUsage>();
        }
    }
}
