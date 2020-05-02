using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MixologyJournalApp.Platform
{
    public interface IBackend : INotifyPropertyChanged
    {
        Boolean HasBeenSetup
        {
            get;
        }

        Boolean IsAuthenticated
        {
            get;
        }

        IEnumerable<ILoginMethod> LoginMethods
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
