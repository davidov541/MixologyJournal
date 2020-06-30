using MixologyJournalApp.Platform;
using MixologyJournalApp.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MixologyJournalApp.ViewModel
{
    internal class RootPageMasterViewModel : INotifyPropertyChanged
    {
        private IBackend _backend;

        public ObservableCollection<IMasterMenuItem> MenuItems { get; set; }

        public String UserName
        {
            get
            {
                return _backend.User?.Name;
            }
        }

        public Uri UserIcon
        {
            get
            {
                return _backend.User?.IconPath;
            }
        }

        public RootPageMasterViewModel(App app)
        {
            MenuItems = new ObservableCollection<IMasterMenuItem>(new IMasterMenuItem[]
            {
                    new MasterMenuItem<RecipeListPage>("Recipes"),
                    new MasterMenuItem<DrinkListPage>("Drinks"),
                    new MasterMenuItem<SettingsPage>("Settings")
            });
            _backend = app.PlatformInfo.Backend;
            _backend.PropertyChanged += Backend_PropertyChanged;
        }

        private void Backend_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IBackend.User):
                    OnPropertyChanged(nameof(UserName));
                    OnPropertyChanged(nameof(UserIcon));
                    break;
                default:
                    break;
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
