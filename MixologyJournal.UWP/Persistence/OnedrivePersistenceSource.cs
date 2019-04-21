using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.OneDrive.Sdk;
using MixologyJournal.ViewModel;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace MixologyJournal.Persistence
{
    internal class OneDrivePersistenceSource : IPersistenceSource
    {
        private const String SettingsFileName = "MixologyJournalSettings.xml";
        private const String AppFolderName = "MixologyJournal";
        private const String CacheFolderName = "Cache";

        public String Name
        {
            get
            {
                ResourceLoader resourceLoader = new ResourceLoader();
                return resourceLoader.GetString("OneDriveSourceName");
            }
        }

        public bool IsLocal
        {
            get
            {
                return false;
            }
        }

        public async Task SaveJournalAsync(XDocument doc, IJournalViewModel journal)
        {
            IItemRequestBuilder odAppFolder = await GetOneDriveFolder();

            // Make memory stream of the document.
            MemoryStream memStream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(memStream);
            doc.Save(writer);
            writer.Dispose();
            memStream.Position = 0;

            IItemRequestBuilder odSettingsFile = odAppFolder.ItemWithPath(SettingsFileName);
            await odAppFolder.ItemWithPath(SettingsFileName).Content.Request().PutAsync<Item>(memStream);
        }

        public async Task SaveFileAsync(Stream stream, String identifier)
        {
            IItemRequestBuilder odAppFolder = await GetOneDriveFolder();
            await odAppFolder.ItemWithPath(identifier).Content.Request().PutAsync<Item>(stream);
        }

        public async Task<XDocument> LoadJournalAsync()
        {
            XDocument doc;
            IItemRequestBuilder odAppFolder;
            try
            {
                odAppFolder = await GetOneDriveFolder();
                IItemRequestBuilder settingsFile = odAppFolder.ItemWithPath(SettingsFileName);
                await settingsFile.Request().GetAsync();
                using (Stream stream = await settingsFile.Content.Request().GetAsync())
                {
                    doc = XDocument.Load(stream);
                }
            }
            catch (FileNotFoundException)
            {
                // If we can't access OneDrive for some reason, try the local files.
                return null;
            }
            catch (OneDriveException e)
            {
                if (e.Error.Code.Equals("itemNotFound"))
                {
                    return null;
                }
                throw;
            }
            return doc;
        }

        public async Task<String> LoadFileAsync(String identifier)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            StorageFolder cacheFolder = await appFolder.CreateFolderAsync(CacheFolderName, CreationCollisionOption.OpenIfExists);
            StorageFile cachedFile;
            try
            {
                cachedFile = await cacheFolder.GetFileAsync(identifier);
            }
            catch (FileNotFoundException)
            {
                try
                {
                    IItemRequestBuilder odAppFolder = await GetOneDriveFolder();
                    IItemRequestBuilder odFile = odAppFolder.ItemWithPath(identifier);
                    Item item = await odFile.Request().GetAsync();
                    cachedFile = await cacheFolder.CreateFileAsync(identifier);
                    using (Stream fileStream = await cachedFile.OpenStreamForWriteAsync())
                    {
                        using (Stream s = await odFile.Content.Request().GetAsync())
                        {
                            s.CopyTo(fileStream);
                        }
                    }
                }
                catch (OneDriveException)
                {
                    return null;
                }
            }
            return cachedFile.Path;
        }

        public async Task DeleteFileAsync(String identifier)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            StorageFolder cacheFolder = await appFolder.CreateFolderAsync(CacheFolderName, CreationCollisionOption.OpenIfExists);
            StorageFile cachedFile;
            try
            {
                cachedFile = await cacheFolder.GetFileAsync(identifier);
                await cachedFile.DeleteAsync();
            }
            catch (FileNotFoundException)
            {
                // This just means we never cached the file, which is fine.
            }
            IItemRequestBuilder odAppFolder = await GetOneDriveFolder();
            IItemRequestBuilder odFile = odAppFolder.ItemWithPath(identifier);
            await odFile.Request().DeleteAsync();
        }

        public async Task<String> GetUniqueFileNameAsync(String baseName)
        {
            String name = baseName;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            String[] scopes = new String[]
            {
                        "onedrive.readwrite",
                        "onedrive.appfolder",
                        "wl.signin"
            };
            IOneDriveClient client = OneDriveClientExtensions.GetClientUsingOnlineIdAuthenticator(scopes);
            AccountSession session = await client.AuthenticateAsync();
            IChildrenCollectionPage files = await client.Drive.Special.AppRoot.Children.Request().GetAsync();
            StorageFolder cacheFolder = await appFolder.CreateFolderAsync(CacheFolderName, CreationCollisionOption.OpenIfExists);
            if (files.Any(i => i.Name.Equals(baseName)) || await cacheFolder.TryGetItemAsync(name) != null)
            {
                bool unique = false;
                int i = 0;
                while (!unique)
                {
                    i++;
                    name = Path.GetFileNameWithoutExtension(baseName) + i.ToString() + Path.GetExtension(baseName);
                    unique = !files.Any(item => item.Name.Equals(name)) && await cacheFolder.TryGetItemAsync(name) == null;
                }
            }
            return name;
        }

        private static async Task<IItemRequestBuilder> GetOneDriveFolder()
        {
            String[] scopes = new String[]
            {
                "onedrive.readwrite",
                "onedrive.appfolder",
                "wl.signin"
            };
            IOneDriveClient client = OneDriveClientExtensions.GetClientUsingOnlineIdAuthenticator(scopes);
            AccountSession session = await client.AuthenticateAsync();
            IItemRequestBuilder folder = client.Drive.Special.AppRoot;
            return folder;
        }

        private static async Task<IItemRequestBuilder> GetOneDriveFile(String identifier)
        {
            String[] scopes = new String[]
            {
                "onedrive.readwrite",
                "onedrive.appfolder",
                "wl.signin"
            };
            IOneDriveClient client = OneDriveClientExtensions.GetClientUsingOnlineIdAuthenticator(scopes);
            AccountSession session = await client.AuthenticateAsync();
            IItemRequestBuilder settings = client.Drive.Special.AppRoot.ItemWithPath(identifier);
            return settings;
        }

        public async Task ClearCacheAsync()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder appFolder = await localFolder.CreateFolderAsync(AppFolderName, CreationCollisionOption.OpenIfExists);
            StorageFolder cacheFolder = await appFolder.CreateFolderAsync(CacheFolderName, CreationCollisionOption.OpenIfExists);
            foreach (StorageFile file in await cacheFolder.GetFilesAsync())
            {
                await file.DeleteAsync();
            }
        }
    }
}
