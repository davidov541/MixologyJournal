using MixologyJournalApp.Platform;
using MixologyJournalApp.View;
using MixologyJournalApp.View.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MixologyJournalApp.ViewModel
{
    internal class RootPageMasterViewModel : INotifyPropertyChanged
    {
        private readonly AuthenticationManager _authentication;

        public ObservableCollection<IMasterMenuItem> MenuItems { get; set; }

        public String UserName
        {
            get
            {
                return _authentication.User?.Name;
            }
        }

        public Uri UserIcon
        {
            get
            {
                return _authentication.User?.IconPath;
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
            if (app.PlatformInfo.Authentication.IsAuthenticated)
            {
            }
            _authentication = app.PlatformInfo.Authentication;
            _authentication.PropertyChanged += Authentication_PropertyChanged;
        }

        private void Authentication_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AuthenticationManager.User):
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
