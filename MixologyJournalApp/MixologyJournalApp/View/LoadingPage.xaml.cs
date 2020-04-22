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
    }
}