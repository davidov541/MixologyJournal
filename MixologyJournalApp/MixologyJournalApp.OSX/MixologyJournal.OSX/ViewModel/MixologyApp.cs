using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;           
using AppKit;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel.App;
using MixologyJournalApp.ViewModel;

namespace MixologyJournal.OSX.ViewModel
{
    internal class MixologyApp : BaseMixologyApp
    {
		ResourceLoader _resourceManager = new ResourceLoader("Strings");

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
            /*
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.MediumXga;

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo != null)
            {
                return await photo.OpenStreamForReadAsync();
            }
            */
            await Task.Yield();
            return null;
        }

        protected override async Task<Stream> GetExistingPictureAsync(UInt64 uploadLimit)
        {
            /*
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
            */
            await Task.Yield();
            return null;
        }

        protected async override Task DisplayDialogAsync(String title, String content, String okText)
		{
			await DisplayDialogAsync(title, content, okText);
        }

        protected async override Task<bool> DisplayDialogAsync(String title, String content, String yesText, String noText)
        {
            return await DisplayDialogAsync(title, content, yesText, noText);
        }

        private async Task<bool> DisplayDialogAsync(String title, String content, params String[] buttonTexts)
        {
			NSAlert alertBox = new NSAlert();
			alertBox.MessageText = title;
			alertBox.InformativeText = content;
            foreach(String buttonText in buttonTexts)
            {
                alertBox.AddButton(buttonText);
            }
			alertBox.AlertStyle = NSAlertStyle.Informational;
            nint result = -1;
            Semaphore resultSemaphore = new Semaphore(0, 1);
            alertBox.InvokeOnMainThread(() => {
                result = alertBox.RunModal();
                resultSemaphore.Release();
            });

            resultSemaphore.WaitOne();
            await Task.Yield();
            // It seems like the first button is 1000, and then each one is one more.
            // I don't know how to use the official OK/Cancel button schemes, but this works at least.
            return result == 1000;
		}

        protected override String GetLocalizedString(String key)
        {
            return _resourceManager.GetString(key);
        }

    }
}
