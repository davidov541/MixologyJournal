using System;
using System.Linq;
using System.Xml.Linq;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.Persistence
{
    internal class RecipePersister : BaseRecipePersister, ISettingsPersister<BaseRecipe>
    {
        private const String PersistedName = "BaseRecipe";
        private const String PersistedDerived = "DerivedRecipe";
        private const String PersistedId = "ID";
        private const String PersistedFavorite = "FavoriteID";

        public void Write(BaseRecipe elementToWrite, XContainer parentNode)
        {
            XElement entryElem = new XElement(PersistedName);
            PopulateXmlElement(entryElem, elementToWrite, parentNode);
            if (elementToWrite.FavoriteRecipe != null)
            {
                entryElem.SetAttributeValue(PersistedFavorite, elementToWrite.FavoriteRecipe.ID.ToString());
            }
            foreach (HomemadeDrink modified in elementToWrite.DerivedRecipes)
            {
                XElement derivedElem = new XElement(PersistedDerived);
                PopulateXmlElement(derivedElem, modified, entryElem);
            }
        }

        public Boolean TryCreate(XElement element, Journal journal, out BaseRecipe createdElement)
        {
            createdElement = null;
            if (element.Name.LocalName.Equals(PersistedName))
            {
                int id;
                if (element.Attribute(PersistedId) != null &&
                    int.TryParse(element.Attribute(PersistedId).Value, out id))
                {
                    createdElement = new BaseRecipe(id);
                }
                else
                {
                    createdElement = new BaseRecipe();
                }
                if (TryPopulateInstance(createdElement, element))
                {
                    foreach (XElement elem in element.Elements())
                    {
                        HomemadeDrink modified;
                        if (elem.Name.LocalName.Equals(PersistedDerived) &&
                            TryCreateModifiedRecipe(elem, createdElement, out modified))
                        {
                            // Creating the modified version automatically adds itself to the original, 
                            // so nothing we need to do here.
                        }
                    }
                    if (element.Attribute(PersistedFavorite) != null &&
                        int.TryParse(element.Attribute(PersistedFavorite).Value, out id) &&
                        createdElement.DerivedRecipes.Any(e => e.ID == id))
                    {
                        createdElement.FavoriteRecipe = createdElement.DerivedRecipes.FirstOrDefault(e => e.ID == id);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool TryCreateModifiedRecipe(XElement element, BaseRecipe baseRecipe, out HomemadeDrink createdElement)
        {
            createdElement = null;
            int id = 0;
            if (element.Attribute(PersistedId) != null &&
                int.TryParse(element.Attribute(PersistedId).Value, out id))
            {
                createdElement = new HomemadeDrink(baseRecipe, id);
                TryPopulateInstance(createdElement, element);
                baseRecipe.AddModifiedRecipe(createdElement);
                return true;
            }
            return false;
        }
    }
}
