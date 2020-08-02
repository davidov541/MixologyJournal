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
        private readonly IPictureCreation _context;
        private readonly ICommand _selectionCommand;

        internal DetailCardView(IPictureCreation creation, ICommand selectionCommand)
        {
            _selectionCommand = selectionCommand;
            _context = creation;
            BindingContext = _context;
            InitializeComponent();
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