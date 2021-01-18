using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class RecipeListPageViewModel: INotifyPropertyChanged
    {
        private readonly LocalDataCache _cache;
        private readonly App _app;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsEmpty
        {
            get
            {
                return _cache.Recipes.Count == 0;
            }
        }

        public double InitProgress
        {
            get
            {
                return _cache.InitProgress;
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
            _cache.PropertyChanged += Cache_PropertyChanged;
        }

        private void Cache_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(LocalDataCache.InitProgress):
                    OnPropertyChanged(nameof(InitProgress));
                    break;
            }
        }

        private void Recipes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Recipes));
            OnPropertyChanged(nameof(IsEmpty));
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
