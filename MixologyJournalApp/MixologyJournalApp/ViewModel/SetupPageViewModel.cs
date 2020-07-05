using MixologyJournalApp.Platform;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class SetupPageViewModel
    {
        private readonly IPlatform _platform;

        public ObservableCollection<SetupPageItem> PageItems
        {
            get;
            private set;
        }

        public SetupPageViewModel(IPlatform platform)
        {
            _platform = platform;
            _platform.Backend.PropertyChanged += Backend_PropertyChanged;

            PageItems = new ObservableCollection<SetupPageItem>()
            {
                new SetupPageItem("Welcome to Mixology Journal! Please log in with your authentication mechanism of choice.", SetupPageItem.ItemType.Login, _platform.Backend.LoginMethods)
            };
        }

        private async void Backend_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IBackend.IsAuthenticated))
            {
                await (App.Current as App).StartApp();
            }
        }
    }
}
