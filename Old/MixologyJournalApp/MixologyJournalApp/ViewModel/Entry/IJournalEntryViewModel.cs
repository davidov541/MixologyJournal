using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Entry
{
    public interface IJournalEntryViewModel : ISaveablePageViewModel, INotifyPropertyChanged
    {
        BaseMixologyApp App
        {
            get;
        }

        DateTime CreationDate
        {
            get;
        }

        ObservableCollection<String> Pictures
        {
            get;
        }

        String Notes
        {
            get;
            set;
        }

        Task AddNewPictureAsync();

        Task AddExistingPictureAsync();
    }
}
