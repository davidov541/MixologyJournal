using System;

namespace MixologyJournal.SourceModel.Recipe
{
    internal class Ingredient
    {
        public String Name
        {
            get;
            set;
        }

        public String Details
        {
            get;
            set;
        }

        public Amount Amount
        {
            get;
            private set;
        }

        public Ingredient() :
            this(String.Empty, String.Empty, new Amount())
        {
        }

        public Ingredient(String name, String brand, Amount amount)
        {
            Name = name;
            Details = brand;
            Amount = amount;
        }

        public Ingredient Clone()
        {
            return new Ingredient(Name, Details, Amount.Clone());
        }

        public override Boolean Equals(Object obj)
        {
            Ingredient other = obj as Ingredient;
            if (other == null)
            {
                return false;
            }
            return Name.Equals(other.Name) && Details.Equals(other.Details) && Amount.Equals(other.Amount);
        }

        public override Int32 GetHashCode()
        {
            return ((Name.GetHashCode() * 49) + Details.GetHashCode()) * 49 + Amount.GetHashCode();
        }
    }
}
