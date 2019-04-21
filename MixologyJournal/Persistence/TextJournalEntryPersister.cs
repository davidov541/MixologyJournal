using System;
using System.Xml.Linq;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;

namespace MixologyJournal.Persistence
{
    internal class TextJournalEntryPersister : JournalEntryPersister, ISettingsPersister<NoteEntry>
    {
        private const String PersistedName = "TextJournalEntry";

        public void Write(NoteEntry elementToWrite, XContainer parentNode)
        {
            XElement entryElem = new XElement(PersistedName);
            PopulateXmlElement(entryElem, elementToWrite, parentNode);
            parentNode.Add(entryElem);
        }

        public Boolean TryCreate(XElement element, Journal journal, out NoteEntry createdElement)
        {
            createdElement = null;
            if (element.Name.LocalName.Equals(PersistedName))
            {
                createdElement = new NoteEntry();
                return TryPopulateInstance(createdElement, element);
            }
            return false;
        }
    }
}
