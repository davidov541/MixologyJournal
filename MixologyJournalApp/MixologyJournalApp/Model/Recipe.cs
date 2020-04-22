using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Recipe
    {
        [JsonProperty]
        public String Name
        {
            get;
            set;
        }

        [JsonProperty]
        public List<String> Steps
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

        [JsonProperty]
        public List<IngredientUsage> Ingredients
        {
            get;
            set;
        }

        public static async Task<Recipe> GetRecipe(String id)
        {
            String result = await App.GetInstance().PlatformInfo.Backend.GetResult("");
            return GetRecipeFromJSON(result);
        }

        public static Recipe GetRecipeFromJSON(String jsonScript)
        {
            return JsonConvert.DeserializeObject<Recipe>(jsonScript);
        }

        public static Recipe CreateEmptyRecipe()
        {
            Recipe recipe = new Recipe();
            recipe.Steps.Add("");
            return recipe;
        }

        public Recipe()
        {
            Steps = new List<String>();
            Ingredients = new List<IngredientUsage>();
        }

        public Recipe(String id)
        {
            Id = id;
        }
    }
}
