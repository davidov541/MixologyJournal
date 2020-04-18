using MixologyJournalApp.ViewModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        private MainPageViewModel _nextPageVM;
        public LoadingPage()
        {
            InitializeComponent();
            
        }

        public async Task LoadAsync()
        {
            _nextPageVM = new MainPageViewModel();
            await _nextPageVM.UpdateRecipes();
            NavigationPage navigationPage = new NavigationPage(new MainPage(_nextPageVM));
            await Navigation.PushAsync(navigationPage);
        }
    }
}