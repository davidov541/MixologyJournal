using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;

namespace MixologyJournalApp.Security
{
    public interface IAuthenticate
    {
        bool IsAuthenticated
        {
            get;
        }

        MobileServiceUser User
        {
            get;
        }

        Task<bool> Authenticate();
    }
}
