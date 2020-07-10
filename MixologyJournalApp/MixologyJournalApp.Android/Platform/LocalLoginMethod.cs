using MixologyJournalApp.Platform;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MixologyJournalApp.Droid.Platform
{
    public class LocalLoginMethod : ILoginMethod
    {
        public string Name
        {
            get
            {
                return "Local Android Account";
            }
        }

        public String Icon
        {
            get
            {
                return "icon_auth0";
            }
        }

        public String BackgroundColor
        {
            get
            {
                return "#F8F8F8";
            }
        }

        public String ForegroundColor
        {
            get
            {
                return "#000000";
            }
        }

        private Boolean _isLoggedIn = false;
        public Boolean IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            private set
            {
                _isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(Login);
            }
        }

        public ICommand LogoffCommand
        {
            get
            {
                return new Command(Logoff);
            }
        }

        public User CurrentUser
        {
            get;
            private set;
        }

        public LocalLoginMethod()
        {
        }

        public async Task Init(bool setupMode)
        {
            IsLoggedIn = true;
        }

        private async void Login()
        {
            IsLoggedIn = true;
        }

        private void Logoff()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}