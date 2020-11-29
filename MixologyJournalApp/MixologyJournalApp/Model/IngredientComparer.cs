using System.Collections.Generic;

namespace MixologyJournalApp.Model
{
    internal class IngredientComparer : IEqualityComparer<Ingredient>
    {
        public bool Equals(Ingredient x, Ingredient y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Ingredient obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
