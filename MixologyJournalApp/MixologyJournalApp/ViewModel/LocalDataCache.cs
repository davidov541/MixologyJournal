using MixologyJournalApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MixologyJournalApp.ViewModel
{
    internal class LocalDataCache
    {
        public ObservableCollection<RecipeViewModel> Recipes { get; } = new ObservableCollection<RecipeViewModel>();

        public ObservableCollection<IngredientViewModel> AvailableIngredients { get; } = new ObservableCollection<IngredientViewModel>();

        public async Task Init()
        {
            await UpdateRecipes();
            await UpdateAvailableIngredients();
        }

        public async Task Resync()
        {
            await UpdateRecipes();
            await UpdateAvailableIngredients();
        }

        private async Task UpdateRecipes()
        {
            String jsonResult = await App.GetInstance().PlatformInfo.Backend.GetResult("/insecure/recipes");
            List<RecipeViewModel> recipes = JsonConvert.DeserializeObject<List<Recipe>>(jsonResult).Select(r => new RecipeViewModel(r)).ToList();
            Recipes.Clear();
            foreach (RecipeViewModel r in recipes)
            {
                r.PropertyChanged += Recipe_PropertyChanged;
                Recipes.Add(r);
            }
        }

        private async Task UpdateAvailableIngredients()
        {
            String jsonResult = await App.GetInstance().PlatformInfo.Backend.GetResult("/insecure/ingredients");
            List<IngredientViewModel> ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(jsonResult).Select(i => new IngredientViewModel(i)).ToList();
            AvailableIngredients.Clear();
            foreach (IngredientViewModel i in ingredients)
            {
                AvailableIngredients.Add(i);
            }
        }

        private void Recipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
