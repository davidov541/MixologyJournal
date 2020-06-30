using MixologyJournalApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsPageViewModel _viewModel;
        public SettingsPage(App app)
        {
            _viewModel = new SettingsPageViewModel(app);
            BindingContext = _viewModel;
            InitializeComponent();
        }

        private void LogOutButton_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}