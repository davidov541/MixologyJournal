using MixologyJournalApp.MAUI.Views;

namespace MixologyJournalApp.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RecipeViewPage), typeof(RecipeViewPage));
        }
    }
}