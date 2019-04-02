using System;
using System.Collections.Generic;

namespace MixologyJournal.ViewModel.Recipe
{
    public interface IViewRecipeViewModel
    {
        String Instructions
        {
            get;
        }

        IEnumerable<IEditIngredientViewModel> Ingredients
        {
            get;
        }

        String Title
        {
            get;
        }
    }

    public interface IEditRecipeViewModel
    {
        void SetInstructions(String instructions);

        void SetTitle(String title);

        void AddIngredient();

        void RemoveIngredient(IEditIngredientViewModel ingred);

        void Save();

        void Cancel();
    }
}
