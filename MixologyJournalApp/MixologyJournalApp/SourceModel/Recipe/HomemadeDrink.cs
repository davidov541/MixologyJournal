namespace MixologyJournal.SourceModel.Recipe
{
    internal class HomemadeDrink : Recipe
    {
        public BaseRecipe BaseRecipe
        {
            get;
            private set;
        }

        public HomemadeDrink(BaseRecipe recipe)
            : base(recipe.Name, recipe.Instructions)
        {
            Initialize(recipe);
        }

        public HomemadeDrink(BaseRecipe recipe, int id)
            : base(recipe.Name, recipe.Instructions, id)
        {
            Initialize(recipe);
        }

        private void Initialize(BaseRecipe recipe)
        {
            BaseRecipe = recipe;
            foreach (Ingredient ingredient in recipe.Ingredients)
            {
                AddIngredient(ingredient.Clone());
            }
        }
    }
}
