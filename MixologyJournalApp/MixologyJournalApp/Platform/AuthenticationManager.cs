using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MixologyJournalApp.Platform
{
    public class AuthenticationManager: INotifyPropertyChanged
    {
        public Boolean IsAuthenticated
        {
            get
            {
                return _loginMethods.Any(m => m.IsLoggedIn);
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

        public AuthenticationManager(IEnumerable<ILoginMethod> loginMethods)
        {
            _loginMethods.AddRange(loginMethods);
            _loginMethods.ForEach(l =>
            {
                l.PropertyChanged += LoginMethod_PropertyChanged;
                l.LoginEnabled += LoginMethod_LoginEnabled;
            });
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
                case nameof(ILoginMethod.IsLoggedIn):
                    OnPropertyChanged(nameof(IsAuthenticated));
                    OnPropertyChanged(nameof(User));
                    break;
                default:
                    break;
            }
        }

        public async Task Init(bool setupMode)
        {
            IEnumerable<Task> initializationTasks = _loginMethods.Select(l => l.Init(setupMode));
            await Task.WhenAll(initializationTasks);
        }
    }
}
