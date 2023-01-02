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

        private Ingredient _ingredient;
        [Ignore]
        public Ingredient Ingredient
        {
            get
            {
                return this._ingredient;
            }
            set
            {
                this._ingredient = value;
                this.IngredientId = value.Id;
            }
        }

        public String IngredientId
        {
            get;
            set;
        }

        private Unit _unit;
        [Ignore]
        public Unit Unit
        {
            get
            {
                return this._unit;
            }
            set
            {
                this._unit = value;
                this.UnitId = value.Id;
            }
        }

        public String UnitId
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
