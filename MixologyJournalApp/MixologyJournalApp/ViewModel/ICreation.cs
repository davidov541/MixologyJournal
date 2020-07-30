using System;
using System.Windows.Input;

namespace MixologyJournalApp.ViewModel
{
    internal interface ICreation: ICreationInfo
    {
        ICommand DeleteCommand
        {
            get;
        }

        Boolean CanBeDeleted
        {
            get;
        }

        ICommand ToggleFavoriteCommand
        {
            get;
        }

        Boolean CanBeFavorited
        {
            get;
        }

        Boolean CanBeUnfavorited
        {
            get;
        }
    }
}
