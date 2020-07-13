using MixologyJournalApp.ViewModel;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailCardView : Frame
    {
        private readonly ICreationInfo _context;
        private readonly ICommand _selectionCommand;

        internal DetailCardView(ICreationInfo recipe, ICommand selectionCommand)
        {
            _selectionCommand = selectionCommand;
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
            if (_selectionCommand != null)
            {
                _selectionCommand.Execute(_context);
            }
        }
    }
}