using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel.App;
using Windows.ApplicationModel.Resources;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace MixologyJournal.ViewModel
{
    internal class MixologyApp : BaseMixologyApp
    {
        private AppPersister _persister;
        protected override BaseAppPersister Persister
        {
            get
            {
                return _persister;
            }
        }

        public MixologyApp()
        {
            _persister = new AppPersister(this);
        }

        protected override async Task<Stream> GetNewPictureAsync()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.MediumXga;

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo != null)
            {
                return await photo.OpenStreamForReadAsync();
            }
            return null;
        }

        protected override async Task<Stream> GetExistingPictureAsync(UInt64 uploadLimit)
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            StorageFile photo = await openPicker.PickSingleFileAsync();
            if (photo != null)
            {
                BasicProperties attr = await photo.GetBasicPropertiesAsync();
                if (attr.Size > uploadLimit)
                {
                    ResourceLoader resourceLoader = new ResourceLoader();
                    if (await DisplayDialogAsync(
                        resourceLoader.GetString("TooLargeDialogHeader"),
                        resourceLoader.GetString("TooLargeDialogDescription"),
                        resourceLoader.GetString("PickAnotherFileOption"),
                        resourceLoader.GetString("CancelOption")
                    ))
                    {
                        await GetExistingPictureAsync(uploadLimit);
                    }
                    return null;
                }

                return await photo.OpenStreamForReadAsync();
            }
            return null;
        }

        protected async override Task DisplayDialogAsync(String title, String content, String yesText)
        {
            ContentDialog errorDialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = yesText,
                IsSecondaryButtonEnabled = false
            };

            await errorDialog.ShowAsync();
        }

		protected async override Task<bool> DisplayDialogAsync(String title, String content, String yesText, String noText)
        {
            ContentDialog errorDialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = yesText,
                SecondaryButtonText = noText
            };

            ContentDialogResult result = await errorDialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

		protected override String GetLocalizedString(String key)
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            return resourceLoader.GetString(key);
        }
    }
}
