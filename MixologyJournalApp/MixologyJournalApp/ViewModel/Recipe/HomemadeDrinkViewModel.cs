using System;
using System.Collections.Generic;
using System.Linq;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Recipe
{
    internal class HomemadeDrinkViewModel : RecipeViewModel<HomemadeDrink>
    {
        public override String Caption
        {
            get
            {
                return GetDifferenceDescription();
            }
        }

        public BaseRecipeViewModel BaseRecipe
        {
            get;
            private set;
        }

        public HomemadeDrinkViewModel(HomemadeDrink model, BaseRecipeViewModel baseRecipe, BaseMixologyApp app)
            : base(model, app)
        {
            BaseRecipe = baseRecipe;
        }

        public override void Save()
        {
            base.Save();
            BaseRecipe.Save();
        }

        protected override void Reset()
        {
            base.Reset();
            if (BaseRecipe != null)
            {
                BaseRecipe.Cancel();
            }
        }

        private String GetDifferenceDescription()
        {
            List<IngredientDiff> ingredientDiffs = new List<IngredientDiff>();
            foreach (EditIngredientViewModel modIngred in Ingredients)
            {
                IEditIngredientViewModel baseIngred = BaseRecipe.Ingredients.FirstOrDefault(i => i.Name.Equals(modIngred.Name));
                ingredientDiffs.Add(new IngredientDiff(baseIngred as EditIngredientViewModel, modIngred));
            }
            foreach (EditIngredientViewModel baseIngred in BaseRecipe.Ingredients)
            {
                if (!Ingredients.Any(i => i.Name.Equals(baseIngred.Name)))
                {
                    ingredientDiffs.Add(new IngredientDiff(baseIngred, null));
                }
            }

            String difference = String.Empty;
            List<IngredientDiff> addedAndRemovedIngreds = ingredientDiffs.Where(i => i.Status == ChangedStatus.Added || i.Status == ChangedStatus.Removed).ToList();
            if (addedAndRemovedIngreds.Any())
            {
                for (int i = 0; i < addedAndRemovedIngreds.Count; i++)
                {
                    if (addedAndRemovedIngreds[i].Status == ChangedStatus.Added)
                    {
                        difference += "Added " + addedAndRemovedIngreds[i].Name;
                    }
                    else
                    {
                        difference += "Removed " + addedAndRemovedIngreds[i].Name;
                    }
                    difference += GetListSuffix(i, addedAndRemovedIngreds.Count);
                }
                return difference;
            }
            List<IngredientDiff> changedIngreds = ingredientDiffs.Where(i => i.Status == ChangedStatus.Modified).ToList();
            if (changedIngreds.Any())
            {
                for (int i = 0; i < changedIngreds.Count; i++)
                {
                    difference += changedIngreds[i].Details;
                    if (changedIngreds[i].Amount.Status == AmountDiffStatus.Increased)
                    {
                        difference += "More " + changedIngreds[i].Name;
                    }
                    else
                    {
                        difference += "Less " + changedIngreds[i].Name;
                    }
                    difference += GetListSuffix(i, changedIngreds.Count);

                }
                return difference;
            }

            List<EditIngredientViewModel> detailedIngreds = Ingredients.OfType<EditIngredientViewModel>().Where(i => !String.IsNullOrEmpty(i.Details)).ToList();
            if (detailedIngreds.Any())
            {
                difference = "With ";
                for (int i = 0; i < detailedIngreds.Count; i++)
                {
                    difference += detailedIngreds[i].Details;
                    difference += GetListSuffix(i, detailedIngreds.Count);
                }
                return difference;
            }
            return "No Changes";
        }

        private String GetListSuffix(int currIndex, int length)
        {
            String difference = String.Empty;
            if (currIndex + 2 == length)
            {
                if (length > 2)
                {
                    difference += ",";
                }
                difference += " & ";
            }
            else if (currIndex + 2 < length)
            {
                difference += ", ";
            }
            return difference;
        }

        public override Boolean Equals(Object obj)
        {
            HomemadeDrinkViewModel o = obj as HomemadeDrinkViewModel;
            if (o == null)
            {
                return false;
            }
            return o.Model.ID == Model.ID;
        }

        public override Int32 GetHashCode()
        {
            return Model.ID.GetHashCode();
        }
    }
}
