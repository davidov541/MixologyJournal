using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using MixologyJournal.ViewModel;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace MixologyJournal.Persistence
{
    internal class LocalPersistenceSource : IPersistenceSource
    {
        private const String SettingsFileName = "MixologyJournalSettings.xml";
        private const String AppFolderName = "MixologyJournal";

        public String Name
        {
            get
            {
                ResourceLoader resourceLoader = new ResourceLoader();
                return resourceLoader.GetString("LocalSourceName");
            }
        }

        public bool IsLocal
        {
            get
            {
                return true;
            }
        }

        public async Task SaveJournalAsync(XDocument doc, IJournalViewModel journal)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            StorageFile settingsFile = await appFolder.CreateFileAsync(SettingsFileName, CreationCollisionOption.ReplaceExisting);
            using (Stream stream = await settingsFile.OpenStreamForWriteAsync())
            {
                doc.Save(stream);
            }
        }

        public async Task SaveFileAsync(Stream stream, String identifier)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            StorageFile file = await appFolder.CreateFileAsync(identifier, CreationCollisionOption.OpenIfExists);
            using (Stream writeToStream = await file.OpenStreamForWriteAsync())
            {
                await stream.CopyToAsync(writeToStream);
            }
        }

        public async Task<XDocument> LoadJournalAsync()
        {
            XDocument doc;
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
                StorageFile settingsFile = await appFolder.GetFileAsync(SettingsFileName);
                using (Stream stream = await settingsFile.OpenStreamForReadAsync())
                {
                    doc = XDocument.Load(stream);
                }
            }
            catch (FileNotFoundException)
            {
                // If the file just plain isn't there, create a new journal.
                return null;
            }
            catch (InvalidOperationException)
            {
                // This is thrown by XDocument.Load when the file is empty.
                return null;
            }
            catch (XmlException)
            {
                // If the journal is corrupt, just start a new journal.
                return null;
            }
            return doc;
        }

        public async Task<String> LoadFileAsync(String identifier)
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
                StorageFile file = await appFolder.GetFileAsync(Path.GetFileName(identifier));
                if (file == null)
                {
                    return String.Empty;
                }
                return file.Path;
            }
            catch (FileNotFoundException)
            {
                return String.Empty;
            }
        }

        public async Task DeleteFileAsync(String identifier)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            StorageFile file = await appFolder.GetFileAsync(Path.GetFileName(identifier));
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }

        public async Task<String> GetUniqueFileNameAsync(String baseName)
        {
            String name = baseName;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            if (await appFolder.TryGetItemAsync(baseName) != null)
            {
                int i = 0;
                bool unique = false;
                while (!unique)
                {
                    i++;
                    name = Path.GetFileNameWithoutExtension(baseName) + i.ToString() + Path.GetExtension(baseName);
                    unique = await appFolder.TryGetItemAsync(name) == null;
                }
            }
            return name;
        }

        public async Task ClearCacheAsync()
        {
            // No cache to clear.
            await Task.CompletedTask;
        }
    }
}
