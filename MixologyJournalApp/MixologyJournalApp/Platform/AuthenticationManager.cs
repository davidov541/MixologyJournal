using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace MixologyJournalApp.Platform
{
    public class AuthenticationManager: INotifyPropertyChanged
    {
        public Boolean IsAuthenticated
        {
            get
            {
                return _loginMethods.OfType<IRemoteLoginMethod>().Any(m => m.IsLoggedIn);
            }
        }

        public Boolean IsUsingRemote
        {
            get
            {
                return _loginMethods.OfType<IRemoteLoginMethod>().Any(m => m.IsEnabled);
            }
        }

        private readonly List<ILoginMethod> _loginMethods = new List<ILoginMethod>();
        public IEnumerable<ILoginMethod> LoginMethods
        {
            get
            {
                return _loginMethods;
            }
        }

        public User User
        {
            get
            {
                return _loginMethods.FirstOrDefault(m => m.IsEnabled).CurrentUser;
            }
        }

        public event EventHandler LoggingOff;

        public AuthenticationManager(IEnumerable<ILoginMethod> loginMethods)
        {
            _loginMethods.AddRange(loginMethods);
            _loginMethods.ForEach(l =>
            {
                l.PropertyChanged += LoginMethod_PropertyChanged;
                l.LoginEnabled += LoginMethod_LoginEnabled;
            });
            _loginMethods.OfType<IRemoteLoginMethod>().ForEach(l =>
            {
                l.LoggingOff += Method_LoggingOff;
            });
        }

        private void Method_LoggingOff(object sender, EventArgs e)
        {
            LoggingOff?.Invoke(this, new EventArgs());
        }

        private void LoginMethod_LoginEnabled(object sender, EventArgs e)
        {
            LoginEnabled?.Invoke(this, new EventArgs());
        }

        public AuthenticationManager(params ILoginMethod[] loginMethods): this(loginMethods.AsEnumerable())
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler LoginEnabled;

        private void LoginMethod_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IRemoteLoginMethod.IsLoggedIn):
                    OnPropertyChanged(nameof(IsAuthenticated));
                    OnPropertyChanged(nameof(User));
                    break;
                default:
                    break;
            }
        }

        public async Task Init(bool setupMode)
        {
            IEnumerable<Task> initializationTasks = _loginMethods.OfType<IRemoteLoginMethod>().Select(l => l.Init(setupMode));
            await Task.WhenAll(initializationTasks);
        }
    }
}
