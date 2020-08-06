using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MixologyJournalApp.Platform
{
    public interface ILoginMethod : INotifyPropertyChanged
    {
        String Name
        {
            get;
        }

        String Icon
        {
            get;
        }

        String BackgroundColor
        {
            get;
        }

        String ForegroundColor
        {
            get;
        }

        Boolean IsEnabled
        {
            get;
        }

        ICommand LoginCommand
        {
            get;
        }

        ICommand LogoffCommand
        {
            get;
        }

        User CurrentUser
        {
            get;
        }

        event EventHandler LoginEnabled;
    }
}
