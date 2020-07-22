using MixologyJournalApp.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class IngredientUsageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IngredientUsage _model;
        private readonly LocalDataCache _cache;
        private readonly App _app;

        private IngredientViewModel _ingredient;
        public IngredientViewModel Ingredient
        {
            get
            {
                return _ingredient;
            }
            set
            {
                _ingredient = value;
                if (_ingredient != null)
                {
                    _model.Ingredient = _ingredient.Model;
                }
                OnPropertyChanged(nameof(Ingredient));
                OnPropertyChanged(nameof(FullDescription));
            }
        }

        public String FormattedAmount
        {
            get
            {
                return Amount + " " + _unit.ToString();
            }
        }

        public String Amount
        {
            get
            {
                return _model.Amount;
            }
            set
            {
                _model.Amount = value;
                OnPropertyChanged(nameof(Amount));
                OnPropertyChanged(nameof(FullDescription));
            }
        }

        private UnitViewModel _unit;
        public UnitViewModel Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
                if (_unit != null)
                {
                    _model.Unit = _unit.Model;
                }
                OnPropertyChanged(nameof(Unit));
                OnPropertyChanged(nameof(FullDescription));
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

        public String FullDescription
        {
            get
            {
                return ToString();
            }
        }

        public IngredientUsageViewModel(IngredientUsage model, App app)
        {
            _model = model;
            _app = app;
            _cache = _app.Cache;

            Ingredient = new IngredientViewModel(_model.Ingredient);
            Unit = new UnitViewModel(_model.Unit);
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(Amount.ToString()) || String.IsNullOrEmpty(Unit.Name) || String.IsNullOrEmpty(Ingredient.Name))
            {
                return String.Empty;
            }
            return String.Format("{0} {1}s of {2}", Amount, Unit.Name, Ingredient.Name);
        }
    }
}
