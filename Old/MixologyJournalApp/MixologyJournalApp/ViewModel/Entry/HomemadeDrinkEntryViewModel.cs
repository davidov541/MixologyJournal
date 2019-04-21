using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournal.ViewModel.Entry
{
    public interface IHomemadeDrinkEntryViewModel : IRecipePageViewModel
    {
        bool IsFavorite
        {
            get;
            set;
        }
    }

    internal class HomemadeDrinkEntryViewModel : DrinkEntryViewModel, IHomemadeDrinkEntryViewModel
    {
        private BaseRecipePageViewModel _basePage;
        private BaseMixologyApp _app;

        private HomemadeDrinkEntry Entry
        {
            get
            {
                return Model as HomemadeDrinkEntry;
            }
        }

        public int ServingsNumber
        {
            get
            {
                if (_recipe == null)
                {
                    return 1;
                }
                return _recipe.ServingsNumber;
            }
            set
            {
                _recipe.ServingsNumber = value;
                OnPropertyChanged(nameof(ServingsNumber));
            }
        }

        private HomemadeDrinkViewModel _recipe;
        public HomemadeDrinkViewModel Recipe
        {
            get
            {
                return _recipe;
            }
            private set
            {
                if (_recipe != null)
                {
                    _recipe.BaseRecipe.PropertyChanged -= BaseRecipe_PropertyChanged;
                }
                _recipe = value;
                OnPropertyChanged(nameof(Recipe));
                OnPropertyChanged(nameof(IsFavorite));
                if (_recipe != null)
                {
                    _recipe.BaseRecipe.PropertyChanged += BaseRecipe_PropertyChanged;
                }
            }
        }

        public override String Caption
        {
            get
            {
                if (Recipe == null)
                {
                    return String.Empty;
                }
                return Recipe.Caption;
            }
        }

        public override Symbol? Icon
        {
            get
            {
                if (IsFavorite)
                {
                    return Symbol.SolidStar;
                }
                return null;
            }
        }

        public bool IsFavorite
        {
            get
            {
                return Recipe != null && Recipe.BaseRecipe.Favorite != null && Recipe.BaseRecipe.Favorite.Equals(Recipe);
            }
            set
            {
                Recipe.BaseRecipe.SetNewFavorite(Recipe, value);
            }
        }

        public IEnumerable<HomemadeDrinkEntryViewModel> SiblingRecipes
        {
            get
            {
                if (_basePage == null)
                {
                    return new List<HomemadeDrinkEntryViewModel>();
                }
                return _basePage.DerivedRecipes.Where(r => !r.Recipe.Equals(Recipe));
            }
        }

        public HomemadeDrinkEntryViewModel(IBaseRecipePageViewModel baseRecipe, BaseMixologyApp app) :
            base(app)
        {
            Recipe = ((baseRecipe as BaseRecipePageViewModel).Recipe as BaseRecipeViewModel).Clone();
            AttachToDrink(Recipe);
            AttachToModel(new HomemadeDrinkEntry(Recipe.Model));
            _basePage = baseRecipe as BaseRecipePageViewModel;
            _app = app;
        }

        public HomemadeDrinkEntryViewModel(HomemadeDrinkEntry entry, BaseRecipePageViewModel baseRecipe, BaseMixologyApp app) :
            base(app)
        {
            Recipe = new HomemadeDrinkViewModel(entry.Recipe, baseRecipe.Recipe as BaseRecipeViewModel, app);
            AttachToDrink(Recipe);
            AttachToModel(entry);
            _basePage = baseRecipe;
            _app = app;
        }

        private void BaseRecipe_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("FavoriteRecipe"))
            {
                OnPropertyChanged(nameof(IsFavorite));
            }
        }
    }
}
