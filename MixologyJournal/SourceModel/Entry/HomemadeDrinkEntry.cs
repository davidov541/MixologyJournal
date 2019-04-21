using System;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.SourceModel.Entry
{
    internal class HomemadeDrinkEntry : DrinkEntry<HomemadeDrink>
    {
        public HomemadeDrinkEntry(HomemadeDrink recipe) :
            base(recipe, String.Empty, DateTime.Now)
        {
        }

        public HomemadeDrinkEntry(HomemadeDrink recipe, String contents, DateTime creationDate) : 
            base(recipe, contents, creationDate)
        {
        }
    }
}
