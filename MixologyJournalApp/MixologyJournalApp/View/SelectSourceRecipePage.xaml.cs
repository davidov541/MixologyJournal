using MixologyJournalApp.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectSourceRecipePage : ContentPage
    {
        internal class BasisChangedEventArgs: EventArgs
        {
            internal RecipeViewModel NewBasis
            {
                get;
                private set;
            }

            internal BasisChangedEventArgs(RecipeViewModel newBasis)
            {
                NewBasis = newBasis;
            }
        }
        private readonly SelectSourceRecipePageViewModel _vm;

        internal event EventHandler<BasisChangedEventArgs> BasisChanged;

        private RecipeViewModel _basis;
        internal RecipeViewModel Basis
        {
            get
            {
                return _basis;
            }
            private set
            {
                _basis = value;
                BasisChanged?.Invoke(this, new BasisChangedEventArgs(value));
            }
        }

        internal SelectSourceRecipePage(App app)
        {
            _vm = new SelectSourceRecipePageViewModel(app);
            BindingContext = _vm;
            InitializeComponent();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Basis = e.Item as RecipeViewModel;
            await Navigation.PopModalAsync();
        }
    }
}