using SQLite;

namespace MixologyJournalApp.MAUI.Model
{
    internal class Unit
    {
        public static List<Unit> InitialUnits
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

        [PrimaryKey]
        public int Id
        {
            get;
            set;
        }

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

        public String Format
        {
            get;
            set;
        }
      
        public static Unit CreateEmpty()
        {
            Unit unit = new Unit
            {
                Id = 0,
                Name = "",
                Plural = "",
                Format = "{0} {1} of {2}"
            };
            return unit;
        }

        public Unit()
        {
        }

        public Unit Clone()
        {
            Unit clone = new Unit
            {
                Id = Id,
                Name = Name,
                Plural = Plural,
                Format = Format
            };
            return clone;
        }
    }
}
