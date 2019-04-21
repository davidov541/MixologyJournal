using System.Linq;
using Android.Support.V4.App;
using Java.Lang;
using MixologyJournal.ViewModel.Entry;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournalApp.Droid.View
{
    public class RecipePageAdapter : FragmentPagerAdapter
    {
        private IBaseRecipePageViewModel _recipe;
        private bool _hasFavorite;

        internal RecipePageAdapter(FragmentManager fm, IBaseRecipePageViewModel recipe) : base(fm)
        {
            _recipe = recipe;
            _hasFavorite = _recipe.Recipe.Favorite != null;
        }

        public override int Count {
            get
            {
                int num = 1;
                if (_hasFavorite)
                {
                    num++;
                }
                num += _recipe.Recipe.DerivedRecipes.Count();
                return num;
            }
        }

        public override Fragment GetItem(int position)
        {
            IViewRecipeViewModel recipe;
            switch(position)
            {
                case 0:
                    recipe = _recipe.Recipe;
                    break;
                case 1:
                    if (_hasFavorite)
                    {
                        recipe = _recipe.Recipe.Favorite;
                    }
                    else
                    {
                        recipe = _recipe.Recipe.DerivedRecipes.First();
                    }
                    break;
                default:
                    int offset = 1;
                    if (_hasFavorite) offset++;
                    recipe = _recipe.Recipe.DerivedRecipes.ElementAt(position - offset);
                    break;
            }
            return new RecipeDetailsFragment(recipe);
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            switch(position)
            {
                case 0:
                    return new String("Original");
                case 1:
                    if (_hasFavorite)
                    {
                        return new String("Favorite");
                    }
                    return new String(_recipe.Recipe.DerivedRecipes.First().Title);
                default:
                    int offset = 1;
                    if (_hasFavorite) offset++;
                    return new String(_recipe.Recipe.DerivedRecipes.ElementAt(position - offset).Title);
            }
        }
    }
}
