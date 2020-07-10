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
                return "Local Account";
            }
        }

        public String Icon
        {
            get
            {
                return "local_login_48";
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
                return false;
            }
       }

        public Boolean IsEnabled
        {
            get
            {
                return true;
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
            get
            {
                return new User("This Phone", new Uri("https://storageaccountmixolb7df.blob.core.windows.net/resources/icons8-phone-case-96.png"), "", "");
            }
        }

        public LocalLoginMethod()
        {
        }

        public async Task Init(bool setupMode)
        {
        }

        private async void Login()
        {
            LoginEnabled?.Invoke(this, new EventArgs());
        }

        private void Logoff()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler LoginEnabled;
    }
}