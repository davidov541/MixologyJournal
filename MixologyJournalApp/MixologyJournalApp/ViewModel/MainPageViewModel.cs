using MixologyJournalApp.Model;
using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MixologyJournalApp.ViewModel
{
    internal class MainPageViewModel: INotifyPropertyChanged
    {
        private LocalDataCache _cache;

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

        public ObservableCollection<RecipeViewModel> Recipes
        {
            get
            {
                return _cache.Recipes;
            }
        }

        public MainPageViewModel()
        {
            _cache = App.GetInstance().Cache;
            _cache.Recipes.CollectionChanged += Recipes_CollectionChanged;
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
                    platform.AlertDialogFactory.showDialog("Sign-in result", message);
                }
            }
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsUnauthenticated));

            await _cache.Resync();
        }

        public async Task LogOff()
        {
            IPlatform platform = App.GetInstance().PlatformInfo;
            if (platform.Backend != null)
            {
                await platform.Backend.LogOffAsync();
                platform.AlertDialogFactory.showDialog("Sign-out result", "Logged out");
            }
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsUnauthenticated));

            await _cache.Resync();
        }
    }
}
