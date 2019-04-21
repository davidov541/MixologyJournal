using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.List;
using MixologyJournal.ViewModel.Recipe;

namespace MixologyJournal.ViewModel.Entry
{
    public interface IBaseRecipePageViewModel : IListableViewModel, IPageViewModel
    {
        IBaseViewRecipeViewModel Recipe
        {
            get;
        }

        Task DeleteAsync();
    }

    public interface IBaseRecipeEditPageViewModel : IListableViewModel, IPageViewModel, ISaveablePageViewModel
    {
        IBaseEditRecipeViewModel Recipe
        {
            get;
        }

        void AddIngredient();
    }

    internal class BaseRecipePageViewModel : IBaseRecipeEditPageViewModel, IBaseRecipePageViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BaseMixologyApp _app;

        private BaseRecipeViewModel _recipe;
        IBaseViewRecipeViewModel IBaseRecipePageViewModel.Recipe
        {
            get
            {
                return _recipe;
            }
        }

        public IBaseEditRecipeViewModel Recipe
        {
            get
            {
                return _recipe;
            }
        }

        public IViewRecipeViewModel Favorite
        {
            get
            {
                return _recipe.Favorite;
            }
        }

        public String Title
        {
            get
            {
                return _recipe.Title;
            }
        }

        public String Caption
        {
            get
            {
                return _recipe.Caption;
            }
        }

        public Symbol? Icon
        {
            get
            {
                return null;
            }
        }

        public int ID
        {
            get
            {
                return _recipe.ID;
            }
        }

        private List<HomemadeDrinkEntryViewModel> _derivedRecipes;

        public IEnumerable<HomemadeDrinkEntryViewModel> DerivedRecipes
        {
            get
            {
                return _derivedRecipes;
            }
        }

        public IEnumerable<HomemadeDrinkEntryViewModel> DerivedRecipesWithoutFavorite
        {
            get
            {
                return DerivedRecipes.Where(r => !r.IsFavorite);
            }
        }

        private bool _busy = false;
        public bool Busy
        {
            get
            {
                return _busy;
            }
            private set
            {
                _busy = value;
                OnPropertyChanged(nameof(Busy));
            }
        }

        internal BaseRecipePageViewModel(BaseMixologyApp app) :
            this(new BaseRecipeViewModel(app), app)
        {
        }

        internal BaseRecipePageViewModel(BaseRecipeViewModel recipe, BaseMixologyApp app)
        {
            _recipe = recipe;
            _derivedRecipes = new List<HomemadeDrinkEntryViewModel>();
            _recipe.PropertyChanged += Recipe_PropertyChanged;
            _app = app;
        }

        private void Recipe_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(BaseRecipeViewModel.Favorite)))
            {
                OnPropertyChanged(nameof(Favorite));
            }
        }

        public async Task SaveAsync()
        {
            Busy = true;
            _recipe.Save();
            await _app.Persister.SaveAsync(_app.Journal);
            Busy = false;
        }

        public void Cancel()
        {
            _recipe.Cancel();
        }

        public void AddIngredient()
        {
            _recipe.AddIngredient();
        }

        internal void AddModifiedRecipe(HomemadeDrinkEntryViewModel page)
        {
            _derivedRecipes.Add(page);
            _recipe.AddModifiedRecipe(page.Recipe);
        }

        protected void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task DeleteAsync()
        {
            Busy = true;
            await _app.Journal.RemoveRecipeAsync(this);
            Busy = false;
        }
    }
}
