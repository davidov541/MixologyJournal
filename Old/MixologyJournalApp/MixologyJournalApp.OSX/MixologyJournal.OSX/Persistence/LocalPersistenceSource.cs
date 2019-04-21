using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using MixologyJournal.ViewModel;

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
                /*
                ResourceLoader resourceLoader = new ResourceLoader();
                return resourceLoader.GetString("LocalSourceName");
                */
                return String.Empty;
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
            /*
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            StorageFile settingsFile = await appFolder.CreateFileAsync(SettingsFileName, CreationCollisionOption.ReplaceExisting);
            using (Stream stream = await settingsFile.OpenStreamForWriteAsync())
            {
                doc.Save(stream);
            }
            */
            await Task.Yield();
        }

        public async Task SaveFileAsync(Stream stream, String identifier)
        {
            /*
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            StorageFile file = await appFolder.CreateFileAsync(identifier, CreationCollisionOption.OpenIfExists);
            using (Stream writeToStream = await file.OpenStreamForWriteAsync())
            {
                await stream.CopyToAsync(writeToStream);
            }
            */
            await Task.Yield();
        }

        public async Task<XDocument> LoadJournalAsync()
        {
            await Task.Yield();
            /*
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
            */
            return null;
        }

        public async Task<String> LoadFileAsync(String identifier)
        {
            await Task.Yield();
            /*
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
            */
            return String.Empty;
        }

        public async Task DeleteFileAsync(String identifier)
        {
            await Task.Yield();
            /*
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            StorageFile file = await appFolder.GetFileAsync(Path.GetFileName(identifier));
            if (file != null)
            {
                await file.DeleteAsync();
            }
            */
        }

        public async Task<String> GetUniqueFileNameAsync(String baseName)
        {
            await Task.Yield();
            /*
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
            */
            return String.Empty;
        }

        public async Task ClearCacheAsync()
        {
            // No cache to clear.
            await Task.CompletedTask;
        }
    }
}
