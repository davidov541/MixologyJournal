using MixologyJournalApp.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardView : Frame
    {
        private RecipeViewModel _context;

        internal CardView(RecipeViewModel recipe)
        {
            _context = recipe;
            BindingContext = _context;
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            // boxCardColor.HeightRequest = boxCardColor.Width / 16 * 9;
            // imgCard.HeightRequest = imgCard.Width / 16 * 9;

        }

        private void Card_Tapped(object sender, EventArgs e)
        {
        }
    }
}