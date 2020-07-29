using MixologyJournalApp.View.Controls;
using MixologyJournalApp.ViewModel;
using System;
using Xamarin.Forms;

namespace MixologyJournalApp.View
{
    public class CreateContentPage: ContentPage
    {
        private ICreationInfo _vm;
        private ImageSourceChooser _imageChooser;
        internal CreateContentPage(ICreationInfo viewModel): base()
        {
            _vm = viewModel;
        }

        protected void Init(ImageSourceChooser imageChooser)
        {
            _imageChooser = imageChooser;
        }

        protected void ChangePictureRecognizer_Tapped(object sender, EventArgs e)
        {
            _imageChooser.IsVisible = true;
        }

        internal async void ImageChooser_ImageSourceMade(object sender, ImageSourceChooser.ImageSourceChoiceEventArgs e)
        {
            _imageChooser.IsVisible = false;
            switch (e.Choice)
            {
                case ImageSourceChooser.ImageSourceChoice.ChooseFromGallery:
                    await _vm.ChoosePicture();
                    break;
                case ImageSourceChooser.ImageSourceChoice.TakeAPhoto:
                    await _vm.TakePicture();
                    break;
            }
        }
    }
}
