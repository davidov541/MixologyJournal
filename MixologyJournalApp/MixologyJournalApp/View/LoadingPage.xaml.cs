using MixologyJournalApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        private readonly LocalDataCache _cache;

        internal LoadingPage(LocalDataCache cache)
        {
            _cache = cache;
            BindingContext = _cache;
            InitializeComponent();
        }
    }
}