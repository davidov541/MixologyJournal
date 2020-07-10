using MixologyJournalApp.Platform;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace MixologyJournalApp.ViewModel
{
    internal class SetupPageViewModel: INotifyPropertyChanged
    {
        private readonly IPlatform _platform;

        public ObservableCollection<SetupPageItem> PageItems
        {
            get;
            private set;
        }

        private int _position;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public SetupPageViewModel(IPlatform platform)
        {
            _platform = platform;
            _platform.Authentication.PropertyChanged += Authentication_PropertyChanged;

            PageItems = new ObservableCollection<SetupPageItem>()
            {
                new SetupPageItem("Welcome to Mixology Journal!\n\nYou've taken your first step to\nimproving your cocktail making skills!"),
                new SetupPageItem("With Mixology Journal, you can log every variation of a recipe you create. \n\nWhen you find a favorite variation, you can keep that for use later!"),
                new SetupPageItem("In order to create custom recipes and log your drinks,\nyou need to log in using your Google account.\n\nIf you decide not to you can do so later.\nRegardless, you will have access to classic recipes.", _platform.Authentication.LoginMethods)
            };
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Authentication_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AuthenticationManager.IsAuthenticated))
            {
                await (Application.Current as App).StartApp();
            }
        }
    }
}
