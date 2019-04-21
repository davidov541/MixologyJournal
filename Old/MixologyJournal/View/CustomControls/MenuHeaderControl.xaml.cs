using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MixologyJournal.View
{
    public sealed partial class MenuHeaderControl : UserControl
    {
        public String Header
        {
            get { return (String)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static DependencyProperty HeaderProperty = 
            DependencyProperty.Register("Header", typeof(String), typeof(MenuHeaderControl), new PropertyMetadata(String.Empty));

        public MenuHeaderControl()
        {
            this.InitializeComponent();
        }
    }
}
