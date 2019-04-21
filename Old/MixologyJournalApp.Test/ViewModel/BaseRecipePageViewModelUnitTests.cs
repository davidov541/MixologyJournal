using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class BaseRecipePageViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
			BaseMixologyApp app = CreateTestApp();
            BaseRecipePageViewModel viewModel = new BaseRecipePageViewModel(app);

            Assert.AreEqual(null, viewModel.Favorite);
            Assert.IsFalse(viewModel.DerivedRecipes.Any());
            Assert.IsFalse(viewModel.DerivedRecipesWithoutFavorite.Any());
            Assert.IsFalse(viewModel.Busy);
        }

        /*
        [TestMethod]
        public void NormalConstructorTest()
        {
			BaseMixologyApp app = CreateTestViewModel();
            BaseRecipePageViewModel viewModel = app.Journal.BaseRecipes.First() as BaseRecipePageViewModel;

            HomemadeDrinkViewModel modifiedVM = app.Journal.Entries.OfType<HomemadeDrinkEntryViewModel>().Last().Recipe;
            (viewModel.Recipe as BaseRecipeViewModel).Favorite = modifiedVM;

            Assert.AreEqual("Name", viewModel.Title);
            Assert.AreEqual(2, viewModel.DerivedRecipes.Count());
            Assert.AreEqual(1, viewModel.DerivedRecipesWithoutFavorite.Count());
            Assert.IsFalse(viewModel.Busy);
        }
        
        [TestMethod]
        public void RecipePropertyChangedTest()
        {
			BaseMixologyApp app = CreateTestViewModel();
            BaseRecipePageViewModel viewModel = app.Journal.BaseRecipes.First() as BaseRecipePageViewModel;

            bool sawFavorite = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("Favorite"))
                {
                    sawFavorite = true;
                }
            };

            HomemadeDrinkViewModel modified = new HomemadeDrinkViewModel((viewModel.Recipe as BaseRecipeViewModel).Model.Clone(), viewModel.Recipe as BaseRecipeViewModel, app);
            (viewModel.Recipe as BaseRecipeViewModel).FavoriteRecipe = modified;

            Assert.IsTrue(sawFavorite);
        }
        */

        [TestMethod]
        public void AddIngredientTest()
        {
			BaseMixologyApp app = CreateTestViewModel();

            BaseRecipePageViewModel viewModel = app.Journal.BaseRecipes.First() as BaseRecipePageViewModel;
            Assert.AreEqual(0, (viewModel.Recipe as BaseRecipeViewModel).Ingredients.Count());

            viewModel.AddIngredient();

            Assert.AreEqual(1, (viewModel.Recipe as BaseRecipeViewModel).Ingredients.Count());
        }

        [TestMethod]
        public void SaveNewTest()
        {
            StartTimingTest();

			BaseMixologyApp app = CreateTestApp();
            BaseRecipePageViewModel viewModel = new BaseRecipePageViewModel(app);

            Mock.Get(app.Persister).Setup(p => p.SaveAsync(app.Journal)).Returns(WaitForSaveTest());

            viewModel.SaveAsync().Wait();
            Assert.IsTrue(viewModel.Busy);
            FinishTimingTest();
            Thread.Sleep(250);
            Assert.IsFalse(viewModel.Busy);
        }

        [TestMethod]
        public void DeleteEntryTest()
        {
            StartTimingTest();

			BaseMixologyApp app = CreateTestViewModel();

            Mock.Get(app.Persister).Setup(p => p.SaveAsync(app.Journal)).Returns(WaitForSaveTest());

            IBaseRecipePageViewModel viewModel = app.Journal.BaseRecipes.First();
            viewModel.DeleteAsync();
            Assert.IsTrue(viewModel.Busy);
            FinishTimingTest();
            Thread.Sleep(250);
            Assert.IsFalse(viewModel.Busy);
        }

        private BaseMixologyApp CreateTestViewModel()
        {
			BaseMixologyApp app = CreateTestApp();
            BaseRecipe recipeModel = new BaseRecipe("Name", "Instructions");
            BaseRecipeViewModel recipe = new BaseRecipeViewModel(recipeModel, app);
            HomemadeDrink modifiedOne = new HomemadeDrink(recipeModel);
            HomemadeDrink modifiedTwo = new HomemadeDrink(recipeModel);
            BaseRecipePageViewModel viewModel = new BaseRecipePageViewModel(recipe, app);
            viewModel.AddModifiedRecipe(new HomemadeDrinkEntryViewModel(new HomemadeDrinkEntry(modifiedOne, string.Empty, DateTime.Now), viewModel, app));
            viewModel.AddModifiedRecipe(new HomemadeDrinkEntryViewModel(new HomemadeDrinkEntry(modifiedTwo, string.Empty, DateTime.Now), viewModel, app));
            (app.Journal as JournalViewModel).AddBaseRecipeAsync(viewModel).Wait();
            return app;
        }
    }
}
