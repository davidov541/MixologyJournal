using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;

namespace MixologyJournalApp.Model
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

        Task<String> GetResult(String path);

        Task<bool> Authenticate();

        Task LogOffAsync();
    }
}
