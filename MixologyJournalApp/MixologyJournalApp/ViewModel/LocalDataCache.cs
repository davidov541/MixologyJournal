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

        public async Task Init()
        {
            await UpdateRecipes();
        }

        public async Task Resync()
        {
            await UpdateRecipes();
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

        private void Recipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
