using System;
using System.Xml.Linq;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.Persistence
{
    internal class JournalPersister : ISettingsPersister<Journal>
    {
        private ISettingsPersister<NoteEntry> _entryPersister;
        private ISettingsPersister<BaseRecipe> _baseRecipePersister;
        private ISettingsPersister<HomemadeDrinkEntry> _modifiedRecipeEntryPersister;
        private ISettingsPersister<BarDrinkEntry> _barDrinkPersister;

        public JournalPersister()
        {
            _entryPersister = new TextJournalEntryPersister();
            _baseRecipePersister = new RecipePersister();
            _modifiedRecipeEntryPersister = new ModifiedRecipeEntryPersister();
            _barDrinkPersister = new BarDrinkEntryPersister();
        }

        private const String PersistedName = "Journal";
        public void Write(Journal elementToPersist, XContainer parentNode)
        {
            XElement journalElem = new XElement(PersistedName);
            parentNode.Add(journalElem);
            // Important: Write the recipes first so that they can be referenced from entries.
            foreach (BaseRecipe recipe in elementToPersist.BaseRecipes)
            {
                _baseRecipePersister.Write(recipe, journalElem);
            }
            foreach (IJournalEntry entry in elementToPersist.Entries)
            {
                if (entry is NoteEntry)
                {
                    _entryPersister.Write(entry as NoteEntry, journalElem);
                }
                else if (entry is HomemadeDrinkEntry)
                {
                    _modifiedRecipeEntryPersister.Write(entry as HomemadeDrinkEntry, journalElem);
                }
                else if (entry is BarDrinkEntry)
                {
                    _barDrinkPersister.Write(entry as BarDrinkEntry, journalElem);
                }
            }
        }

        public Boolean TryCreate(XElement element, Journal journal, out Journal createdElement)
        {
            createdElement = null;
            if (element.Name.LocalName.Equals(PersistedName))
            {
                createdElement = new Journal();
                foreach (XElement elem in element.Elements())
                {
                    NoteEntry entry;
                    BaseRecipe recipe;
                    HomemadeDrinkEntry modifiedRecipeEntry;
                    BarDrinkEntry barDrinkEntry;
                    if (_entryPersister.TryCreate(elem, createdElement, out entry))
                    {
                        createdElement.AddEntry(entry);
                    }
                    else if (_baseRecipePersister.TryCreate(elem, createdElement, out recipe))
                    {
                        createdElement.AddRecipe(recipe);
                    }
                    else if (_modifiedRecipeEntryPersister.TryCreate(elem, createdElement, out modifiedRecipeEntry))
                    {
                        createdElement.AddEntry(modifiedRecipeEntry);
                    }
                    else if (_barDrinkPersister.TryCreate(elem, createdElement, out barDrinkEntry))
                    {
                        createdElement.AddEntry(barDrinkEntry);
                    }
                }
                return true;
            }
            return false;
        }
    }
}
