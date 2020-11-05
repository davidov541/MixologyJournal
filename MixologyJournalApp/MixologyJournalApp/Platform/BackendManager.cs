using MixologyJournalApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MixologyJournalApp.Platform
{
    public class BackendManager
    {
        private IBackend _backend;
        private IAlertDialogFactory _alertFactory;

        private Boolean _canAccessRemote = true;

        public BackendManager(IBackend backend, IAlertDialogFactory alertFactory)
        {
            _backend = backend;
            _alertFactory = alertFactory;
        }

        internal async Task<List<Recipe>> UpdateRecipes()
        {
            if (_canAccessRemote)
            {
                try
                {
                    String jsonResult = await _backend.GetResult("/insecure/recipes");
                    List<Recipe> recipeModels = JsonConvert.DeserializeObject<List<Recipe>>(jsonResult);
                    return recipeModels;
                }
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                }
            }
            return new List<Recipe>();
        }

        internal async Task<List<Drink>> UpdateDrinks()
        {
            // If we are local only, then we won't get any drinks from the remote backend.
            if (_canAccessRemote)
            {
                try
                {
                    String jsonResult = await _backend.GetResult("/insecure/drinks");
                    List<Drink> drinkModels = JsonConvert.DeserializeObject<List<Drink>>(jsonResult);
                    return drinkModels;
                }
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                }
            }
            return new List<Drink>();
        }

        internal async Task<List<Ingredient>> UpdateAvailableIngredients()
        {
            if (_canAccessRemote)
            {
                try
                {
                    String jsonResult = await _backend.GetResult("/insecure/ingredients");
                    List<Ingredient> ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(jsonResult).ToList();
                    return ingredients;
                }
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                }
            }
            return new List<Ingredient>();
        }

        internal async Task<List<Unit>> UpdateAvailableUnits()
        {
            if (_canAccessRemote)
            {
                try
                {
                    String jsonResult = await _backend.GetResult("/insecure/units");
                    List<Unit> units = JsonConvert.DeserializeObject<List<Unit>>(jsonResult).ToList();
                    return units;
                }
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                }
            }
            return new List<Unit>();
        }

        internal async Task<List<Category>> UpdateCategories()
        {
            if (_canAccessRemote)
            {
                try
                {
                    String jsonResult = await _backend.GetResult("/insecure/categories");
                    List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(jsonResult).ToList();
                    return categories;
                }
                catch (HttpRequestException)
                {
                    WarnAboutRemoteAccessibility();
                }
            }
            return new List<Category>();
        }

        internal async Task<Boolean> UpdateFavoriteDrink(Drink drink, Boolean isFavorite)
        {
            Boolean finalResult = false;
            if (_canAccessRemote)
            {
                QueryResult result = await _backend.PostResult("/secure/favorite", new FavoriteRequest(drink.SourceRecipeID, drink.Id, isFavorite));
                if (result.Result)
                {
                    drink.IsFavoriteUploaded = true;
                }
                else
                {
                    WarnAboutRemoteAccessibility();
                }
                finalResult = result.Result;
            }

            drink.IsFavoriteUploaded = finalResult;
            return finalResult;
        }

        internal async Task<Boolean> UploadRecipe(Recipe model)
        {
            if (_canAccessRemote)
            {
                QueryResult result = await _backend.PostResult("/secure/recipes", model);
                if (result.Result)
                {
                    model.Id = result.Content["createdId"];
                    model.Uploaded = true;
                }
                else
                {
                    WarnAboutRemoteAccessibility();
                }
                return result.Result;
            }
            return false;
        }

        internal async Task<Boolean> UploadDrink(Drink model)
        {
            if (_canAccessRemote)
            {
                QueryResult result = await _backend.PostResult("/secure/drinks", model);
                if (result.Result)
                {
                    model.Uploaded = true;
                    model.Id = result.Content["createdId"];
                }
                else
                {
                    WarnAboutRemoteAccessibility();
                }

                return result.Result;
            }
            return false;
        }

        internal async Task<Boolean> DeleteRecipe(Recipe recipe)
        {
            Boolean finalResult = false;
            if (_canAccessRemote)
            {
                QueryResult result = await _backend.DeleteResult("/secure/recipes", recipe);
                finalResult = result.Result;

                if (!finalResult)
                {
                    WarnAboutRemoteAccessibility();
                }
            }

            return finalResult;
        }

        internal async Task<Boolean> DeleteDrink(Drink drink)
        {
            Boolean finalResult = false;
            if (_canAccessRemote)
            {
                QueryResult result = await _backend.DeleteResult("/secure/drinks", drink);
                finalResult = result.Result;

                if (!finalResult)
                {
                    WarnAboutRemoteAccessibility();
                }
            }

            return finalResult;
        }

        internal async Task<PictureInfo> UploadPicture(String path)
        {
            Byte[] content = File.ReadAllBytes(path);
            QueryResult result = await _backend.SendFile(content, "/secure/addpicture");
            if (result.Result)
            {
                String remotePath = result.Content["filePath"];
                String sasPath = result.Content["fileSAS"];
                return new PictureInfo(remotePath, sasPath);
            }
            return new PictureInfo();
        }

        private void WarnAboutRemoteAccessibility()
        {
            if (_canAccessRemote)
            {
                _alertFactory.ShowDialog("Server Down",
                                         "The backend server appears to be down. We will use the local cache, but are unable to retrieve any updated content from the servers at this time.\n" +
                                         "Additionally, any changes made will be kept locally until we are able to sync with the backend again.");
            }
            _canAccessRemote = false;
        }
    }
}
