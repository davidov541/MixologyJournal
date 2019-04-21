using System;

namespace MixologyJournal.ViewModel.List
{
    internal class LetterHeader : IListEntryHeader
    {
        private String _header;

        public String Caption
        {
            get
            {
                return String.Empty;
            }
        }

        public String Title
        {
            get
            {
                return _header.ToUpper();
            }
        }

        public Symbol? Icon
        {
            get
            {
                return null;
            }
        }

        public LetterHeader(String header)
        {
            _header = header;
        }
    }
}
