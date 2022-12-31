using SQLite;

namespace MixologyJournalApp.MAUI.Model
{
    internal class Recipe
    {
        public String Name
        {
            get;
            set;
        }

        [Ignore]
        public List<String> Steps
        {
            get;
            set;
        }

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get;
            set;
        }

        [Ignore]
        public List<IngredientUsage> Ingredients
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
