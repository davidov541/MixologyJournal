using MixologyJournalApp.Platform;
using MixologyJournalApp.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetupPage : ContentPage
    {
        private readonly SetupPageViewModel _viewModel;
        public SetupPage(IPlatform platform)
        {
            _viewModel = new SetupPageViewModel(platform);
            BindingContext = _viewModel;
            InitializeComponent();
       }

        private async void SkipButton_Clicked(object sender, System.EventArgs e)
        {
            await (App.Current as App).StartApp();
        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Right:
                    _viewModel.GoToPrevious();
                    break;
                case SwipeDirection.Left:
                    _viewModel.GoToNext();
                    break;
                case SwipeDirection.Up:
                case SwipeDirection.Down:
                default:
                    break;
            }
        }
    }
}