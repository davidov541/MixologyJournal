using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditableImage : ContentView
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create("Image",
            typeof(ImageSource),
            typeof(EditableImage),
            null,
            defaultBindingMode: BindingMode.TwoWay);

        public ImageSource Image
        {
            get
            {
                return GetValue(ImageProperty) as ImageSource;
            }
            set
            {
                SetValue(ImageProperty, value);
            }
        }

        public static readonly BindableProperty UpdateImageCommandProperty = BindableProperty.CreateAttached("UpdateImageCommand",
            typeof(ICommand),
            typeof(EditableImage),
            null,
            defaultBindingMode: BindingMode.OneWay);

        public ICommand UpdateImageCommand
        {
            get
            {
                return GetValue(UpdateImageCommandProperty) as ICommand;
            }
            set
            {
                SetValue(UpdateImageCommandProperty, value);
            }
        }

        public EditableImage()
        {
            InitializeComponent();
        }
    }
}