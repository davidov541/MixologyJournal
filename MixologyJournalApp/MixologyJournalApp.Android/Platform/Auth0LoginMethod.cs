using MixologyJournalApp.Platform;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace MixologyJournalApp.Droid.Platform
{
    internal class Auth0LoginMethod : IRemoteLoginMethod
    {
        private const String RenewalTokenKey = "RenewalToken";

        public String Name
        {
            get
            {
                return "Cloud Storage";
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

        public Boolean IsLoggedIn
        {
            get
            {
                return IsEnabled;
            }
            private set
            {
                IsEnabled = value;
                OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        public Boolean IsEnabled { get; private set; } = false;


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

        public async Task Init(bool setupMode)
        {
            IsLoggedIn = false;
            if (!setupMode)
            {
                try
                {
                    String renewalToken = await SecureStorage.GetAsync(RenewalTokenKey);
                    if (!String.IsNullOrEmpty(renewalToken))
                    {
                        CurrentUser = await _mainActivity.RunRenewalActivity(renewalToken);
                        IsLoggedIn = CurrentUser != null;
                        await SecureStorage.SetAsync(RenewalTokenKey, CurrentUser.RefreshToken);
                    }
                }
                catch (Exception)
                {
                    // We weren't able to get a renewal token. 
                    // Likely this is a cold boot, so we should just ignore this and move on.
                }
            }
        }

        private async void Login()
        {
            CurrentUser = await _mainActivity.RunLoginActivity();
            if (CurrentUser != null)
            {
                await SecureStorage.SetAsync(RenewalTokenKey, CurrentUser.RefreshToken);
                IsLoggedIn = true;
                LoginEnabled?.Invoke(this, new EventArgs());
            }
            else
            {
                IsLoggedIn = false;
            }
        }

        private void Logoff()
        {
            LoggingOff?.Invoke(this, new EventArgs());
            SecureStorage.Remove(RenewalTokenKey);
            IsLoggedIn = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler LoginEnabled;

        public event EventHandler LoggingOff;
    }
}