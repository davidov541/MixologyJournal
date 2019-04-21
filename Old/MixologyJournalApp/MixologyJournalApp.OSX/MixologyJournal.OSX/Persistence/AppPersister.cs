using Foundation;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;

namespace MixologyJournal.Persistence
{
    internal class AppPersister : BaseAppPersister
    {
        private NSUserDefaults _defaults;
        public override IPersistenceSource Source
        {
            get
            {
                IPersistenceSource source = Sources.FirstOrDefault(s => s.IsLocal);
                String sourceStr = _defaults.StringForKey("Source");
                if (sourceStr != null)
                {
                    source = Sources.FirstOrDefault(s => s.Name.Equals(sourceStr));
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
                }
                return source;
            }
            set
            {
                _defaults.SetString(value.Name, "Source");
            }
        }

        public AppPersister(BaseMixologyApp app) :
            base(app)
        {
            _defaults = NSUserDefaults.StandardUserDefaults;
            AddSource(new LocalPersistenceSource());
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
