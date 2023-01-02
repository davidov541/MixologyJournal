using MixologyJournalApp.MAUI.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MixologyJournalApp.MAUI.ViewModel
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private AppModel _model = null;

        public ObservableCollection<UnitViewModel> Units
        {
            get;
            private set;
        } = new();

        public ObservableCollection<RecipeViewModel> Recipes
        {
            get;
            private set;
        } = new();

        public bool Initializing
        {
            get
            {
                return this._model.Initializing;
            }
        }

        public AppViewModel()
        {
            this._model = new AppModel();
            this._model.PropertyChanged += Model_PropertyChanged;
            this._model.Recipes.CollectionChanged += Recipes_CollectionChanged;
            this._model.Units.CollectionChanged += Units_CollectionChanged;
        }

        private void Recipes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (Recipe recipe in e.NewItems.OfType<Recipe>())
                        {
                            this.Recipes.Add(new RecipeViewModel(recipe));
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                        break;
                }
            });
        }

        private void Units_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (Unit unit in e.NewItems.OfType<Unit>())
                        {
                            this.Units.Add(new UnitViewModel(unit));
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                        break;
                }
            });
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AppModel.Initializing):
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Initializing)));
                    break;
            }
        }

        internal async Task InitializeAsync()
        {
            await this._model.InitializeAsync();
        }
    }
}
