using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MixologyJournal.SourceModel;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;

namespace MixologyJournal.Persistence
{
    public abstract class BaseAppPersister
    {
        private JournalPersister _journalPersister;
        private BaseMixologyApp _app;

        protected BaseAppPersister(BaseMixologyApp app)
        {
            _journalPersister = new JournalPersister();
            _sources = new List<IPersistenceSource>();
            _app = app;
        }

        private void StartWrite(XDocument doc, JournalViewModel journal)
        {
            _journalPersister.Write((journal as JournalViewModel).Model, doc);
        }

        private JournalViewModel GetEmptyJournal()
        {
            Assembly asm = typeof(BaseAppPersister).GetTypeInfo().Assembly;
            Stream s = asm.GetManifestResourceStream("MixologyJournalApp.RecipePacks.en_US.DefaultRecipes.xml");
            XDocument doc = XDocument.Load(s);
            return LoadJournal(doc);
        }

        private JournalViewModel LoadJournal(XDocument doc)
        {
            Journal journal;
            if (!_journalPersister.TryCreate(doc.Elements().Last(), null, out journal))
            {
                throw new NotImplementedException("Something Went Wrong!");
            }
            return new JournalViewModel(journal, _app);
        }

        private List<IPersistenceSource> _sources;
        private Mutex _processing = new Mutex(false);

        public abstract IPersistenceSource Source
        {
            get;
            set;
        }

        public IEnumerable<IPersistenceSource> Sources
        {
            get
            {
                return _sources;
            }
        }

        #region Save
        internal async Task SaveAsync(JournalViewModel journal)
        {
            try
            {
                _processing.WaitOne();
                XDocument doc = new XDocument();
                StartWrite(doc, journal);
                await Source.SaveJournalAsync(doc, journal);
            }
            finally
            {
                _processing.ReleaseMutex();
            }
        }

        public async Task<String> SaveFileAsync(Stream stream, String identifier, bool overwrite)
        {
            try
            {
                _processing.WaitOne();
                if (!overwrite)
                {
                    identifier = await GetUniqueFileName(identifier);
                }
                await Source.SaveFileAsync(stream, identifier);
            }
            finally
            {
                _processing.ReleaseMutex();
            }
            return identifier;
        }
        #endregion

        #region Load
        internal async Task<JournalViewModel> LoadAsync()
        {
            return await LoadFromSourceAsync(Source);
        }

        internal async Task<JournalViewModel> LoadFromSourceAsync(IPersistenceSource source)
        {
            try
            {
                _processing.WaitOne();
                XDocument doc = await source.LoadJournalAsync();
                if (doc == null)
                {
                    return GetEmptyJournal();
                }
                Source = source;
                return LoadJournal(doc);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                _processing.ReleaseMutex();
            }
        }

        public async Task<String> LoadFileAsync(String identifier)
        {
            return await LoadFileFromSourceAsync(identifier, Source);
        }

        protected async Task<String> LoadFileFromSourceAsync(String identifier, IPersistenceSource source)
        {
            try
            {
                _processing.WaitOne();
                return await Source.LoadFileAsync(identifier);
            }
            finally
            {
                _processing.ReleaseMutex();
            }
        }

        internal async Task PortJournalToNewSourceAsync(IPersistenceSource oldSource)
        {
            foreach (IJournalEntryViewModel recipe in _app.Journal.Entries)
            {
                foreach (String picture in recipe.Pictures)
                {
                    await PortPictureToNewSourceAsync(picture, oldSource);
                }
            }
        }

        protected abstract Task PortPictureToNewSourceAsync(String picture, IPersistenceSource oldSource);
        #endregion

        #region Public Misc
        public async Task ClearCacheAsync()
        {
            try
            {
                _processing.WaitOne();
                await Source.ClearCacheAsync();
            }
            finally
            {
                _processing.ReleaseMutex();
            }
        }

        public async Task DeleteFileAsync(String identifier)
        {
            try
            {
                _processing.WaitOne();
                await Source.DeleteFileAsync(identifier);
            }
            finally
            {
                _processing.ReleaseMutex();
            }
        }

        public async Task<String> SaveNewPictureAsync()
        {
            Stream s = await _app.GetNewPictureAsync();
            if (s != null)
            {
                using (s)
                {
                    return await SaveFileAsync(s, "CapturedPhoto", false);
                }
            }
            return null;
        }

        public async Task<String> SaveExistingPictureAsync()
        {
            Stream s = await _app.GetExistingPictureAsync(Source.IsLocal ? UInt64.MaxValue : 1000000);
            if (s != null)
            {
                using (s)
                {
                    return await SaveFileAsync(s, "CapturedPhoto", false);
                }
            }
            return null;
        }

        #endregion

        #region Utilities
        private async Task<String> GetUniqueFileName(String identifier)
        {
            return await Source.GetUniqueFileNameAsync(identifier);
        }

        protected void AddSource(IPersistenceSource persister)
        {
            _sources.Add(persister);
        }
        #endregion
    }
}
