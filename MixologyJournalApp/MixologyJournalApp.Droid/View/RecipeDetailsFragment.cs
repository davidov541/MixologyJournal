using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using MixologyJournal.ViewModel.Recipe;
using AndroidView = Android.Views.View;
using Fragment = Android.Support.V4.App.Fragment;

namespace MixologyJournalApp.Droid.View
{
    public class RecipeDetailsFragment : Fragment
    {
        private IViewRecipeViewModel _recipe;
        internal RecipeDetailsFragment(IViewRecipeViewModel recipe) : base()
        {
            _recipe = recipe;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override AndroidView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            AndroidView view = inflater.Inflate(Resource.Layout.RecipePageFragment, container, false);

            TextView ingredientsText = view.FindViewById<TextView>(Resource.Id.ingredientsText);
            ingredientsText.Text = getIngredientsDescription(_recipe);

            TextView stepsText = view.FindViewById<TextView>(Resource.Id.stepsText);
            stepsText.Text = _recipe.Instructions;
            return view;
        }

        private String getIngredientsDescription(IViewRecipeViewModel recipe)
        {
            String result = String.Empty;
            foreach (IEditIngredientViewModel ingredient in recipe.Ingredients)
            {
                result += ingredient.Description + "\n";
            }
            if (result.Length > 2)
            {
                result = result.Remove(result.Length - 1, 1);
            }
            return result;
        }
    }
}
