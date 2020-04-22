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

        public IngredientViewModel Ingredient
        {
            get;
            set;
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
            }
        }

        public UnitViewModel Unit
        {
            get;
            set;
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

        public IngredientUsageViewModel(IngredientUsage model)
        {
            _model = model;

            _cache = App.GetInstance().Cache;

            Ingredient = new IngredientViewModel(_model.Ingredient);
            Unit = new UnitViewModel(_model.Unit);
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return String.Format("{0} {1}s of {2}", Amount, Unit.Name, Ingredient.Name);
        }
    }
}
