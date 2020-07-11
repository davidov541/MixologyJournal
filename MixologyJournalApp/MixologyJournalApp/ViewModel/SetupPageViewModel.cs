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
            _platform.Authentication.LoginEnabled += Authentication_LoginEnabled;

            PageItems = new ObservableCollection<SetupPageItem>()
            {
                new SetupPageItem("Welcome to Mixology Journal!\n\nYou've taken your first step to\nimproving your cocktail making skills!"),
                new SetupPageItem("With Mixology Journal, you can log every variation of a recipe you create.\n\nWhen you find a favorite variation, you can keep that for use later!"),
                new SetupPageItem("You can choose to save your recipes and drinks locally, or store them in the cloud, secured with your Google account or a custom account.\n\nIf you save your data in the cloud, you will be able to access it on other devices and platforms!", _platform.Authentication.LoginMethods)
            };
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Authentication_LoginEnabled(object sender, EventArgs e)
        {
            await (Application.Current as App).StartApp();
        }
    }
}
