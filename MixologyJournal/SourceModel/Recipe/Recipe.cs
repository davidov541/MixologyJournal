using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MixologyJournalTest")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace MixologyJournal.SourceModel.Recipe
{
    internal abstract class Recipe
    {
        private static int NextID = 0;

        private List<Ingredient> _ingredients;
        public String Name
        {
            get;
            set;
        }

        public int ID
        {
            get;
            private set;
        }

        public IEnumerable<Ingredient> Ingredients
        {
            get
            {
                return _ingredients;
            }
        }

        public String Instructions
        {
            get;
            set;
        }

        protected Recipe(String name, String instructions, int id)
        {
            Name = name;
            Instructions = instructions;
            _ingredients = new List<Ingredient>();

            ID = id;
            if (ID >= NextID)
            {
                NextID = ID + 1;
            }
        }

        protected Recipe(String name, String instructions) :
            this(name, instructions, NextID)
        {
        }

        public void AddIngredient(Ingredient ingredient)
        {
            _ingredients.Add(ingredient);
        }

        public void RemoveIngredient(Ingredient ingredient)
        {
            _ingredients.Remove(ingredient);
        }

        public void ClearIngredients()
        {
            _ingredients.Clear();
        }
    }
}
