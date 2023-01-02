using MixologyJournalApp.MAUI.Data;
using SQLite;

namespace MixologyJournalApp.MAUI.Model
{
    internal class IngredientUsage: ICanSave
    {
        [PrimaryKey]
        public String Id
        {
            get;
            set;
        }

        [Ignore]
        public Ingredient Ingredient
        {
            get;
            set;
        }

        public int IngredientId
        {
            get;
            set;
        }

        [Ignore]
        public Unit Unit
        {
            get;
            set;
        }

        public int UnitId
        {
            get;
            set;
        }

        public String Brand
        {
            get;
            set;
        }

        public String Amount
        {
            get;
            set;
        }

        public String OwnerId
        {
            get;
            set;
        }

        public static IngredientUsage CreateEmpty(String ownerId)
        {
            IngredientUsage usage = new IngredientUsage(ownerId)
            {
                Amount = "",
                Unit = Unit.CreateEmpty(),
                Ingredient = Ingredient.CreateEmpty(),
                Brand = null
            };
            return usage;
        }

        public IngredientUsage()
        {
            Id = Guid.NewGuid().ToString();
        }

        public IngredientUsage(string ownerId): this()
        {
            OwnerId = ownerId;
        }

        public IngredientUsage Clone()
        {
            IngredientUsage clone = new IngredientUsage(OwnerId)
            {
                Id = Id,
                Amount = Amount,
                Ingredient = Ingredient.Clone(),
                Unit = Unit.Clone(),
                Brand = Brand
            };
            return clone;
        }

        public async Task SaveAsync(IStateSaver stateSaver)
        {
            await stateSaver.InsertOrReplaceAsync(this);
        }
    }
}
