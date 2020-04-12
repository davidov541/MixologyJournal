using Microsoft.WindowsAzure.MobileServices;
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

        MobileServiceClient Client
        {
            get;
        }

        Task<bool> Authenticate();
    }
}
