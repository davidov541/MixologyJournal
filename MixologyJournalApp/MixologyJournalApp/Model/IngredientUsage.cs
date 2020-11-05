using Newtonsoft.Json;
using System;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class IngredientUsage
    {
        [JsonProperty("ingredient")]
        public Ingredient Ingredient
        {
            get;
            set;
        }

        [JsonProperty("unit")]
        public Unit Unit
        {
            get;
            set;
        }

        [JsonProperty("brand")]
        public String Brand
        {
            get;
            set;
        }

        [JsonProperty("amount")]
        public String Amount
        {
            get;
            set;
        }

        public static IngredientUsage CreateEmpty()
        {
            IngredientUsage usage = new IngredientUsage
            {
                Amount = "",
                Unit = Unit.CreateEmpty(),
                Ingredient = Ingredient.CreateEmpty(),
                Brand = null
            };
            return usage;
        }

        public IngredientUsage()
        {
        }

        public IngredientUsage Clone()
        {
            IngredientUsage clone = new IngredientUsage
            {
                Amount = Amount,
                Ingredient = Ingredient.Clone(),
                Unit = Unit.Clone(),
                Brand = Brand
            };
            return clone;
        }
    }
}
