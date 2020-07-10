using MixologyJournalApp.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MixologyJournalApp.ViewModel
{
    internal class SettingsPageViewModel: INotifyPropertyChanged
    {
        private readonly IPlatform _platform;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<ILoginMethod> LoginMethods
        {
            get
            {
                return _platform.Authentication.LoginMethods;
            }
        }

        public Boolean IsLoggedIn
        {
            get
            {
                return _platform.Authentication.IsAuthenticated;
            }
        }

        public SettingsPageViewModel(App app)
        {
            _platform = app.PlatformInfo;
            _platform.Authentication.PropertyChanged += Authentication_PropertyChanged;
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Authentication_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AuthenticationManager.IsAuthenticated):
                    OnPropertyChanged(nameof(IsLoggedIn));
                    break;
                default:
                    break;
            }
        }
    }
}
