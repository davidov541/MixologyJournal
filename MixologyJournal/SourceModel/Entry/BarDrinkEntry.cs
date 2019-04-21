using System;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournalApp.ViewModel.LocationServices;

namespace MixologyJournal.SourceModel.Entry
{
    internal class BarDrinkEntry : DrinkEntry<BarDrink>
    {
        internal INearbyPlace Location
        {
            get;
            set;
        }

        internal BarDrinkEntry(BarDrink recipe) :
            base(recipe, String.Empty, DateTime.Now)
        {
        }

        internal BarDrinkEntry(BarDrink recipe, String contents, DateTime creationDate) : 
            base(recipe, contents, creationDate)
        {
        }
    }
}
