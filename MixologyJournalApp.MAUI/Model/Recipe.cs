using MixologyJournalApp.MAUI.Data;
using SQLite;

namespace MixologyJournalApp.MAUI.Model
{
    internal class Recipe: ICanSave
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

        public String StepsString
        {
            get
            {
                return String.Join("||", this.Steps);
            }
            set
            {
                this.Steps = new List<String>(value.Split("||"));
            }
        }

        [PrimaryKey]
        public String Id
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
            recipe.Ingredients.Add(IngredientUsage.CreateEmpty(recipe.Id));

            return recipe;
        }

        public Recipe()
        {
            Steps = new List<String>();
            Ingredients = new List<IngredientUsage>();
            Id = Guid.NewGuid().ToString();
        }

        public async Task SaveAsync(IStateSaver stateSaver)
        {
            await stateSaver.InsertOrReplaceAsync(this);
            foreach (IngredientUsage usage in Ingredients)
            {
                await usage.SaveAsync(stateSaver);
            }
        }  
    }
}
