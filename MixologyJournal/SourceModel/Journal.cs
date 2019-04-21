using System.Collections.Generic;
using System.Linq;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.SourceModel
{
    internal class Journal
    {
        private List<IJournalEntry> _entries;
        private List<BaseRecipe> _recipes;

        public IEnumerable<IJournalEntry> Entries
        {
            get
            {
                return _entries;
            }
        }

        public IEnumerable<BaseRecipe> BaseRecipes
        {
            get
            {
                return _recipes;
            }
        }

        public Journal()
        {
            _entries = new List<IJournalEntry>();
            _recipes = new List<BaseRecipe>();
        }

        public void AddEntry(IJournalEntry entry)
        {
            _entries.Add(entry);
        }

        public void AddRecipe(BaseRecipe recipe)
        {
            _recipes.Add(recipe);
        }

        public void RemoveRecipe(BaseRecipe recipe)
        {
            _recipes.Remove(recipe);
        }

        public void RemoveEntry(IJournalEntry entry)
        {
            _entries.Remove(entry);
            HomemadeDrinkEntry modifiedRecipe = entry as HomemadeDrinkEntry;
            if (modifiedRecipe != null && modifiedRecipe.Equals(modifiedRecipe.Recipe.BaseRecipe.FavoriteRecipe))
            {
                modifiedRecipe.Recipe.BaseRecipe.FavoriteRecipe = null;
            }
        }

        public HomemadeDrink GetModifiedRecipe(int id)
        {
            return _recipes.SelectMany(r => r.DerivedRecipes).SingleOrDefault(r => r.ID == id);
        }
    }
}
