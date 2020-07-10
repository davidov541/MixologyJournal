using MixologyJournalApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MixologyJournalApp.Platform
{
    public interface IPlatform
    {
        IAlertDialogFactory AlertDialogFactory
        {
            get;
        }

        IBackend Backend
        {
            get;
        }

        AuthenticationManager Authentication
        {
            get;
        }
    }
}
