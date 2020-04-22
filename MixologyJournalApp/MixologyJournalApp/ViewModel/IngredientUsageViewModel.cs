﻿using MixologyJournalApp.Model;
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

            Ingredient = new IngredientViewModel(_model.Ingredient);
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
