﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class DrinkListPageViewModel: INotifyPropertyChanged
    {
        private readonly LocalDataCache _cache;
        private readonly App _app;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsEmpty
        {
            get
            {
                return _cache.Drinks.Count == 0;
            }
        }

        public ObservableCollection<DrinkViewModel> Drinks
        {
            get
            {
                return _cache.Drinks;
            }
        }

        public DrinkListPageViewModel(App app)
        {
            _app = app;
            _cache = _app.Cache;
            _cache.Drinks.CollectionChanged += Drinks_CollectionChanged;
        }

        private void Drinks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Drinks));
            OnPropertyChanged(nameof(IsEmpty));
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
