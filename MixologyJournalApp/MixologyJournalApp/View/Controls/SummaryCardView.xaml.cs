using MixologyJournalApp.ViewModel;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SummaryCardView : Frame
    {
        private readonly IPictureCreationInfo _context;
        private readonly ICommand _selectionCommand;

        internal SummaryCardView(IPictureCreationInfo recipe, ICommand selectionCommand)
        {
            _selectionCommand = selectionCommand;
            _context = recipe;
            BindingContext = _context;
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            imgCard.HeightRequest = imgCard.Width / 16 * 9;
        }

        private void Card_Tapped(object sender, EventArgs e)
        {
            _selectionCommand.Execute(_context);
        }
    }
}