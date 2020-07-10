using System;
using System.Threading.Tasks;

namespace MixologyJournalApp.Platform
{
    public interface IBackend
    {
        Task<QueryResult> PostResult(String path, Object body);

        Task<QueryResult> DeleteResult(String path, Object body);

        Task<String> GetResult(String path);
    }
}
