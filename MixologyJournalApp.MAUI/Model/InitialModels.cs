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
                        Id = "BuiltIn-1",
                        Name = "Ounce",
                        Plural = "Ounces",
                        Format = "{0} {1} of {2}"
                    },
                    new Unit
                    {
                        Id = "BuiltIn-2",
                        Name = "Dash",
                        Plural = "Dashes",
                        Format = "{0} {1} of {2}"
                    },
                    new Unit
                    {
                        Id = "BuiltIn-3",
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
                        Id = "BuiltIn-1",
                        Name = "Whisky",
                        Plural = "Whisky"
                    },
                    new Ingredient
                    {
                        Id = "BuiltIn-2",
                        Name = "Simple Syrup",
                        Plural = "Simple Syrup"
                    },
                    new Ingredient
                    {
                        Id = "BuiltIn-3",
                        Name = "Angostura Bitters",
                        Plural = "Angostura Bitters"
                    }
                };
            }
        }

        public static List<Recipe> Recipes
        {
            get
            {
                return new List<Recipe>
                {
                    new Recipe
                    {
                        Id = "BuiltIn-1",
                        Name = "Old-Fashioned",
                        Steps = new List<String>
                        {
                            "Combine all ingredients",
                            "Stir with ice",
                            "Serve"
                        },
                        Ingredients = new List<IngredientUsage>
                        {
                            new IngredientUsage
                            {
                                Id = "BuiltIn-OldFashioned-1",
                                Ingredient = Ingredients[0],
                                Unit = Units[0],
                                Brand = null,
                                Amount = "2",
                                OwnerId = "BuiltIn-1"
                            },
                            new IngredientUsage
                            {
                                Id = "BuiltIn-OldFashioned-2",
                                Ingredient = Ingredients[1],
                                Unit = Units[0],
                                Brand = null,
                                Amount = "0.5",
                                OwnerId = "BuiltIn-1"
                            },
                            new IngredientUsage
                            {
                                Id = "BuiltIn-OldFashioned-3",
                                Ingredient = Ingredients[2],
                                Unit = Units[1],
                                Brand = null,
                                Amount = "2",
                                OwnerId = "BuiltIn-1"
                            }
                        }
                    }
                };
            }
        }
    }
}
