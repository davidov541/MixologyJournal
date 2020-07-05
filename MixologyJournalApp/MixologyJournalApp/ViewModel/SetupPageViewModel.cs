using MixologyJournalApp.Platform;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
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

        public Command ChangePositionCommand
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
                ChangePositionCommand.ChangeCanExecute();
                OnPropertyChanged(nameof(Position));
            }
        }

        public SetupPageViewModel(IPlatform platform)
        {
            _platform = platform;
            _platform.Backend.PropertyChanged += Backend_PropertyChanged;

            PageItems = new ObservableCollection<SetupPageItem>()
            {
                new SetupPageItem("Welcome to Mixology Journal!\n\nYou've taken your first step to improving your cocktail making skills!"),
                new SetupPageItem("With Mixology Journal, you can log every variation of a recipe you create. \n\nWhen you find a favorite variation, you can keep that for use later!"),
                new SetupPageItem("In order to create custom recipes and log your drinks, you need to log in using your Google account.\n\nIf you decide not to, you can do so later, and will have access to classic recipes.", _platform.Backend.LoginMethods)
            };

            ChangePositionCommand = new Command(ChangePosition, CanChangePosition);
        }

        private bool CanChangePosition(Object parameter)
        {
            switch (int.Parse(parameter.ToString()))
            {
                case 1:
                    return Position != PageItems.Count - 1;
                case -1:
                    return Position != 0;
                default:
                    return false;
            }
        }

        private void ChangePosition(Object parameter)
        {
            int direction = int.Parse(parameter.ToString());
            Position += direction;
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Backend_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IBackend.IsAuthenticated))
            {
                await (Application.Current as App).StartApp();
            }
        }
    }
}
