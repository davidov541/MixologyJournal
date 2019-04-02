using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Preferences;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;

namespace MixologyJournal.Droid.Persistence
{
    internal class AppPersister : BaseAppPersister
    {
        private ISharedPreferences _preferences;

        public override IPersistenceSource Source
        {
            get
            {
                String sourceStr = _preferences.GetString("Source", LocalPersistenceSource.LocalSourceName);
                IPersistenceSource source = Sources.FirstOrDefault(s => s.Name.Equals(sourceStr));
                if (source == null)
                {
                    // Backwards compatibility fix for when we used to store enum values.
                    switch (sourceStr)
                    {
                        case "1":
                        case "0":
                        default:
                            source = Sources.OfType<LocalPersistenceSource>().First();
                            break;
                    }
                }
                return source;
            }
            set
            {
                _preferences.Edit().PutString("Source", value.Name).Commit();
            }
        }

        public AppPersister(BaseMixologyApp app, Context context) :
            base(app)
        {
            _preferences = PreferenceManager.GetDefaultSharedPreferences(context);
            AddSource(new LocalPersistenceSource(context));
            // AddSource(new OneDrivePersistenceSource());
        }

        #region Public Utilities
        protected override async Task PortPictureToNewSourceAsync(String picture, IPersistenceSource oldSource)
        {
            String picIdentifier = Path.GetFileName(picture);
            String fullPath = await LoadFileFromSourceAsync(picIdentifier, oldSource);
            // Check that we actually found the file. Otherwise, we've gotten corrupted somehow.
            if (!String.IsNullOrEmpty(fullPath))
            {
                using (FileStream s = new FileStream(fullPath, FileMode.Open))
                {
                    await SaveFileAsync(s, picIdentifier, true);
                }
            }
        }
        #endregion
    }
}
