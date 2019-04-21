using System;

namespace MixologyJournal.SourceModel.Recipe
{
    internal enum AmountUnit
    {
        Ounce,
        Milliliter,
        Centiliter,
        Liter,
        Pint,
        Spoon,
        Dash,
        Teaspoon,
        Tablespoon,
        Cup,
        Drop,
        Pinch,
        Slice,
        Twist,
        Piece,
        Unknown,
        ToTaste
    }

    internal class Amount
    {
        public AmountUnit Unit
        {
            get;
            set;
        }

        public double Quantity
        {
            get;
            set;
        }

        public Amount() :
            this(0, AmountUnit.Ounce)
        {
        }

        public Amount(double quantity, AmountUnit unit)
        {
            Unit = unit;
            Quantity = quantity;
        }

        public Amount Clone()
        {
            return new Amount(Quantity, Unit);
        }

        public override Boolean Equals(Object obj)
        {
            Amount other = obj as Amount;
            if (other == null)
            {
                return false;
            }
            return Quantity.Equals(other.Quantity) && Unit.Equals(other.Unit);
        }

        public override Int32 GetHashCode()
        {
            return Quantity.GetHashCode() * 49 + Unit.GetHashCode();
        }
    }
}
