﻿using MixologyJournalApp.MAUI.Data;
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

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get;
            set;
        }

        public static Ingredient CreateEmpty()
        {
            Ingredient ingredient = new Ingredient
            {
                Name = "",
                Id = 0,
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

        public async Task SaveAsync(IStateSaver stateSaver)
        {
            await stateSaver.InsertOrReplaceAsync(this);
        }
    }
}