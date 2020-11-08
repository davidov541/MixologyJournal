using MixologyJournalApp.Platform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace MixologyJournalApp.ViewModel
{
    internal class SetupPageViewModel: INotifyPropertyChanged
    {
        private readonly IPlatform _platform;

        public ObservableCollection<SetupPageItem> PageItems
        {
            get;
            private set;
        }

        private int _position;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
                OnPropertyChanged(nameof(ImageSource));
                OnPropertyChanged(nameof(CurrentItem));
                OnPropertyChanged(nameof(Caption));
                OnPropertyChanged(nameof(ImageVisible));
                OnPropertyChanged(nameof(LoginButtonsVisible));
                OnPropertyChanged(nameof(LoginMethods));
            }
        }

        private SetupPageItem CurrentItem
        {
            get
            {
                return PageItems.ElementAt(Position);
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                return CurrentItem.ImageSource;
            }
        }

        public String Caption
        {
            get
            {
                return CurrentItem.Caption;
            }
        }

        public Boolean LoginButtonsVisible
        {
            get
            {
                return CurrentItem.Type == SetupPageItem.ItemType.Login;
            }
        }

        public Boolean ImageVisible
        {
            get
            {
                return CurrentItem.Type == SetupPageItem.ItemType.Image;
            }
        }

        public IEnumerable<ILoginMethod> LoginMethods
        {
            get
            {
                return CurrentItem.LoginMethods;
            }
        }

        public SetupPageViewModel(IPlatform platform)
        {
            _platform = platform;
            _platform.Authentication.LoginEnabled += Authentication_LoginEnabled;

            PageItems = new ObservableCollection<SetupPageItem>()
            {
                new SetupPageItem("Welcome to Mixology Journal!\n\nYou've taken your first step to\nimproving your cocktail making skills.", ImageSource.FromFile("gt_chalk_transparent.png")),
                new SetupPageItem("With Mixology Journal, you can log every variation of a recipe you create.\n\nWhen you find a favorite variation, you can keep that for use later."),
                new SetupPageItem("First, add recipes for cocktails that you like to make often.\n\nThese can be from books, your notes, or from your head."),
                new SetupPageItem("Then, when you create drinks based on those recipes, you can log how it turned out.\n\nThis might include new brands or ingredients, or different amounts."),
                new SetupPageItem("Using this, you can track variations in how you make a recipe that make a drink taste better or worse.\n\nThat way, you can continue improving and refining your recipes."),
                new SetupPageItem("Once you find a variation you like most, you can favorite it.\nThis variation will be shown to you when you look at the recipe by default.\n\nThat way, when you go back to create the recipe again, you'll start with your best possible variation!"),
                new SetupPageItem("You can choose to save your recipes and drinks locally, or store them in the cloud, secured with your Google account or a custom account.\n\nIf you save your data in the cloud, you will be able to access it on other devices and platforms!", _platform.Authentication.LoginMethods)
            };
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Authentication_LoginEnabled(object sender, EventArgs e)
        {
            await (Application.Current as App).StartApp();
        }
    }
}
