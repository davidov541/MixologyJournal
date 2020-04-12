using Newtonsoft.Json;
using System;
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

        public static async Task<Recipe> GetRecipe(String id)
        {
            String result = await App.GetInstance().Backend.GetResult("");
            return GetRecipeFromJSON(result);
        }

        public static Recipe GetRecipeFromJSON(String jsonScript)
        {
            return JsonConvert.DeserializeObject<Recipe>(jsonScript);
        }

        public Recipe(String id)
        {
        }
    }
}
