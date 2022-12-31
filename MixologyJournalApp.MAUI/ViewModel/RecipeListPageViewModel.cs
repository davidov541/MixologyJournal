using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MixologyJournalApp.MAUI.ViewModel
{
    internal class RecipeListPageViewModel : INotifyPropertyChanged
    {
        private readonly AppViewModel _viewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        //public bool IsEmpty
        //{
        //    get
        //    {
        //        return _cache.Recipes.Count == 0;
        //    }
        //}

        //public ObservableCollection<RecipeViewModel> Recipes
        //{
        //    get
        //    {
        //        return _cache.Recipes;
        //    }
        //}

        public ObservableCollection<UnitViewModel> Units
        {
            get {
                return this._viewModel.Units;
            }
        }

        public RecipeListPageViewModel(AppViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Recipes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //OnPropertyChanged(nameof(Recipes));
            //OnPropertyChanged(nameof(IsEmpty));
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal async Task InitializeAsync()
        {
            await this._viewModel.InitializeAsync();
        }
    }
}
