using System;
using System.Collections.Generic;

namespace MixologyJournal.SourceModel.Recipe
{
    internal class BaseRecipe : Recipe
    {
        private List<HomemadeDrink> _derivedRecipes;
        public IEnumerable<HomemadeDrink> DerivedRecipes
        {
            get
            {
                return _derivedRecipes;
            }
        }

        public HomemadeDrink FavoriteRecipe
        {
            get;
            set;
        }

        public BaseRecipe() :
            this(String.Empty, String.Empty)
        {
        }

        public BaseRecipe(int id)
            : this(String.Empty, String.Empty, id)
        {
        }

        public BaseRecipe(String name, String instructions)
            : base(name, instructions)
        {
            _derivedRecipes = new List<HomemadeDrink>();
        }

        public BaseRecipe(String name, String instructions, int id)
            : base(name, instructions, id)
        {
            _derivedRecipes = new List<HomemadeDrink>();
        }

        public void AddModifiedRecipe(HomemadeDrink entry)
        {
            _derivedRecipes.Add(entry);
        }

        public HomemadeDrink Clone()
        {
            return new HomemadeDrink(this);
        }
    }
}
