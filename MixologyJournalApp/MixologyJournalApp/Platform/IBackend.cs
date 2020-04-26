using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;

namespace MixologyJournalApp.Platform
{
    public interface IBackend
    {
        bool IsAuthenticated
        {
            get;
        }

        MobileServiceUser User
        {
            get;
        }

        Task Init();

        Task<bool> Authenticate();

        Task<bool> PostResult(String path, Object body);

        Task<String> GetResult(String path);

        Task LogOffAsync();
    }
}
