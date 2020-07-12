using System;
using System.Threading.Tasks;

namespace MixologyJournalApp.Platform
{
    public interface IRemoteLoginMethod : ILoginMethod
    {
        Boolean IsLoggedIn
        {
            get;
        }

        event EventHandler LoggingOff;

        Task Init(bool setupMode);
    }
}
