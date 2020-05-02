using MixologyJournalApp.Platform;
using System.Collections.Generic;
using System.ComponentModel;

namespace MixologyJournalApp.ViewModel
{
    internal class SetupPageViewModel
    {
        private IPlatform _platform;
        public IEnumerable<ILoginMethod> LoginMethods
        {
            get
            {
                return _platform.Backend.LoginMethods;
            }
        }

        public SetupPageViewModel(IPlatform platform)
        {
            _platform = platform;
            _platform.Backend.PropertyChanged += Backend_PropertyChanged;
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
