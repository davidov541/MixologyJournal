using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MixologyJournalApp.ViewModel
{
    internal class ModifyIngredientPageViewModel : INotifyPropertyChanged
    {
        private IngredientUsageViewModel _ingredient;
        private readonly LocalDataCache _cache;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Ingredient Usage Fields
        public IngredientViewModel Ingredient
        {
            get
            {
                return _ingredient.Ingredient;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_ingredient != null || _ingredient.Ingredient != null || !_ingredient.Ingredient.Equals(value))
                {
                    _ingredient.Ingredient = value;
                }
            }
        }

        public String Brand
        {
            get
            {
                return _ingredient.Brand;
            }
            set
            {
                _ingredient.Brand = value;
            }
        }

        public String Amount
        {
            get
            {
                return _ingredient.Amount;
            }
            set
            {
                _ingredient.Amount = value;
            }
        }

        public UnitViewModel Unit
        {
            get
            {
                return _ingredient.Unit;
            }
            set
            {
                _ingredient.Unit = value;
            }
        }
        #endregion

        #region Categorization Information
        private CategoryViewModel _chosenCategory;
        public CategoryViewModel ChosenCategory
        {
            get
            {
                return _chosenCategory;
            }
            set
            {
                _chosenCategory = value;

                RevealSubcategory();

                OnPropertyChanged(nameof(ChosenCategory));
                OnPropertyChanged(nameof(Subcategories));
                OnPropertyChanged(nameof(AvailableIngredients));
            }
        }

        private CategoryViewModel _chosenSubcategory;
        public CategoryViewModel ChosenSubcategory
        {
            get
            {
                return _chosenSubcategory;
            }
            set
            {
                _chosenSubcategory = value;

                RevealIngredient();

                OnPropertyChanged(nameof(ChosenSubcategory));
                OnPropertyChanged(nameof(AvailableIngredients));
                OnPropertyChanged(nameof(ShowIngredient));
            }
        }

        private Boolean _showSubcategory = false;
        public Boolean ShowSubcategory
        {
            get
            {
                return _showSubcategory;
            }
            private set
            {
                Boolean oldValue = _showSubcategory;
                _showSubcategory = value;
                if (oldValue != value)
                {
                    OnPropertyChanged(nameof(ShowSubcategory));
                }
            }
        }

        private Boolean _showIngredient = false;
        public Boolean ShowIngredient
        {
            get
            {
                return _showIngredient;
            }
            private set
            {
                Boolean oldValue = _showIngredient;
                _showIngredient = value;
                if (oldValue != value)
                {
                    OnPropertyChanged(nameof(ShowIngredient));
                }
            }
        }

        private void RevealSubcategory()
        {
            ShowSubcategory = ChosenCategory != null && Subcategories.Any();
            if (!Subcategories.Contains(ChosenSubcategory))
            {
                ChosenSubcategory = null;
            }
        }

        private void RevealIngredient()
        {
            ShowIngredient = AvailableIngredients.Any();
            if (!AvailableIngredients.Contains(Ingredient))
            {
                _ingredient.Ingredient = null;
            }
        }
        #endregion

        #region Drop-Down Populations
        public ObservableCollection<IngredientViewModel> AvailableIngredients
        {
            get
            {
                List<IngredientViewModel> ingredients = new List<IngredientViewModel>();
                if (_chosenCategory != null)
                {
                    ingredients.AddRange(_chosenCategory.Ingredients);
                }

                if (_chosenSubcategory != null)
                {
                    ingredients.AddRange(_chosenSubcategory.Ingredients);
                }

                return new ObservableCollection<IngredientViewModel>(ingredients);
            }
        }

        public ObservableCollection<UnitViewModel> AvailableUnits
        {
            get
            {
                return _cache.AvailableUnits;
            }
        }

        public ObservableCollection<CategoryViewModel> Categories
        {
            get
            {
                return _cache.TopLevelCategories;
            }
        }

        public ObservableCollection<CategoryViewModel> Subcategories
        {
            get
            {
                if (_chosenCategory == null)
                {
                    return new ObservableCollection<CategoryViewModel>();
                }
                return new ObservableCollection<CategoryViewModel>(_chosenCategory.Subcategories);
            }
        }
        #endregion

        public ModifyIngredientPageViewModel(IngredientUsageViewModel ingredient, App app)
        {
            _ingredient = ingredient;
            _ingredient.PropertyChanged += Ingredient_PropertyChanged;
            _cache = app.Cache;

            IngredientViewModel previousIngredient = Ingredient;
            ChosenCategory = Categories.FirstOrDefault(c => c.ContainsIngredient(previousIngredient));
            ChosenSubcategory = Subcategories.FirstOrDefault(c => c.ContainsIngredient(previousIngredient));
            Ingredient = previousIngredient;
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Ingredient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IngredientUsageViewModel.Unit):
                    OnPropertyChanged(nameof(Unit));
                    break;
                case nameof(IngredientUsageViewModel.Amount):
                    OnPropertyChanged(nameof(Amount));
                    break;
                case nameof(IngredientUsageViewModel.Brand):
                    OnPropertyChanged(nameof(Brand));
                    break;
                case nameof(IngredientUsageViewModel.Ingredient):
                    OnPropertyChanged(nameof(Ingredient));
                    break;
            }
        }

        #region State Management
        private IngredientUsageViewModel _state;

        internal void SaveCurrentState()
        {
            _state = _ingredient.Clone() as IngredientUsageViewModel;
        }


        internal void RestoreFromState()
        {
            if (_state != null)
            {
                _ingredient.RestoreFromState(_state);
                _state = null;
            }
        }
        #endregion
    }
}
