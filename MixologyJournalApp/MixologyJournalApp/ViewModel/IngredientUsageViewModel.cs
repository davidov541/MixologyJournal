using MixologyJournalApp.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class IngredientUsageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IngredientUsage _model;
        private LocalDataCache _cache;

        public IngredientViewModel Ingredient
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

        public IngredientUsageViewModel(IngredientUsage model)
        {
            _model = model;

            _cache = App.GetInstance().Cache;

            Ingredient ingredientModel = new Ingredient();
            ingredientModel.Id = _model.Id;
            ingredientModel.Name = _model.Name;
            Ingredient = new IngredientViewModel(ingredientModel);
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
