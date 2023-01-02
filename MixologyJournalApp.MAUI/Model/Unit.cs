using MixologyJournalApp.MAUI.Data;
using SQLite;

namespace MixologyJournalApp.MAUI.Model
{
    internal class Unit: ICanSave
    {
        [PrimaryKey]
        public String Id
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
                Name = "",
                Plural = "",
                Format = "{0} {1} of {2}"
            };
            return unit;
        }

        public Unit()
        {
            Id = Guid.NewGuid().ToString();
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

        public async Task SaveAsync(IStateSaver stateSaver)
        {
            await stateSaver.InsertOrReplaceAsync(this);
        }
    }
}
