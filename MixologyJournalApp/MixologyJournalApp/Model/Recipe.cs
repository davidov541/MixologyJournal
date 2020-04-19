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
        public List<Ingredient> Ingredients
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

        public Recipe()
        {
            Steps = new List<String>();
            Steps.Add("");
            Ingredients = new List<Ingredient>();
        }

        public Recipe(String id)
        {
            Id = id;
        }
    }
}
