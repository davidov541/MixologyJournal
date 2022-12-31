namespace MixologyJournalApp.MAUI.Model
{
    internal class IngredientUsage
    {
        public Ingredient Ingredient
        {
            get;
            set;
        }

        public Unit Unit
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
                Amount = Amount,
                Ingredient = Ingredient.Clone(),
                Unit = Unit.Clone(),
                Brand = Brand
            };
            return clone;
        }
    }
}
