using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Category
    {
        [JsonProperty("id")]
        public String Id
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public String Name
        {
            get;
            set;
        }

        [JsonProperty("subcategories")]
        public List<Category> Subcategories
        {
            get;
            set;
        }

        [JsonProperty("ingredients")]
        public List<String> IngredientIds
        {
            get;
            set;
        }

        private List<Ingredient> _ingredients = new List<Ingredient>();
        internal IEnumerable<Ingredient> Ingredients
        {
            get
            {
                return _ingredients;
            }
        }

        public Category()
        {
            IngredientIds = new List<string>();
            Subcategories = new List<Category>();
        }

        internal void Init(IDictionary<String, Ingredient> ingredientLookup)
        {
            _ingredients.AddRange(IngredientIds.Where(ingredientLookup.ContainsKey).Select(i => ingredientLookup[i]));
            foreach (Category sub in Subcategories)
            {
                sub.Init(ingredientLookup);
            }
        }
    }
}
