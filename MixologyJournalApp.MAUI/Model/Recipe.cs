namespace MixologyJournalApp.MAUI.Model
{
    internal class Recipe
    {
        public String Name
        {
            get;
            set;
        }

        public List<String> Steps
        {
            get;
            set;
        }

        public String Id
        {
            get;
            set;
        }

        public List<IngredientUsage> Ingredients
        {
            get;
            set;
        }

        public Boolean IsBuiltIn
        {
            get;
            set;
        }

        public static Recipe CreateEmptyRecipe()
        {
            Recipe recipe = new Recipe();

            recipe.Steps.Add("");
            recipe.Ingredients.Add(IngredientUsage.CreateEmpty());

            return recipe;
        }

        public Recipe()
        {
            Steps = new List<String>();
            Ingredients = new List<IngredientUsage>();
        }
    }
}
