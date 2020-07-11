﻿using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ModelCache
    {
        private List<Recipe> _recipes = new List<Recipe>();
        [JsonProperty("recipes")]
        public IEnumerable<Recipe> Recipes
        {
            get
            {
                return _recipes;
            }
            set
            {
                _recipes = new List<Recipe>(value);
            }
        }

        private List<Drink> _drinks = new List<Drink>();
        [JsonProperty("drinks")]
        public IEnumerable<Drink> Drinks
        {
            get
            {
                return _drinks;
            }
            set
            {
                _drinks = new List<Drink>(value);
            }
        }

        private List<Ingredient> _ingredients = new List<Ingredient>();
        [JsonProperty("ingredients")]
        public IEnumerable<Ingredient> AvailableIngredients
        {
            get
            {
                return _ingredients;
            }
            set
            {
                _ingredients = new List<Ingredient>(value);
            }
        }

        private List<Unit> _units = new List<Unit>();
        [JsonProperty("units")]
        public IEnumerable<Unit> AvailableUnits
        {
            get
            {
                return _units;
            }
            set
            {
                _units = new List<Unit>(value);
            }
        }

        public double InitProgress { get; private set; } = 0.0;

        private IBackend _backend;
        public ModelCache(IBackend backend)
        {
            _backend = backend;
        }

        public async Task Init()
        {
            InitProgress = 0.0;
            await UpdateAvailableUnits();

            InitProgress = 0.25;
            await UpdateAvailableIngredients();

            InitProgress = 0.5;
            List<Recipe> recipeModels = await UpdateRecipes();

            InitProgress = 0.75;
            await UpdateDrinks(recipeModels);

            InitProgress = 1.0;
        }

        public async Task Resync()
        {
            await Init();
        }

        private async Task<List<Recipe>> UpdateRecipes()
        {
            String jsonResult = await _backend.GetResult("/insecure/recipes");
            List<Recipe> recipeModels = JsonConvert.DeserializeObject<List<Recipe>>(jsonResult);
            _recipes.Clear();
            foreach (Recipe r in recipeModels.OrderBy(i => i.Name))
            {
                _recipes.Add(r);
            }
            return recipeModels;
        }

        private async Task UpdateDrinks(List<Recipe> recipeModels)
        {
            String jsonResult = await _backend.GetResult("/insecure/drinks");
            List<Drink> drinkModels = JsonConvert.DeserializeObject<List<Drink>>(jsonResult);
            drinkModels.ForEach(d => d.Init(recipeModels.FirstOrDefault(r => r.Id.Equals(d.SourceRecipeID))));
            _drinks.Clear();
            foreach (Drink d in drinkModels.OrderBy(i => i.Name))
            {
                _drinks.Add(d);
            }
        }

        private async Task UpdateAvailableIngredients()
        {
            String jsonResult = await _backend.GetResult("/insecure/ingredients");
            List<Ingredient> ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(jsonResult).ToList();
            _ingredients.Clear();
            foreach (Ingredient i in ingredients.OrderBy(i => i.Name))
            {
                _ingredients.Add(i);
            }
        }

        private async Task UpdateAvailableUnits()
        {
            String jsonResult = await _backend.GetResult("/insecure/units");
            List<Unit> units = JsonConvert.DeserializeObject<List<Unit>>(jsonResult).ToList();
            _units.Clear();
            foreach (Unit u in units.OrderBy(i => i.Name))
            {
                _units.Add(u);
            }
        }

        public async Task<Boolean> CreateRecipe(Recipe model)
        {
            QueryResult result = await _backend.PostResult("/secure/recipes", model);

            if (result.Result)
            {
                model.Id = result.Content["createdId"];

                Recipe insertBeforeRecipe = Recipes.FirstOrDefault(r => model.Name.CompareTo(r.Name) < 0);
                if (insertBeforeRecipe == null)
                {
                    _recipes.Add(model);
                }
                else
                {
                    int insertIndex = _recipes.IndexOf(insertBeforeRecipe);
                    _recipes.Insert(insertIndex, model);
                }
            }
            return result.Result;
        }

        public async Task<Boolean> DeleteRecipe(Recipe recipe)
        {
            QueryResult result = await _backend.DeleteResult("/secure/recipes", recipe);
            if (result.Result)
            {
                _recipes.Remove(recipe);
            }
            return result.Result;
        }

        public async Task<Boolean> CreateDrink(Drink model)
        {
            QueryResult result = await _backend.PostResult("/secure/drinks", model);

            if (result.Result)
            {
                model.Id = result.Content["createdId"];

                Drink insertBeforeRecipe = Drinks.FirstOrDefault(d => model.Name.CompareTo(d.Name) < 0);
                if (insertBeforeRecipe == null)
                {
                    _drinks.Add(model);
                }
                else
                {
                    int insertIndex = _drinks.IndexOf(insertBeforeRecipe);
                    _drinks.Insert(insertIndex, model);
                }
            }
            return result.Result;
        }

        public async Task<Boolean> DeleteDrink(Drink drink)
        {
            QueryResult result = await _backend.DeleteResult("/secure/drinks", drink);
            if (result.Result)
            {
                _drinks.Remove(drink);
            }
            return result.Result;
        }

        public async Task UpdateFavoriteDrink(Drink drink, Boolean isFavorite)
        {
            await _backend.PostResult("/secure/favorite", new FavoriteRequest(drink.SourceRecipeID, drink.Id, isFavorite));
        }
    }
}
