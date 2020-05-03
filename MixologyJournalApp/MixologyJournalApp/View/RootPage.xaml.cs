using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPage : MasterDetailPage
    {
        private readonly App _app;
        private RootPageMaster _master;

        public RootPage(App app)
        {
            _app = app;
            _master = new RootPageMaster(new RootPageMasterViewModel(_app));
            Master = _master;
            Detail = new NavigationPage(new RecipeListPage(_app));
            InitializeComponent();
            _master.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is IMasterMenuItem item))
            {
                return;
            }

            Page page = (Page)Activator.CreateInstance(item.TargetType, _app);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            _master.ListView.SelectedItem = null;
        }
    }
}