using System;
using System.IO;
using System.Xml.Linq;
using MixologyJournal.SourceModel.Entry;

namespace MixologyJournal.Persistence
{
    internal class JournalEntryPersister
    {
        private const String PersistedTitle = "Title";
        private const String PersistedContents = "Contents";
        private const String PersistedCreationDate = "CreationDate";
        private const String PersistedPicture = "Picture";
        private const String PersistedPath = "Path";

        protected void PopulateXmlElement(XElement entryElem, JournalEntry elementToWrite, XContainer parentNode)
        {
            entryElem.SetAttributeValue(PersistedTitle, elementToWrite.Title);
            entryElem.SetAttributeValue(PersistedContents, elementToWrite.Notes);
            entryElem.SetAttributeValue(PersistedCreationDate, elementToWrite.CreationDate.ToBinary().ToString());
            foreach (String picPath in elementToWrite.Pictures)
            {
                XElement picElem = new XElement(PersistedPicture);
                picElem.SetAttributeValue(PersistedPath, picPath);
                entryElem.Add(picElem);
            }
        }

        protected bool TryPopulateInstance(JournalEntry entry, XElement element)
        {
            long longDate;
            if (element.Attribute(PersistedTitle) != null &&
                element.Attribute(PersistedContents) != null &&
                element.Attribute(PersistedCreationDate) != null &&
                long.TryParse(element.Attribute(PersistedCreationDate).Value, out longDate))
            {
                entry.Title = element.Attribute(PersistedTitle).Value;
                entry.Notes = element.Attribute(PersistedContents).Value;
                entry.CreationDate = DateTime.FromBinary(longDate);
                foreach (XElement pic in element.Elements())
                {
                    if (pic.Name.LocalName.Equals(PersistedPicture) &&
                        pic.Attribute(PersistedPath) != null)
                    {
                        entry.AddPicture(Path.GetFileName(pic.Attribute(PersistedPath).Value));
                    }
                }
                return true;
            }
            return false;
        }
    }
}
