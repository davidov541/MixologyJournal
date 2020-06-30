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
        public ObservableCollection<IMasterMenuItem> MenuItems { get; set; }

        private String _userName;
        public String UserName
        {
            get
            {
                return _userName;
            }
            private set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private Uri _userIcon;
        public Uri UserIcon
        {
            get
            {
                return _userIcon;
            }
            private set
            {
                _userIcon = value;
                OnPropertyChanged(nameof(UserIcon));
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
            User currentUser = app.PlatformInfo.Backend.User;
            if (currentUser != null)
            {
                UserName = currentUser.Name;
                UserIcon = currentUser.IconPath;
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
