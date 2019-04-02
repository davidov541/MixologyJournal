using System;
using System.Xml.Linq;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.Persistence
{
    internal class BarDrinkPersister : BaseRecipePersister, ISettingsPersister<BarDrink>
    {
        private const String PersistedName = "BarDrink";
        private const String PersistedId = "ID";

        public Boolean TryCreate(XElement element, Journal journal, out BarDrink createdElement)
        {
            createdElement = null;
            if (element.Name.LocalName.Equals(PersistedName))
            {
                int id;
                if (element.Attribute(PersistedId) != null &&
                    int.TryParse(element.Attribute(PersistedId).Value, out id))
                {
                    createdElement = new BarDrink(id);
                }
                else
                {
                    return false;
                }
                return TryPopulateInstance(createdElement, element);
            }
            return false;
        }

        public void Write(BarDrink elementToPersist, XContainer parentNode)
        {
            XElement entryElem = new XElement(PersistedName);
            PopulateXmlElement(entryElem, elementToPersist, parentNode);
        }
    }
}
