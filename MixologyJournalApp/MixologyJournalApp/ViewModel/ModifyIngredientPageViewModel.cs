using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class ModifyIngredientPageViewModel : INotifyPropertyChanged
    {
        private IngredientUsageViewModel _ingredient;
        private readonly LocalDataCache _cache;

        public event PropertyChangedEventHandler PropertyChanged;

        public IngredientViewModel Ingredient
        {
            get
            {
                return _ingredient.Ingredient;
            }
            set
            {
                _ingredient.Ingredient = value;
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

        public ObservableCollection<IngredientViewModel> AvailableIngredients
        {
            get
            {
                return _cache.AvailableIngredients;
            }
        }

        public ObservableCollection<UnitViewModel> AvailableUnits
        {
            get
            {
                return _cache.AvailableUnits;
            }
        }

        public ModifyIngredientPageViewModel(IngredientUsageViewModel ingredient, App app)
        {
            _ingredient = ingredient;
            _ingredient.PropertyChanged += Ingredient_PropertyChanged;
            _cache = app.Cache;
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
