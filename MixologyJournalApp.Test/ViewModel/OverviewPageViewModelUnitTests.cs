using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class OverviewPageViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            IMixologyApp app = CreateTestApp();
            OverviewPageViewModel viewModel = CreateTestOverviewPage(app);

            Assert.AreEqual(true, viewModel.AnyBaseRecipes);
            Assert.AreEqual(false, viewModel.Busy);
            Assert.AreEqual(true, viewModel.AnyJournalEntries);
            Assert.AreEqual(2, viewModel.Entries.Count());
            Assert.AreEqual(2, viewModel.Recipes.Count());
        }

        [TestMethod]
        public void UnderlyingRemoveTest()
        {
            IMixologyApp app = CreateTestApp();
            OverviewPageViewModel viewModel = CreateTestOverviewPage(app);

            Assert.AreEqual(true, viewModel.AnyBaseRecipes);
            Assert.AreEqual(true, viewModel.AnyJournalEntries);
            Assert.AreEqual(2, viewModel.Entries.Count());
            Assert.AreEqual(2, viewModel.Recipes.Count());

            (app.Journal as JournalViewModel).RemoveEntryAsync(app.Journal.Entries.OfType<NotesEntryViewModel>().First()).Wait();

            Assert.AreEqual(true, viewModel.AnyBaseRecipes);
            Assert.AreEqual(true, viewModel.AnyJournalEntries);
            Assert.AreEqual(1, viewModel.Entries.Count());
            Assert.AreEqual(2, viewModel.Recipes.Count());

            (app.Journal as JournalViewModel).RemoveRecipeAsync(app.Journal.BaseRecipes.First()).Wait();

            Assert.AreEqual(false, viewModel.AnyBaseRecipes);
            Assert.AreEqual(false, viewModel.AnyJournalEntries);
            Assert.AreEqual(0, viewModel.Entries.Count());
            Assert.AreEqual(0, viewModel.Recipes.Count());
        }

        private OverviewPageViewModel CreateTestOverviewPage(IMixologyApp app)
        {
            BaseRecipePageViewModel baseRecipe = new BaseRecipePageViewModel(app);
            (app.Journal as JournalViewModel).AddBaseRecipeAsync(baseRecipe).Wait();
            (app.Journal as JournalViewModel).AddEntryAsync(new NotesEntryViewModel(app)).Wait();
            HomemadeDrinkEntryViewModel modifiedRecipe = new HomemadeDrinkEntryViewModel(baseRecipe, app);
            (app.Journal as JournalViewModel).AddEntryAsync(modifiedRecipe).Wait();
            baseRecipe.AddModifiedRecipe(modifiedRecipe);
            OverviewPageViewModel viewModel = new OverviewPageViewModel(app);
            return viewModel;
        }
    }
}
