using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;
using MixologyJournal.ViewModel.Recipe;
using Moq;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class JournalViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void NormalConstructorTest()
        {
            Mock<BaseMixologyApp> app = new Mock<BaseMixologyApp>();
            Journal model = new Journal();
            model.AddEntry(new NoteEntry());
            model.AddRecipe(new BaseRecipe());
            JournalViewModel journal = new JournalViewModel(model, app.Object);
            app.Setup(a => a.Journal).Returns(journal);

            Assert.AreEqual(1, app.Object.Journal.BaseRecipes.Count());
            Assert.AreEqual(1, app.Object.Journal.Entries.Count());
            Assert.IsFalse(app.Object.Journal.IsEmpty);
        }

        [TestMethod]
        public void AddRemoveRecipeTest()
        {
            BaseMixologyApp app = CreateTestApp();

            BaseRecipePageViewModel recipe = new BaseRecipePageViewModel(app);
            HomemadeDrinkEntry modified = new HomemadeDrinkEntry(new HomemadeDrink((recipe.Recipe as BaseRecipeViewModel).Model));
            recipe.AddModifiedRecipe(new HomemadeDrinkEntryViewModel(modified, recipe, app));
            (app.Journal as JournalViewModel).AddBaseRecipeAsync(recipe).Wait();

            Assert.AreEqual(1, app.Journal.BaseRecipes.Count());
            Assert.AreEqual(1, app.Journal.Entries.Count());

            app.Journal.RemoveRecipeAsync(recipe).Wait();

            Assert.AreEqual(0, app.Journal.BaseRecipes.Count());
            Assert.AreEqual(0, app.Journal.Entries.Count());
        }

        [TestMethod]
        public void RemoveNonExistentRecipeTest()
        {
            BaseMixologyApp app = CreateTestApp();

            BaseRecipePageViewModel recipe = new BaseRecipePageViewModel(app);
            HomemadeDrinkEntry modified = new HomemadeDrinkEntry(new HomemadeDrink((recipe.Recipe as BaseRecipeViewModel).Model));
            recipe.AddModifiedRecipe(new HomemadeDrinkEntryViewModel(modified, recipe, app));

            app.Journal.RemoveRecipeAsync(recipe).Wait();

            Assert.AreEqual(0, app.Journal.BaseRecipes.Count());
            Assert.AreEqual(0, app.Journal.Entries.Count());
        }

        [TestMethod]
        public void RemoveRecipeWithNewModifiedTest()
        {
            BaseMixologyApp app = CreateTestApp();

            BaseRecipePageViewModel recipe = new BaseRecipePageViewModel(app);
            (app.Journal as JournalViewModel).AddBaseRecipeAsync(recipe).Wait();

            HomemadeDrinkEntry modified = new HomemadeDrinkEntry(new HomemadeDrink((recipe.Recipe as BaseRecipeViewModel).Model));
            recipe.AddModifiedRecipe(new HomemadeDrinkEntryViewModel(modified, recipe, app));

            Assert.AreEqual(1, app.Journal.BaseRecipes.Count());
            Assert.AreEqual(0, app.Journal.Entries.Count());

            app.Journal.RemoveRecipeAsync(recipe).Wait();

            Assert.AreEqual(0, app.Journal.BaseRecipes.Count());
            Assert.AreEqual(0, app.Journal.Entries.Count());
        }

        [TestMethod]
        public void AddRemoveEntryTest()
        {
            BaseMixologyApp app = CreateTestApp();

            NotesEntryViewModel entry = new NotesEntryViewModel(app);
            (app.Journal as JournalViewModel).AddEntryAsync(entry).Wait();

            Assert.AreEqual(0, app.Journal.BaseRecipes.Count());
            Assert.AreEqual(1, app.Journal.Entries.Count());

            app.Journal.RemoveEntryAsync(entry).Wait();

            Assert.AreEqual(0, app.Journal.BaseRecipes.Count());
            Assert.AreEqual(0, app.Journal.Entries.Count());
        }

        [TestMethod]
        public void RemoveNonExistentEntryTest()
        {
            BaseMixologyApp app = CreateTestApp();

            NotesEntryViewModel entry = new NotesEntryViewModel(app);
            app.Journal.RemoveEntryAsync(entry).Wait();

            Assert.AreEqual(0, app.Journal.BaseRecipes.Count());
            Assert.AreEqual(0, app.Journal.Entries.Count());
        }
    }
}
