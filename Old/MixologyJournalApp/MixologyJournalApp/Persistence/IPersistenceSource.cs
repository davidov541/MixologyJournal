using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using MixologyJournal.ViewModel;

namespace MixologyJournal.Persistence
{
    public interface IPersistenceSource
    {
        String Name
        {
            get;
        }

        bool IsLocal
        {
            get;
        }

        //TODO: Figure out what to do here. 
        // None of the implementations use Journal, and I'd very much like
        // to get rid of that interface, but it seems like it really should
        // be necessary to have the journal in order to save it...
        Task SaveJournalAsync(XDocument doc, IJournalViewModel journal);

        Task SaveFileAsync(Stream stream, String identifier);

        Task<XDocument> LoadJournalAsync();

        Task<String> LoadFileAsync(String identifier);

        Task DeleteFileAsync(String identifier);

        Task<String> GetUniqueFileNameAsync(String baseName);

        Task ClearCacheAsync();
    }
}
