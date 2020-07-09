using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class FavoriteRequest
    {
        [JsonProperty("recipeId")]
        public String RecipeId
        {
            get;
            private set;
        }

        [JsonProperty("drinkId")]
        public String DrinkId
        {
            get;
            private set;
        }

        [JsonProperty("isFavorited")]
        public Boolean IsFavorited
        {
            get;
            private set;
        }

        public FavoriteRequest(String recipeId, String drinkId, Boolean isFavorited)
        {
            RecipeId = recipeId;
            DrinkId = drinkId;
            IsFavorited = isFavorited;
        }
    }
}
