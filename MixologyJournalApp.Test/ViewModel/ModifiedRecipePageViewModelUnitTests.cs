using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.SourceModel.Recipe;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;
using Moq;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class ModifiedRecipePageViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void DefaultConstructor()
        {
            IMixologyApp app = CreateTestApp();

            (app.Journal as JournalViewModel).AddBaseRecipeAsync(new BaseRecipePageViewModel(app)).Wait();
            BaseRecipePageViewModel baseRecipe = new BaseRecipePageViewModel(app);
            HomemadeDrinkEntryViewModel vm = new HomemadeDrinkEntryViewModel(baseRecipe, app);

            Assert.IsFalse(vm.Busy);
            Assert.AreEqual("No Changes", vm.Caption);
            Assert.IsTrue(DateTime.Now.Subtract(vm.CreationDate).TotalMilliseconds < 1000);
            Assert.AreEqual(null, vm.Icon);
            Assert.IsFalse(vm.IsFavorite);
            Assert.AreEqual(String.Empty, vm.Notes);
            Assert.AreEqual(0, vm.Pictures.Count);
            Assert.AreEqual(0.0, vm.Rating);
            Assert.AreEqual(0, vm.SiblingRecipes.Count());
            Assert.AreEqual(String.Empty, vm.Title);
            Assert.AreEqual(1, vm.ServingsNumber);
        }

        [TestMethod]
        public void NormalConstructor()
        {
            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            Assert.IsFalse(vm.Busy);
            Assert.AreEqual("No Changes", vm.Caption);
            Assert.AreEqual(createdCreationTime, vm.CreationDate);
            Assert.AreEqual(null, vm.Icon);
            Assert.IsFalse(vm.IsFavorite);
            Assert.AreEqual(createdContents, vm.Notes);
            Assert.AreEqual(0, vm.Pictures.Count);
            Assert.AreEqual(0.0, vm.Rating);
            Assert.AreEqual(1, vm.SiblingRecipes.Count());
            Assert.AreEqual(createdName, vm.Title);
            Assert.AreEqual(1, vm.ServingsNumber);
        }

        [TestMethod]
        public void SaveSetsBusyTest()
        {
            StartTimingTest();
            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            Assert.IsFalse(vm.Busy);

            Mock.Get(app.Persister).Setup(p => p.SaveAsync(app.Journal)).Returns(WaitForSaveTest());

            vm.SaveAsync();

            Assert.IsTrue(vm.Busy);

            FinishTimingTest();

            Thread.Sleep(250);

            Assert.IsFalse(vm.Busy);
        }

        [TestMethod]
        public void SaveTest()
        {
            StartTimingTest();

            String title = "Title";
            String notes = "Notes";
            String picture = "Picture";
            double rating = 2.5;

            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            vm.Title = title;
            vm.Notes = notes;
            vm.Rating = rating;
            FinishTimingTest();
            Mock.Get(app).Setup(p => p.GetNewPictureAsync()).Returns(GetPictureTaskTest());
            vm.AddNewPictureAsync().Wait();

            Assert.AreEqual(createdName, vm.Model.Title);
            Assert.AreEqual(createdContents, vm.Model.Notes);
            Assert.AreEqual(0.0, (vm.Model as HomemadeDrinkEntry).Rating);
            Assert.AreEqual(0, vm.Model.Pictures.Count());
            Assert.AreEqual(title, vm.Title);
            Assert.AreEqual(notes, vm.Notes);
            Assert.AreEqual(rating, vm.Rating);
            Assert.AreEqual(1, vm.Pictures.Count());
            Assert.AreEqual(picture, vm.Pictures[0]);

            vm.SaveAsync().Wait();

            Assert.AreEqual(title, vm.Model.Title);
            Assert.AreEqual(notes, vm.Model.Notes);
            Assert.AreEqual(rating, (vm.Model as HomemadeDrinkEntry).Rating);
            Assert.AreEqual(1, vm.Model.Pictures.Count());
            Assert.AreEqual(picture, vm.Model.Pictures.ElementAt(0));
            Assert.AreEqual(title, vm.Title);
            Assert.AreEqual(notes, vm.Notes);
            Assert.AreEqual(rating, vm.Rating);
            Assert.AreEqual(1, vm.Pictures.Count());
            Assert.AreEqual(picture, vm.Pictures[0]);
        }

        [TestMethod]
        public void CancelTest()
        {
            StartTimingTest();

            String title = "Title";
            String notes = "Notes";
            String picture = "Picture";
            double rating = 2.5;

            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            vm.Title = title;
            vm.Notes = notes;
            vm.Rating = rating;
            FinishTimingTest();
            Mock.Get(app).Setup(p => p.GetNewPictureAsync()).Returns(GetPictureTaskTest());
            vm.AddNewPictureAsync().Wait();

            Assert.AreEqual(createdName, vm.Model.Title);
            Assert.AreEqual(createdContents, vm.Model.Notes);
            Assert.AreEqual(0.0, (vm.Model as HomemadeDrinkEntry).Rating);
            Assert.AreEqual(0, vm.Model.Pictures.Count());
            Assert.AreEqual(title, vm.Title);
            Assert.AreEqual(notes, vm.Notes);
            Assert.AreEqual(rating, vm.Rating);
            Assert.AreEqual(1, vm.Pictures.Count());
            Assert.AreEqual(picture, vm.Pictures[0]);

            vm.Cancel();

            Assert.AreEqual(createdName, vm.Title);
            Assert.AreEqual(createdContents, vm.Notes);
            Assert.AreEqual(0.0, vm.Rating);
            Assert.AreEqual(0, vm.Pictures.Count());
            Assert.AreEqual(createdName, vm.Model.Title);
            Assert.AreEqual(createdContents, vm.Model.Notes);
            Assert.AreEqual(0.0, (vm.Model as HomemadeDrinkEntry).Rating);
            Assert.AreEqual(0, vm.Model.Pictures.Count());
        }

        [TestMethod]
        public void AddIngredientTest()
        {
            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            Assert.AreEqual(0, vm.Recipe.Ingredients.Count());
            vm.AddIngredient();
            Assert.AreEqual(1, vm.Recipe.Ingredients.Count());
        }

        [TestMethod]
        public void ServingsNumberTest()
        {
            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            Assert.AreEqual(1, vm.ServingsNumber);

            bool servingsNumberChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(HomemadeDrinkEntryViewModel.ServingsNumber)))
                {
                    servingsNumberChanged = true;
                }
            };

            vm.ServingsNumber = 2;

            Assert.IsTrue(servingsNumberChanged);
        }

        [TestMethod]
        public void NotesTest()
        {
            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            bool notesChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(HomemadeDrinkEntryViewModel.Notes)))
                {
                    notesChanged = true;
                }
            };

            vm.Notes = "NewNotes";

            Assert.IsTrue(notesChanged);
        }

        [TestMethod]
        public void TitleTest()
        {
            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            bool titleChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(HomemadeDrinkEntryViewModel.Title)))
                {
                    titleChanged = true;
                }
            };

            vm.Title = "NewTitle";

            Assert.IsTrue(titleChanged);
        }

        [TestMethod]
        public void RatingTest()
        {
            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            bool ratingChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(HomemadeDrinkEntryViewModel.Rating)))
                {
                    ratingChanged = true;
                }
            };

            vm.Rating = 2.5;

            Assert.IsTrue(ratingChanged);
        }

        [TestMethod]
        public void AddNewPictureSetsBusyTest()
        {
            StartTimingTest();

            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            Assert.IsFalse(vm.Busy);

            Mock.Get(app).Setup(p => p.GetNewPictureAsync()).Returns(GetPictureTaskTest());

            vm.AddNewPictureAsync();

            Assert.IsTrue(vm.Busy);

            FinishTimingTest();

            Thread.Sleep(250);

            Assert.IsFalse(vm.Busy);
            Assert.AreEqual(1, vm.Pictures.Count());
            Assert.AreEqual("Picture", vm.Pictures[0]);
        }

        [TestMethod]
        public void AddExistingPictureSetsBusyTest()
        {
            StartTimingTest();

            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            Assert.IsFalse(vm.Busy);

            Mock.Get(app).Setup(p => p.GetExistingPictureAsync(It.IsAny<UInt64>())).Returns(GetPictureTaskTest());

            vm.AddExistingPictureAsync();

            Assert.IsTrue(vm.Busy);

            FinishTimingTest();

            Thread.Sleep(250);

            Assert.IsFalse(vm.Busy);
            Assert.AreEqual(1, vm.Pictures.Count());
            Assert.AreEqual("Picture", vm.Pictures[0]);
        }

        [TestMethod]
        public void FavoriteTest()
        {
            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            Assert.IsFalse(vm.IsFavorite);
            Assert.IsNull(vm.Icon);

            vm.IsFavorite = true;

            Assert.IsTrue(vm.IsFavorite);
            Assert.AreEqual(Symbol.SolidStar, vm.Icon.Value);

            HomemadeDrinkEntryViewModel otherViewModel = (app.Journal.BaseRecipes.First() as BaseRecipePageViewModel).DerivedRecipesWithoutFavorite.First();

            otherViewModel.IsFavorite = true;

            Assert.IsFalse(vm.IsFavorite);
            Assert.IsNull(vm.Icon);
        }

        [TestMethod]
        public void DeleteSetsBusyTest()
        {
            StartTimingTest();
            IMixologyApp app = CreateTestApp();
            HomemadeDrinkEntryViewModel vm = CreateModifiedRecipePageViewModel(app);

            Assert.IsFalse(vm.Busy);

            Mock.Get(app.Persister).Setup(p => p.SaveAsync(app.Journal)).Returns(WaitForSaveTest());

            vm.DeleteAsync();

            Assert.IsTrue(vm.Busy);

            FinishTimingTest();

            Thread.Sleep(250);

            Assert.IsFalse(vm.Busy);
        }

        private const String createdName = "Name";
        private const String createdInstructions = "Instructions";
        private const String createdContents = "Contents";
        private readonly DateTime createdCreationTime = new DateTime(1000);

        private HomemadeDrinkEntryViewModel CreateModifiedRecipePageViewModel(IMixologyApp app)
        {
            BaseRecipe baseRecipe = new BaseRecipe(createdName, createdInstructions);
            HomemadeDrink recipe = new HomemadeDrink(baseRecipe);
            HomemadeDrinkEntry model = new HomemadeDrinkEntry(recipe, createdContents, createdCreationTime);
            BaseRecipePageViewModel basePage = new BaseRecipePageViewModel(app);
            (app.Journal as JournalViewModel).AddBaseRecipeAsync(basePage).Wait();
            basePage.AddModifiedRecipe(new HomemadeDrinkEntryViewModel(model, basePage, app));
            basePage.AddModifiedRecipe(new HomemadeDrinkEntryViewModel(new HomemadeDrinkEntry(new HomemadeDrink(baseRecipe, 100), "OtherContents", createdCreationTime), basePage, app));
            HomemadeDrinkEntryViewModel vm = new HomemadeDrinkEntryViewModel(model, basePage, app);
            (app.Journal as JournalViewModel).AddEntryAsync(vm).Wait();
            return vm;
        }
    }
}
