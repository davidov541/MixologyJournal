using MixologyJournalApp.Platform;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MixologyJournalApp.Droid.Platform
{
    internal class Auth0LoginMethod : ILoginMethod
    {
        public String Name
        {
            get
            {
                return "Auth0";
            }
        }

        public String Icon
        {
            get
            {
                return "ic_google";
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

        private readonly MainActivity _mainActivity;

        public Auth0LoginMethod(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        public async Task Init()
        {
            IsLoggedIn = false;
        }

        private async void Login()
        {
            CurrentUser = await _mainActivity.StartLoginActivity();
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