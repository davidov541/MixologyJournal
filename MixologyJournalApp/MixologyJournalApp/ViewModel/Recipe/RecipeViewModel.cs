using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Recipe
{
    internal interface IRecipeViewModel : IViewRecipeViewModel, IEditRecipeViewModel, INotifyPropertyChanged
    {
        void AddIngredient(EditIngredientViewModel ingred);
    }

    internal abstract class RecipeViewModel<T> : IRecipeViewModel where T : SourceModel.Recipe.Recipe
    {
        private List<EditIngredientViewModel> _addedIngredients;
        private List<EditIngredientViewModel> _removedIngredients;
        private ObservableCollection<EditIngredientViewModel> _ingredients;
        private BaseMixologyApp _app;

        public event PropertyChangedEventHandler PropertyChanged;
        private T _recipe;

        public T Model
        {
            get
            {
                return _recipe;
            }
        }

        public String Title
        {
            get;
            private set;
        }

        public abstract String Caption
        {
            get;
        }

        public virtual Symbol? Icon
        {
            get
            {
                return null;
            }
        }

        public String Instructions
        {
            get;
            private set;
        }

        protected BaseMixologyApp App
        {
            get
            {
                return _app;
            }
        }

        internal int ID
        {
            get
            {
                return _recipe.ID;
            }
        }

        private int _servingsNumber = 1;
        public int ServingsNumber
        {
            get
            {
                return _servingsNumber;
            }
            set
            {
                _servingsNumber = value;
                foreach (IEditIngredientViewModel ingred in Ingredients)
                {
                    // Value comes in zero-indexed, but we translate for ease of use below.
                    ingred.Amount.ServingsNumber = value;
                }
                OnPropertyChanged(nameof(ServingsNumber));
            }
        }

        public IEnumerable<IEditIngredientViewModel> Ingredients
        {
            get
            {
                return _ingredients;
            }
        }

        public RecipeViewModel(T model, BaseMixologyApp app)
        {
            _recipe = model;
            _app = app;
            Reset();
        }

        public void SetInstructions(string instructions)
        {
            Instructions = instructions;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void AddIngredient()
        {
            AddIngredient(new EditIngredientViewModel(_app));
        }

        public void AddIngredient(EditIngredientViewModel ingred)
        {
            _ingredients.Add(ingred as EditIngredientViewModel);
            _addedIngredients.Add(ingred as EditIngredientViewModel);
        }

        protected void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Save()
        {
            _recipe.Name = Title;
            _recipe.Instructions = Instructions;
            foreach (EditIngredientViewModel ingred in _ingredients)
            {
                ingred.Save();
            }
            foreach (EditIngredientViewModel ingred in _addedIngredients)
            {
                _recipe.AddIngredient(ingred.Model);
            }
            foreach (EditIngredientViewModel ingred in _removedIngredients)
            {
                _recipe.RemoveIngredient(ingred.Model);
            }
            _addedIngredients.Clear();
            _removedIngredients.Clear();
        }

        public void Cancel()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            Title = _recipe.Name;
            Instructions = _recipe.Instructions;
            _addedIngredients = new List<EditIngredientViewModel>();
            _removedIngredients = new List<EditIngredientViewModel>();
            _ingredients = new ObservableCollection<EditIngredientViewModel>();
            foreach (Ingredient ingredient in _recipe.Ingredients)
            {
                _ingredients.Add(new EditIngredientViewModel(ingredient, _app));
            }
        }

        public void RemoveIngredient(IEditIngredientViewModel ingred)
        {
            _ingredients.Remove(ingred as EditIngredientViewModel);
            _removedIngredients.Add(ingred as EditIngredientViewModel);
        }

        public override Boolean Equals(Object obj)
        {
            RecipeViewModel<T> other = obj as RecipeViewModel<T>;
            if (other == null)
            {
                return false;
            }
            if (!other.Title.Equals(Title))
            {
                return false;
            }
            if (!other.Instructions.Equals(Instructions))
            {
                return false;
            }
            if (_ingredients.Count != other._ingredients.Count)
            {
                return false;
            }
            for (int i = 0; i < _ingredients.Count; i++)
            {
                if (!_ingredients[i].Equals(other._ingredients[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
