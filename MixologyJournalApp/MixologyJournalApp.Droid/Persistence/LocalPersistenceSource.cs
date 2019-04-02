using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Android.Content;
using MixologyJournal.Persistence;
using MixologyJournal.ViewModel;
using File = Java.IO.File;
using JavaFileNotFoundException = Java.IO.FileNotFoundException;

namespace MixologyJournal.Droid.Persistence
{
    internal class LocalPersistenceSource : IPersistenceSource
    {
        private const String SettingsFileName = "MixologyJournalSettings.xml";
        private const String AppFolderName = "MixologyJournal";
        private Context _context;
        public const String LocalSourceName = "Local";

        public String Name
        {
            get
            {
                return LocalSourceName;
            }
        }

        public bool IsLocal
        {
            get
            {
                return true;
            }
        }

        public LocalPersistenceSource(Context context)
        {
            _context = context;
        }

        public async Task SaveJournalAsync(XDocument doc, IJournalViewModel journal)
        {
            using (Stream stream = _context.OpenFileOutput(SettingsFileName, FileCreationMode.Private))
            {
                doc.Save(stream);
            }
            await Task.Yield();
        }

        public async Task SaveFileAsync(Stream stream, String identifier)
        {
            using (Stream writeToStream = _context.OpenFileOutput(identifier, FileCreationMode.Private))
            {
                await stream.CopyToAsync(writeToStream);
            }
        }

        public async Task<XDocument> LoadJournalAsync()
        {
            XDocument doc;
            try
            {
                using (Stream stream = _context.OpenFileInput(SettingsFileName))
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
            catch (JavaFileNotFoundException)
            {
                // On Android, the Java version of FileNotFound is thrown, so catch that as well.
                return null;
            }
            await Task.Yield();
            return doc;
        }

        public async Task<String> LoadFileAsync(String identifier)
        {
            await Task.Yield();
            try
            {
                File file = _context.GetFileStreamPath(identifier);
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
            await Task.Yield();
            _context.DeleteFile(identifier);
        }

        public async Task<String> GetUniqueFileNameAsync(String baseName)
        {
            await Task.Yield();
            String name = baseName;
            File currFile = _context.GetFileStreamPath(name);
            while (currFile == null || !currFile.Exists())
            {
                int i = 0;
                bool unique = false;
                while (!unique)
                {
                    i++;
                    name = Path.GetFileNameWithoutExtension(baseName) + i.ToString() + Path.GetExtension(baseName);
                    currFile = _context.GetFileStreamPath(name);
                    unique = currFile != null && currFile.Exists();
                }
            }
            return String.Empty;
        }

        public async Task ClearCacheAsync()
        {
            // No cache to clear.
            await Task.CompletedTask;
        }
    }
}
