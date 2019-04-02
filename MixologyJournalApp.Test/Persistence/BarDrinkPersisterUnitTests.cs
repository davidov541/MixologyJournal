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
    public class BarDrinkPersisterUnitTests : PersisterTests
    {
        #region Loading Tests
        private const String BasicPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BarDrink Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BarDrink>";
        [TestMethod]
        public void BasicLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkPersister persister = new BarDrinkPersister();
            BarDrink createdRecipe;
            XDocument doc = XDocument.Parse(BasicPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check base recipe.
            CheckLoadedRecipe(createdRecipe, "Bellini", 3.5);
        }

        private const String CompletelyEmptyPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BarDrink Title="""" Instructions="""" ID=""0"" />";
        [TestMethod]
        public void CompletelyEmptyLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkPersister persister = new BarDrinkPersister();
            BarDrink createdRecipe;
            XDocument doc = XDocument.Parse(CompletelyEmptyPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            Assert.AreEqual("", createdRecipe.Name);
            Assert.AreEqual("", createdRecipe.Instructions);
            Assert.AreEqual(0, createdRecipe.Ingredients.Count());
        }

        private const String NoAttributesPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BarDrink />";
        [TestMethod]
        public void NoAttributesLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkPersister persister = new BarDrinkPersister();
            BarDrink createdRecipe;
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
            BarDrinkPersister persister = new BarDrinkPersister();
            BarDrink createdRecipe;
            XDocument doc = XDocument.Parse(DifferentTagNamePersistedRecipe);
            Assert.IsFalse(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));
        }

        private const String InvalidQuantityPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BarDrink Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""blah"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BarDrink>";
        [TestMethod]
        public void InvalidQuantityLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkPersister persister = new BarDrinkPersister();
            BarDrink createdRecipe;
            XDocument doc = XDocument.Parse(InvalidQuantityPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            Assert.AreEqual("Bellini", createdRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", createdRecipe.Instructions);
            Assert.AreEqual(1, createdRecipe.Ingredients.Count());
            Assert.IsTrue(createdRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
        }

        private const String InvalidUnitPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BarDrink Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
            <Ingredient Name=""White Peach Puree"" Brand="""" AmountUnit=""Blah"" AmountQuantity=""5"" />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BarDrink>";
        [TestMethod]
        public void InvalidUnitLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkPersister persister = new BarDrinkPersister();
            BarDrink createdRecipe;
            XDocument doc = XDocument.Parse(InvalidUnitPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            Assert.AreEqual("Bellini", createdRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", createdRecipe.Instructions);
            Assert.AreEqual(1, createdRecipe.Ingredients.Count());
            Assert.IsTrue(createdRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
        }

        private const String NoIngredientAttributesPersistedRecipe = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <BarDrink Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
            <Ingredient />
            <Ingredient Name=""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BarDrink>";
        [TestMethod]
        public void NoIngredientAttributesLoadTest()
        {
            // Run the function.
            Journal journal = new Journal();
            BarDrinkPersister persister = new BarDrinkPersister();
            BarDrink createdRecipe;
            XDocument doc = XDocument.Parse(InvalidUnitPersistedRecipe);
            Assert.IsTrue(persister.TryCreate(doc.Elements().Last(), journal, out createdRecipe));

            // Check recipe
            Assert.AreEqual("Bellini", createdRecipe.Name);
            Assert.AreEqual("1. Add prosecco to puree slowly while mixing.", createdRecipe.Instructions);
            Assert.AreEqual(1, createdRecipe.Ingredients.Count());
            Assert.IsTrue(createdRecipe.Ingredients.Contains(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce))));
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
        private const String BasicSavedRecipe = @"<BarDrink Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
              <Ingredient Name = ""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
              <Ingredient Name = ""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BarDrink>";
        [TestMethod]
        public void BasicSaveTest()
        {
            BarDrink recipe = new BarDrink();
            recipe.Name = "Bellini";
            recipe.Instructions = "1. Add prosecco to puree slowly while mixing.";
            recipe.AddIngredient(new Ingredient("White Peach Puree", "", new Amount(2, AmountUnit.Ounce)));
            recipe.AddIngredient(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce)));

            XDocument doc = new XDocument();
            BarDrinkPersister persister = new BarDrinkPersister();
            persister.Write(recipe, doc);

            XDocument expectedDoc = XDocument.Parse(BasicSavedRecipe);

            CompareXml(expectedDoc, doc);
        }

        private const String NoIngredientsSavedRecipe = @"<BarDrink Title=""Bellini"" Instructions=""1. Add prosecco to puree slowly while mixing."" ID=""0"">
              <Ingredient Name = ""White Peach Puree"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""2"" />
              <Ingredient Name = ""Prosecco"" Brand="""" AmountUnit=""Ounce"" AmountQuantity=""3.5"" />
            </BarDrink>";
        [TestMethod]
        public void NoIngredientsRecipesSaveTest()
        {
            BarDrink recipe = new BarDrink();
            recipe.Name = "Bellini";
            recipe.Instructions = "1. Add prosecco to puree slowly while mixing.";
            recipe.AddIngredient(new Ingredient("White Peach Puree", "", new Amount(2, AmountUnit.Ounce)));
            recipe.AddIngredient(new Ingredient("Prosecco", "", new Amount(3.5, AmountUnit.Ounce)));

            XDocument doc = new XDocument();
            BarDrinkPersister persister = new BarDrinkPersister();
            persister.Write(recipe, doc);

            XDocument expectedDoc = XDocument.Parse(NoIngredientsSavedRecipe);

            CompareXml(expectedDoc, doc);
        }
        #endregion
    }
}
