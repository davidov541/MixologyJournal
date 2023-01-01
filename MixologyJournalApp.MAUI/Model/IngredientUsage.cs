using SQLite;

namespace MixologyJournalApp.MAUI.Model
{
    internal class IngredientUsage
    {
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get;
            set;
        }

        [Ignore]
        public Ingredient Ingredient
        {
            get;
            set;
        }

        public int IngredientId
        {
            get;
            set;
        }

        [Ignore]
        public Unit Unit
        {
            get;
            set;
        }

        public int UnitId
        {
            get;
            set;
        }

        public String Brand
        {
            get;
            set;
        }

        public String Amount
        {
            get;
            set;
        }

        public static IngredientUsage CreateEmpty()
        {
            IngredientUsage usage = new IngredientUsage
            {
                Id = 0,
                Amount = "",
                Unit = Unit.CreateEmpty(),
                Ingredient = Ingredient.CreateEmpty(),
                Brand = null
            };
            return usage;
        }

        public IngredientUsage()
        {
        }

        public IngredientUsage Clone()
        {
            IngredientUsage clone = new IngredientUsage
            {
                Id = Id,
                Amount = Amount,
                Ingredient = Ingredient.Clone(),
                Unit = Unit.Clone(),
                Brand = Brand
            };
            return clone;
        }
    }
}
