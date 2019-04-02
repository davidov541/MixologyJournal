using System;
using System.Collections.Generic;

namespace MixologyJournal.SourceModel.Entry
{
    internal abstract class JournalEntry : IJournalEntry
    {
        private List<String> _pictures;
        public virtual String Title
        {
            get;
            set;
        }

        public String Notes
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get;
            set;
        }

        private static int NextID = 0;
        public int ID
        {
            get;
            private set;
        }

        public IEnumerable<String> Pictures
        {
            get
            {
                return _pictures;
            }
        }

        protected JournalEntry(int id, String title, String contents, DateTime creationDate)
        {
            Title = title;
            Notes = contents;
            CreationDate = creationDate;
            _pictures = new List<String>();

            ID = id;
            if (ID >= NextID)
            {
                NextID = ID + 1;
            }
        }

        protected JournalEntry(String title, String contents, DateTime creationDate) :
            this(NextID, title, contents, creationDate)
        {
        }

        public void AddPicture(String picturePath)
        {
            _pictures.Add(picturePath);
        }
    }
}
