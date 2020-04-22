﻿using Newtonsoft.Json;
using System;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class IngredientUsage
    {
        [JsonProperty]
        public Ingredient Ingredient
        {
            get;
            set;
        }

        [JsonProperty]
        public Unit Unit
        {
            get;
            set;
        }

        [JsonProperty]
        public String Amount
        {
            get;
            set;
        }

        public static IngredientUsage CreateEmpty()
        {
            IngredientUsage usage = new IngredientUsage();
            usage.Amount = "";
            usage.Unit = Unit.CreateEmpty();
            usage.Ingredient = Ingredient.CreateEmpty();
            return usage;
        }

        public IngredientUsage()
        {
        }
    }
}
