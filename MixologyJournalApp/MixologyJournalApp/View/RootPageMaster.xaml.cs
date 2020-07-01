using MixologyJournalApp.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPageMaster : ContentPage
    {
        public ListView ListView;

        internal RootPageMaster(RootPageMasterViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
            ListView = MenuItemsListView;
        }

        
    }
}