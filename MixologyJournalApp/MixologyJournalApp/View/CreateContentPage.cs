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
        internal CreateContentPage(): base()
        {
        }

        internal void Init(ICreationInfo viewModel, ImageSourceChooser imageChooser, TapGestureRecognizer choosePictureGesture)
        {
            _vm = viewModel;

            _imageChooser = imageChooser;
            _imageChooser.ImageSourceMade += ImageChooser_ImageSourceMade;

            choosePictureGesture.Tapped += ChangePictureRecognizer_Tapped;
        }

        private void ChangePictureRecognizer_Tapped(object sender, EventArgs e)
        {
            _imageChooser.IsVisible = true;
        }

        private async void ImageChooser_ImageSourceMade(object sender, ImageSourceChooser.ImageSourceChoiceEventArgs e)
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
