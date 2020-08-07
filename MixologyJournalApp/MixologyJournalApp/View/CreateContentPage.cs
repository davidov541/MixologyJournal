using MixologyJournalApp.View.Controls;
using MixologyJournalApp.ViewModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MixologyJournalApp.View
{
    public class CreateContentPage : ContentPage
    {
        public ICommand ChangeImageCommand
        {
            get
            {
                return new Command(() => _imageChooser.IsVisible = true);
            }

        }

        private IPictureCreation _vm;
        private ImageSourceChooser _imageChooser;

        internal void Init(IPictureCreation viewModel, ImageSourceChooser imageChooser)
        {
            _vm = viewModel;

            _imageChooser = imageChooser;
            _imageChooser.ImageSourceMade += ImageChooser_ImageSourceMade;
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
                case ImageSourceChooser.ImageSourceChoice.NoChoice:
                    break;
            }
        }
    }
}
