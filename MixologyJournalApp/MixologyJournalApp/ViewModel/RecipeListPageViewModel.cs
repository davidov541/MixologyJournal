﻿using MixologyJournalApp.Platform;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MixologyJournalApp.ViewModel
{
    internal class RecipeListPageViewModel: INotifyPropertyChanged
    {
        private readonly LocalDataCache _cache;
        private readonly App _app;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsAuthenticated
        {
            get
            {
                bool authenticated = false;
                try
                {
                    authenticated = _app.PlatformInfo.Backend.IsAuthenticated;
                } catch (InvalidOperationException)
                {
                    // We haven't created the App object yet, so we should just return that we aren't authenticated.
                }
                return authenticated;
            }
        }

        public bool IsUnauthenticated
        {
            get
            {
                return !IsAuthenticated;
            }
        }

        public ObservableCollection<RecipeViewModel> Recipes
        {
            get
            {
                return _cache.Recipes;
            }
        }

        public RecipeListPageViewModel(App app)
        {
            _app = app;
            _cache = _app.Cache;
            _cache.Recipes.CollectionChanged += Recipes_CollectionChanged;
        }

        private void Recipes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Recipes));
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
