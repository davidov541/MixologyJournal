using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;

namespace MixologyJournal.ViewModel.Recipe
{
    public interface IBaseViewRecipeViewModel : IViewRecipeViewModel
    {
        Task<IHomemadeDrinkEntryViewModel> CreateModifiedRecipeEntryAsync(IBaseRecipePageViewModel baseRecipe);

        IEnumerable<IViewRecipeViewModel> DerivedRecipes
        {
            get;
        }

        IViewRecipeViewModel Favorite
        {
            get;
        }
    }

    public interface IBaseEditRecipeViewModel : IBaseViewRecipeViewModel, IEditRecipeViewModel
    {
    }

    internal class BaseRecipeViewModel : RecipeViewModel<BaseRecipe>, IBaseEditRecipeViewModel
    {
        private List<HomemadeDrinkViewModel> _modifiedRecipes;
        private BaseMixologyApp _app;

        public override String Caption
        {
            get
            {
                return String.Empty;
            }
        }

        public IEnumerable<IViewRecipeViewModel> DerivedRecipes
        {
            get
            {
                return _modifiedRecipes;
            }
        }

        private HomemadeDrink _favoriteRecipe;
        public IViewRecipeViewModel Favorite
        {
            get
            {
                if (_favoriteRecipe == null)
                {
                    return null;
                }
                else
                {
                    return new HomemadeDrinkViewModel(_favoriteRecipe, this, App);
                }
            }
        }

        public BaseRecipeViewModel(BaseMixologyApp app) :
            this(new BaseRecipe(), app)
        {
            _app = app;
        }

        public BaseRecipeViewModel(BaseRecipe model, BaseMixologyApp app)
            : base(model, app)
        {
            _app = app;
            Reset();
        }

        public HomemadeDrinkViewModel Clone()
        {
            return new HomemadeDrinkViewModel(Model.Clone(), this, App);
        }

        public override void Save()
        {
            base.Save();

            Model.FavoriteRecipe = _favoriteRecipe;
            foreach (HomemadeDrinkViewModel modified in _modifiedRecipes)
            {
                if (!Model.DerivedRecipes.Contains(modified.Model))
                {
                    Model.AddModifiedRecipe(modified.Model);
                }
            }
        }

        protected override void Reset()
        {
            base.Reset();

            _favoriteRecipe = Model.FavoriteRecipe;
            _modifiedRecipes = new List<HomemadeDrinkViewModel>();

            foreach (HomemadeDrink modified in Model.DerivedRecipes)
            {
                _modifiedRecipes.Add(new HomemadeDrinkViewModel(modified, this, App));
            }
        }

        public async Task<IHomemadeDrinkEntryViewModel> CreateModifiedRecipeEntryAsync(IBaseRecipePageViewModel baseRecipe)
        {
            HomemadeDrinkEntryViewModel entry = new HomemadeDrinkEntryViewModel(baseRecipe, _app);
            await (_app.Journal as JournalViewModel).AddEntryAsync(entry);
            (baseRecipe as BaseRecipePageViewModel).AddModifiedRecipe(entry);
            return entry;
        }

        internal void SetNewFavorite(HomemadeDrinkViewModel drink, bool isFavorite) 
        {
            if (isFavorite && !drink.Equals(_favoriteRecipe))
            {
                _favoriteRecipe = drink.Model;
                OnPropertyChanged(nameof(Favorite));
            }
            else if (!isFavorite && drink.Equals(_favoriteRecipe))
            {
                _favoriteRecipe = null;
                OnPropertyChanged(nameof(Favorite));
            }
        }

        public void AddModifiedRecipe(HomemadeDrinkViewModel recipe)
        {
            _modifiedRecipes.Add(recipe);
        }

        public override Boolean Equals(Object obj)
        {
            BaseRecipeViewModel other = obj as BaseRecipeViewModel;
            if (other == null)
            {
                return false;
            }
            if (!Caption.Equals(other.Caption))
            {
                return false;
            }
            for (int i = 0; i < DerivedRecipes.Count(); i++)
            {
                if (!DerivedRecipes.ElementAt(i).Equals(other.DerivedRecipes.ElementAt(i)))
                {
                    return false;
                }
            }
            if (!Favorite.Equals(other.Favorite))
            {
                return false;
            }
            return base.Equals(obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
