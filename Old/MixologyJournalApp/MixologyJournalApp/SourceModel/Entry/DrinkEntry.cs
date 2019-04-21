using System;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.SourceModel.Entry
{
    internal abstract class DrinkEntry<T> : JournalEntry, IDrinkEntry where T : Recipe.Recipe
    {
        internal T Recipe
        {
            get;
            private set;
        }

        public double Rating
        {
            get;
            set;
        }

        protected DrinkEntry(T recipe) :
            base(recipe.Name, String.Empty, DateTime.Now)
        {
            Recipe = recipe;
        }

        public DrinkEntry(T recipe, String contents, DateTime creationDate) :
            base(recipe.Name, contents, creationDate)
        {
            Recipe = recipe;
        }
    }
}
