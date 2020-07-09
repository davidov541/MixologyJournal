using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

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

        [JsonProperty("basisRecipe")]
        public String SourceRecipeID
        {
            get;
            set;
        }

        private Recipe _sourceRecipe;
        public Recipe SourceRecipe
        {
            get
            {
                return _sourceRecipe;
            }
            private set
            {
                _sourceRecipe = value;
                if (IsFavorite)
                {
                    SourceRecipe.FavoriteDrink = this;
                }
            }
        }

        [JsonProperty("rating")]
        public float Rating
        {
            get;
            set;
        }

        [JsonProperty("review")]
        public String Review
        {
            get;
            set;
        }

        private Boolean _isFavorite = false;
        [JsonProperty("isFavorite")]
        public Boolean IsFavorite
        {
            get
            {
                return _isFavorite;
            }
            set
            {
                _isFavorite = value;
                if (SourceRecipe != null)
                {
                    SourceRecipe.FavoriteDrink = this;
                }
            }
        }

        public static Drink CreateEmptyDrink(Recipe basis)
        {
            Drink drink = new Drink
            {
                Name = basis.Name,
                SourceRecipe = basis,
                SourceRecipeID = basis.Id
            };
            drink.Steps.AddRange(basis.Steps);
            drink.Ingredients.AddRange(basis.Ingredients.Select(i => i.Clone()));

            return drink;
        }

        public Drink()
        {
            Steps = new List<String>();
            Ingredients = new List<IngredientUsage>();
            Rating = 0.0f;
            Review = "";
        }

        public void Init(Recipe basisRecipe)
        {
            SourceRecipe = basisRecipe;
        }
    }
}
