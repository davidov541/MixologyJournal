namespace MixologyJournalApp.MAUI.Model
{
    internal static class InitialModels
    {
        public static List<Unit> Units
        {
            get
            {
                return new List<Unit>
                {
                    new Unit
                    {
                        Id = 1,
                        Name = "Ounce",
                        Plural = "Ounces",
                        Format = "{0} {1} of {2}"
                    },
                    new Unit
                    {
                        Id = 2,
                        Name = "Dash",
                        Plural = "Dashes",
                        Format = "{0} {1} of {2}"
                    },
                    new Unit
                    {
                        Id = -1,
                        Name = "Slice",
                        Plural = "Slices",
                        Format = "{0} {2} {1}"
                    }
                };
            }
        }
        public static List<Ingredient> Ingredients
        {
            get
            {
                return new List<Ingredient>
                {
                    new Ingredient
                    {
                        Id = 1,
                        Name = "Whisky",
                        Plural = "Whisky"
                    },
                    new Ingredient
                    {
                        Id = -1,
                        Name = "Simple Syrup",
                        Plural = "Simple Syrup"
                    },
                    new Ingredient
                    {
                        Id = -2,
                        Name = "Angostura Bitters",
                        Plural = "Angostura Bitters"
                    }
                };
            }
        }
    }
}
