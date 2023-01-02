using MixologyJournalApp.MAUI.Data;
using SQLite;

namespace MixologyJournalApp.MAUI.Model
{
    internal class Ingredient: ICanSave
    {
        public String Name
        {
            get;
            set;
        }

        public String Plural
        {
            get;
            set;
        }

        [PrimaryKey]
        public String Id
        {
            get;
            set;
        }

        public static Ingredient CreateEmpty()
        {
            Ingredient ingredient = new Ingredient
            {
                Name = "",
                Plural = ""
            };
            return ingredient;
        }

        public Ingredient()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Ingredient Clone()
        {
            Ingredient clone = new Ingredient
            {
                Id = Id,
                Name = Name,
                Plural = Plural
            };
            return clone;
        }

        public async Task SaveAsync(IStateSaver stateSaver)
        {
            await stateSaver.InsertOrReplaceAsync(this);
        }
    }
}
