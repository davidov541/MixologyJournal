using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.Persistence;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournalApp.Test.Persistence
{
    [TestClass]
    public class RecipePersisterUnitTests : PersisterTests
    {
        #region Loading Tests
        private const String BasicPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID=""4"" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            <DerivedRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""1"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </DerivedRecipe>
            <DerivedRecipe Title=""Favorite"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""4"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""5"" />
            </DerivedRecipe>
            </BaseRecipe>";
        [TestMethod]
        public void BasicLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(BasicPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check base recipe.
            CheckLoadedRecipe(createdRecipe, "Bellini", 3.5);
            Assert.AreEqual(2, createdRecipe.DerivedRecipes.Count());
            Assert.IsTrue(ReferenceEquals(createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Favorite")), createdRecipe.FavoriteRecipe));

            // Check first recipe.
            HomemadeDrink firstRecipe = createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Bellini"));
            Assert.IsNotNull(firstRecipe);
            CheckLoadedRecipe(firstRecipe, "Bellini", 3.5);

            // Check second recipe.
            HomemadeDrink secondRecipe = createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Favorite"));
            Assert.IsNotNull(secondRecipe);
            CheckLoadedRecipe(secondRecipe, "Favorite", 5.0);
        }

        private const String NoIDPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID=""4"" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            <DerivedRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""1"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </DerivedRecipe>
            <DerivedRecipe Title=""Favorite"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""4"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""5"" />
            </DerivedRecipe>
            </BaseRecipe>";
        [TestMethod]
        public void NoIDLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(NoIDPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check base recipe.
            CheckLoadedRecipe(createdRecipe, "Bellini", 3.5);
            Assert.AreEqual(2, createdRecipe.DerivedRecipes.Count());
            Assert.IsTrue(ReferenceEquals(createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Favorite")), createdRecipe.FavoriteRecipe));

            // Check first recipe.
            HomemadeDrink firstRecipe = createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Bellini"));
            Assert.IsNotNull(firstRecipe);
            CheckLoadedRecipe(firstRecipe, "Bellini", 3.5);

            // Check second recipe.
            HomemadeDrink secondRecipe = createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Favorite"));
            Assert.IsNotNull(secondRecipe);
            CheckLoadedRecipe(secondRecipe, "Favorite", 5.0);
        }

        private const String NoChildrenPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID="""" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BaseRecipe>";
        [TestMethod]
        public void NoDerivedRecipesLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(NoChildrenPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            CheckLoadedRecipe(createdRecipe, "Bellini", 3.5);
            Assert.AreEqual(0, createdRecipe.DerivedRecipes.Count());
            Assert.IsNull(createdRecipe.FavoriteRecipe);
        }

        private const String CompletelyEmptyPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title="""" Instructions="""" FavoriteID="""" ID=""0"" />";
        [TestMethod]
        public void CompletelyEmptyLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(CompletelyEmptyPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            Assert.AreEqual("", createdRecipe.Name);
            Assert.AreEqual("", createdRecipe.Instructions);
            Assert.AreEqual(0, createdRecipe.Ingredients.Count());
            Assert.AreEqual(0, createdRecipe.DerivedRecipes.Count());
            Assert.IsNull(createdRecipe.FavoriteRecipe);
        }

        private const String NoAttributesPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe />";
        [TestMethod]
        public void NoAttributesLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(NoAttributesPersistedRecipe);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));
        }

        private const String DifferentTagNamePersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <AnotherTag />";
        [TestMethod]
        public void DifferentTagNameLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(DifferentTagNamePersistedRecipe);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));
        }

        private const String InvalidFavoriteIdPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title="""" Instructions="""" FavoriteID=""5"" ID=""0"" />";
        [TestMethod]
        public void InvalidFavoriteIdLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(CompletelyEmptyPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            Assert.AreEqual("", createdRecipe.Name);
            Assert.AreEqual("", createdRecipe.Instructions);
            Assert.AreEqual(0, createdRecipe.Ingredients.Count());
            Assert.AreEqual(0, createdRecipe.DerivedRecipes.Count());
            Assert.IsNull(createdRecipe.FavoriteRecipe);
        }

        private const String InvalidQuantityPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID="""" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""blah"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BaseRecipe>";
        [TestMethod]
        public void InvalidQuantityLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(InvalidQuantityPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            Assert.AreEqual("Bellini", createdRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", createdRecipe.Instructions);
            Assert.AreEqual(1, createdRecipe.Ingredients.Count());
            Assert.IsTrue(createdRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
            Assert.AreEqual(0, createdRecipe.DerivedRecipes.Count());
            Assert.IsNull(createdRecipe.FavoriteRecipe);
        }

        private const String InvalidUnitPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID="""" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Blah"" AmountQuantity=""5"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BaseRecipe>";
        [TestMethod]
        public void InvalidUnitLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(InvalidUnitPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            Assert.AreEqual("Bellini", createdRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", createdRecipe.Instructions);
            Assert.AreEqual(1, createdRecipe.Ingredients.Count());
            Assert.IsTrue(createdRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
            Assert.AreEqual(0, createdRecipe.DerivedRecipes.Count());
            Assert.IsNull(createdRecipe.FavoriteRecipe);
        }

        private const String NoIngredientAttributesPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID="""" ID=""0"">
            <Ingredient />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BaseRecipe>";
        [TestMethod]
        public void NoIngredientAttributesLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(InvalidUnitPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            Assert.AreEqual("Bellini", createdRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", createdRecipe.Instructions);
            Assert.AreEqual(1, createdRecipe.Ingredients.Count());
            Assert.IsTrue(createdRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
            Assert.AreEqual(0, createdRecipe.DerivedRecipes.Count());
            Assert.IsNull(createdRecipe.FavoriteRecipe);
        }

        private const String InvalidModifiedQuantityPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID=""4"" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            <DerivedRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""1"">
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""Blah"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </DerivedRecipe>
            </BaseRecipe>";
        [TestMethod]
        public void InvalidModifiedQuantityLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(InvalidModifiedQuantityPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check first recipe.
            HomemadeDrink firstRecipe = createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Bellini"));
            Assert.IsNotNull(firstRecipe);
            Assert.AreEqual("Bellini", firstRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", firstRecipe.Instructions);
            Assert.AreEqual(1, firstRecipe.Ingredients.Count());
            Assert.IsTrue(firstRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
        }

        private const String InvalidModifiedUnitPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID=""4"" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            <DerivedRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""1"">
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Blah"" AmountQuantity=""5.0"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </DerivedRecipe>
            </BaseRecipe>";
        [TestMethod]
        public void InvalidModifiedUnitLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(InvalidModifiedQuantityPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check first recipe.
            HomemadeDrink firstRecipe = createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Bellini"));
            Assert.IsNotNull(firstRecipe);
            Assert.AreEqual("Bellini", firstRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", firstRecipe.Instructions);
            Assert.AreEqual(1, firstRecipe.Ingredients.Count());
            Assert.IsTrue(firstRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
        }

        private const String NoModifiedIngredientAttributePersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID=""4"" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            <DerivedRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""1"">
            <Ingredient />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </DerivedRecipe>
            </BaseRecipe>";
        [TestMethod]
        public void NoModifiedIngredientAttributeLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(NoModifiedIngredientAttributePersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check first recipe.
            HomemadeDrink firstRecipe = createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Bellini"));
            Assert.IsNotNull(firstRecipe);
            Assert.AreEqual("Bellini", firstRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", firstRecipe.Instructions);
            Assert.AreEqual(1, firstRecipe.Ingredients.Count());
            Assert.IsTrue(firstRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
        }

        private const String InvalidChildPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID=""4"" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            <Blah />
            </BaseRecipe>";
        [TestMethod]
        public void InvalidChildLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(NoModifiedIngredientAttributePersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            CheckLoadedRecipe(createdRecipe, "Bellini", 3.5);
        }

        private const String InvalidModifiedChildPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID=""4"" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            <DerivedRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""1"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            <Blah />
            </DerivedRecipe>
            </BaseRecipe>";
        [TestMethod]
        public void InvalidModifiedChildLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            RecipePersister persister = new RecipePersister();
            BaseRecipe createdRecipe;
            XDocument doc = XDocument.Parse(NoModifiedIngredientAttributePersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check first recipe.
            HomemadeDrink firstRecipe = createdRecipe.DerivedRecipes.FirstOrDefault(r => r.Name.Equals("Bellini"));
            Assert.IsNotNull(firstRecipe);
            Assert.AreEqual("Bellini", firstRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", firstRecipe.Instructions);
            Assert.AreEqual(1, firstRecipe.Ingredients.Count());
            Assert.IsTrue(firstRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
        }

        private void CheckLoadedRecipe(Recipe createdRecipe, String name, double proseccoAmount)
        {
            Assert.AreEqual(name, createdRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", createdRecipe.Instructions);
            Assert.AreEqual(2, createdRecipe.Ingredients.Count());
            Assert.IsTrue(createdRecipe.Ingredients.Contains(new Ingredient("White Peach Puree", "", new Amount(2.0, AmountUnit.Ounce))));
            Assert.IsTrue(createdRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(proseccoAmount, AmountUnit.Ounce))));
        }
        #endregion

        #region Saving Tests
        private const String BasicSavedRecipe = @"<BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" FavoriteID=""1"" ID=""0"">
              <Ingredient Name = ""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
              <Ingredient Name = ""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
              <DerivedRecipe Title = ""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
                <Ingredient Name = ""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
                <Ingredient Name = ""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
              </DerivedRecipe>
              <DerivedRecipe Title = ""Bellini2"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""1"">
                <Ingredient Name = ""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
                <Ingredient Name = ""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""5"" />
              </DerivedRecipe>
            </BaseRecipe>";
        [TestMethod]
        public void BasicSaveTest()
        {
            BaseRecipe recipe = new BaseRecipe("Bellini", "1. Add prosecco to puree slowly while mixing.");
            recipe.AddIngredient(new Ingredient("White Peach Puree", "", new Amount(2, AmountUnit.Ounce)));
            recipe.AddIngredient(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce)));

            HomemadeDrink modifiedOne = new HomemadeDrink(recipe);
            recipe.AddModifiedRecipe(modifiedOne);

            HomemadeDrink modifiedTwo = new HomemadeDrink(recipe);
            modifiedTwo.Name = "Bellini2";
            modifiedTwo.Ingredients.SingleOrDefault(i => i.Name.Equals("Prosecco")).Amount.Quantity = 5;
            recipe.AddModifiedRecipe(modifiedTwo);
            recipe.FavoriteRecipe = modifiedTwo;

            XDocument doc = new XDocument();
            RecipePersister persister = new RecipePersister();
            persister.Write(recipe, doc);

            XDocument expectedDoc = XDocument.Parse(BasicSavedRecipe);

            CompareXml(expectedDoc, doc);
        }

        private const String NoDerivedSavedRecipe = @"<BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
              <Ingredient Name = ""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
              <Ingredient Name = ""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BaseRecipe>";
        [TestMethod]
        public void NoDerivedRecipesSaveTest()
        {
            BaseRecipe recipe = new BaseRecipe("Bellini", "1. Add prosecco to puree slowly while mixing.");
            recipe.AddIngredient(new Ingredient("White Peach Puree", "", new Amount(2, AmountUnit.Ounce)));
            recipe.AddIngredient(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce)));

            XDocument doc = new XDocument();
            RecipePersister persister = new RecipePersister();
            persister.Write(recipe, doc);

            XDocument expectedDoc = XDocument.Parse(NoDerivedSavedRecipe);

            CompareXml(expectedDoc, doc);
        }

        private const String NoIngredientsSavedRecipe = @"<BaseRecipe Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
              <Ingredient Name = ""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
              <Ingredient Name = ""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BaseRecipe>";
        [TestMethod]
        public void NoIngredientsRecipesSaveTest()
        {
            BaseRecipe recipe = new BaseRecipe("Bellini", "1. Add prosecco to puree slowly while mixing.");
            recipe.AddIngredient(new Ingredient("White Peach Puree", "", new Amount(2, AmountUnit.Ounce)));
            recipe.AddIngredient(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce)));

            XDocument doc = new XDocument();
            RecipePersister persister = new RecipePersister();
            persister.Write(recipe, doc);

            XDocument expectedDoc = XDocument.Parse(NoIngredientsSavedRecipe);

            CompareXml(expectedDoc, doc);
        }
        #endregion
    }
}
