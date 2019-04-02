using System;

namespace MixologyJournal.SourceModel.Recipe
{
    internal class BarDrink : Recipe
    {
        public BarDrink()
            : base(String.Empty, String.Empty)
        {
        }

        public BarDrink(int id)
            : base(String.Empty, String.Empty, id)
        {
        }
    }
}
