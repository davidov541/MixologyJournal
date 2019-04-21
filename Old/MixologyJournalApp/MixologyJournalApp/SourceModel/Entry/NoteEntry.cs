using System;

namespace MixologyJournal.SourceModel.Entry
{
    internal class NoteEntry : JournalEntry
    {
        public NoteEntry() :
            this(String.Empty, String.Empty, DateTime.Now)
        {
        }

        public NoteEntry(String title, String contents, DateTime creationDate) :
            base(title, contents, creationDate)
        {
        }
    }
}
