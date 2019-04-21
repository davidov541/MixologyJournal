using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using MixologyJournal.Droid.Persistence;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel.App;
using MixologyJournalApp.Droid;
using MixologyJournalApp.ViewModel;

namespace MixologyJournal.Droid.ViewModel
{
    internal class MixologyApp : BaseMixologyApp
    {
        ResourceLoader _resourceManager;

        private AppPersister _persister;
        private Context _context;
        private MixologyApplication _activity;

        protected override BaseAppPersister Persister
        {
            get
            {
                return _persister;
            }
        }

        public MixologyApp(Context context, MixologyApplication activity)
        {
            Stream stringsFile = context.Assets.Open(CultureInfo.CurrentCulture + ".xml");
            _resourceManager = new ResourceLoader(stringsFile);
            _context = context;
            _activity = activity;
            _persister = new AppPersister(this, context);
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

        private Semaphore _dialogDisplay = new Semaphore(1, 1);
        private Semaphore _dialogResult = new Semaphore(0, 1);

        private async Task<bool> DisplayDialogAsync(String title, String content, params String[] buttonTexts)
        {
            AlertDialog.Builder alertBox = new AlertDialog.Builder(_activity);
            alertBox.SetTitle(title);
            alertBox.SetMessage(content);
            int result = -1;
            EventHandler<DialogClickEventArgs> choiceHandler = (sender, e) =>
            {
                result = e.Which;
                _dialogResult.Release();
            };
            switch(buttonTexts.Length) {
                case 1:
                    alertBox.SetPositiveButton(buttonTexts[0], choiceHandler);
                    break;
                case 2:
                    alertBox.SetPositiveButton(buttonTexts[0], choiceHandler);
                    alertBox.SetNegativeButton(buttonTexts[1], choiceHandler);
                    break;
                case 3:
                    alertBox.SetPositiveButton(buttonTexts[0], choiceHandler);
                    alertBox.SetNeutralButton(buttonTexts[1], choiceHandler);
                    alertBox.SetNegativeButton(buttonTexts[2], choiceHandler);
                    break;
                default:
                    throw new NotSupportedException("Four or more buttons is not supported on Android!");
            }
            // alertBox.AlertStyle = NSAlertStyle.Informational;
            _dialogDisplay.WaitOne();
            Dialog dialog = alertBox.Create();
            _activity.CurrentActivity.RunOnUiThread(() => dialog.Show());
            await Task.Run(() => _dialogResult.WaitOne());
            _dialogDisplay.Release();
            // It seems like the first button is 1000, and then each one is one more.
            // I don't know how to use the official OK/Cancel button schemes, but this works at least.
            return result == -1;
        }

        protected override String GetLocalizedString(String key)
        {
            return _resourceManager.GetString(key);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
