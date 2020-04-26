using MixologyJournalApp.Platform;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MixologyJournalApp.ViewModel
{
    internal class MainPageViewModel: INotifyPropertyChanged
    {
        private readonly LocalDataCache _cache;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsAuthenticated
        {
            get
            {
                bool authenticated = false;
                try
                {
                    authenticated = App.GetInstance().PlatformInfo.Backend.IsAuthenticated;
                } catch (InvalidOperationException)
                {
                    // We haven't created the App object yet, so we should just return that we aren't authenticated.
                }
                return authenticated;
            }
        }

        public bool IsUnauthenticated
        {
            get
            {
                return !IsAuthenticated;
            }
        }

        public ICommand LoginCommand
        {
            get;
            private set;
        }

        public ICommand LogoffCommand
        {
            get;
            private set;
        }

        public ObservableCollection<RecipeViewModel> Recipes
        {
            get
            {
                return _cache.Recipes;
            }
        }

        public MainPageViewModel()
        {
            SetCommands();

            _cache = App.GetInstance().Cache;
            _cache.Recipes.CollectionChanged += Recipes_CollectionChanged;
        }

        private void SetCommands()
        {
            LoginCommand = new Command(
                execute: async () =>
                {
                    await LogIn();
                },
                canExecute: () =>
                {
                    return IsUnauthenticated;
                });
            LogoffCommand = new Command(
                execute: async () =>
                {
                    await LogOff();
                },
                canExecute: () =>
                {
                    return IsAuthenticated;
                });
        }

        private void Recipes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Recipes));
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LogIn()
        {
            IPlatform platform = App.GetInstance().PlatformInfo;
            if (platform.Backend != null)
            {
                if (await App.GetInstance().PlatformInfo.Backend.Authenticate())
                {
                    // Display the success or failure message.
                    String message = string.Format("you are now signed-in as {0}.", platform.Backend.User.UserId); ;
                    platform.AlertDialogFactory.ShowDialog("Sign-in result", message);
                }
            }
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsUnauthenticated));

            (LoginCommand as Command).ChangeCanExecute();
            (LogoffCommand as Command).ChangeCanExecute();

            await _cache.Resync();
        }

        public async Task LogOff()
        {
            IPlatform platform = App.GetInstance().PlatformInfo;
            if (platform.Backend != null)
            {
                await platform.Backend.LogOffAsync();
                platform.AlertDialogFactory.ShowDialog("Sign-out result", "Logged out");
            }
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsUnauthenticated));

            (LoginCommand as Command).ChangeCanExecute();
            (LogoffCommand as Command).ChangeCanExecute();

            await _cache.Resync();
        }
    }
}
