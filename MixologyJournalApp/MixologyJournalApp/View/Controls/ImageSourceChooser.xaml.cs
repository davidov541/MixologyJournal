using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixologyJournalApp.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageSourceChooser : ContentView
    {
        internal enum ImageSourceChoice
        {
            TakeAPhoto,
            ChooseFromGallery,
            NoChoice
        }

        internal class ImageSourceChoiceEventArgs: EventArgs
        {
            public ImageSourceChoice Choice
            {
                get;
                private set;
            }

            internal ImageSourceChoiceEventArgs(ImageSourceChoice choice)
            {
                Choice = choice;
            }
        }

        internal event EventHandler<ImageSourceChoiceEventArgs> ImageSourceMade;

        public ImageSourceChooser()
        {
            InitializeComponent();
        }

        private void TakeNewPhoto_Tapped(object sender, EventArgs e)
        {
            ImageSourceMade?.Invoke(this, new ImageSourceChoiceEventArgs(ImageSourceChoice.TakeAPhoto));
        }

        private void ChooseFromGallery_Tapped(object sender, EventArgs e)
        {
            ImageSourceMade?.Invoke(this, new ImageSourceChoiceEventArgs(ImageSourceChoice.ChooseFromGallery));
        }

        private void CancelGesture_Tapped(object sender, EventArgs e)
        {
            ImageSourceMade?.Invoke(this, new ImageSourceChoiceEventArgs(ImageSourceChoice.NoChoice));
        }
    }
}