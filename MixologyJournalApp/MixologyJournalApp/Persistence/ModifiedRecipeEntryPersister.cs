using System;
using System.Xml.Linq;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.Persistence
{
    internal class ModifiedRecipeEntryPersister : JournalEntryPersister, ISettingsPersister<HomemadeDrinkEntry>
    {
        private const String PersistedName = "ModifiedRecipeJournalEntry";
        private const String PersistedRecipeId = "RecipeID";
        private const String PersistedRating = "Rating";

        public void Write(HomemadeDrinkEntry elementToWrite, XContainer parentNode)
        {
            XElement entryElem = new XElement(PersistedName);
            PopulateXmlElement(entryElem, elementToWrite, parentNode);
            entryElem.SetAttributeValue(PersistedRecipeId, elementToWrite.Recipe.ID.ToString());
            entryElem.SetAttributeValue(PersistedRating, elementToWrite.Rating.ToString());
            parentNode.Add(entryElem);
        }

        public Boolean TryCreate(XElement element, Journal journal, out HomemadeDrinkEntry createdElement)
        {
            int recipeId = 0;
            createdElement = null;
            if (element.Name.LocalName.Equals(PersistedName) &&
                element.Attribute(PersistedRecipeId) != null &&
                int.TryParse(element.Attribute(PersistedRecipeId).Value, out recipeId))
            {
                HomemadeDrink recipe = journal.GetModifiedRecipe(recipeId);
                if (recipe != null)
                {
                    createdElement = new HomemadeDrinkEntry(recipe);
                    double rating = 0.0;
                    if (element.Attribute(PersistedRating) != null &&
                        Double.TryParse(element.Attribute(PersistedRating).Value, out rating))
                    {
                        createdElement.Rating = rating;
                    }
                    return TryPopulateInstance(createdElement, element);
                }
            }
            return false;
        }
    }
}
