using System;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Entry
{
    public interface INotesEntryViewModel : IJournalEntryViewModel
    {
    }

    internal class NotesEntryViewModel : JournalEntryViewModel, INotesEntryViewModel
    {
        private BaseMixologyApp _app;

        private NoteEntry Entry
        {
            get
            {
                return Model as NoteEntry;
            }
        }

        public String Header
        {
            get
            {
                return Title;
            }
        }

        public NotesEntryViewModel(BaseMixologyApp app)
            : this(new NoteEntry(), app)
        {
        }

        public NotesEntryViewModel(NoteEntry entry, BaseMixologyApp app) :
            base(entry, app)
        {
            _app = app;
        }
    }
}
