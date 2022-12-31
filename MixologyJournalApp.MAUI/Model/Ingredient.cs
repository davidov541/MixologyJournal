namespace MixologyJournalApp.MAUI.Model
{
    internal class Ingredient
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
                Id = "",
                Plural = ""
            };
            return ingredient;
        }

        public Ingredient()
        {
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
    }
}
