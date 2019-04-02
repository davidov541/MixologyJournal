using System;
using System.Linq;
using System.Threading;
using GeoCoordinatePortable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixologyJournal.ViewModel.App;
using MixologyJournal.ViewModel.Entry;
using MixologyJournalApp.ViewModel.LocationServices;
using Moq;

namespace MixologyJournalApp.Test.ViewModel
{
    [TestClass]
    public class BarDrinkPageViewModelUnitTests : BaseViewModelUnitTests
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            IMixologyApp app = CreateTestApp();
            BarDrinkEntryViewModel vm = new BarDrinkEntryViewModel(app);

            Assert.IsFalse(vm.Busy);
            Assert.IsTrue(DateTime.Now.Subtract(vm.CreationDate).TotalMilliseconds < 1000);
            Assert.IsNotNull(vm.Model);
            Assert.AreEqual(0, vm.Pictures.Count());
            Assert.AreEqual(String.Empty, vm.Title);
        }

        [TestMethod]
        public void AddIngredientTest()
        {
            IMixologyApp app = CreateTestApp();
            BarDrinkEntryViewModel vm = new BarDrinkEntryViewModel(app);

            Assert.AreEqual(0, vm.Drink.Ingredients.Count());
            vm.AddIngredient();
            Assert.AreEqual(1, vm.Drink.Ingredients.Count());
        }

        [TestMethod]
        public void NotesTest()
        {
            IMixologyApp app = CreateTestApp();
            BarDrinkEntryViewModel vm = new BarDrinkEntryViewModel(app);

            bool notesChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(BarDrinkEntryViewModel.Notes)))
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
            BarDrinkEntryViewModel vm = new BarDrinkEntryViewModel(app);

            bool titleChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(BarDrinkEntryViewModel.Title)))
                {
                    titleChanged = true;
                }
            };

            vm.Title = "NewTitle";

            Assert.IsTrue(titleChanged);
        }

        [TestMethod]
        public void LocationTest()
        {
            IMixologyApp app = CreateTestApp();
            BarDrinkEntryViewModel vm = new BarDrinkEntryViewModel(app);

            bool locationChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(BarDrinkEntryViewModel.Location)))
                {
                    locationChanged = true;
                }
            };

            vm.Location = new NearbyPlace("Name" ,"Identifier", new GeoCoordinate(0, 0));

            Assert.IsTrue(locationChanged);
        }

        [TestMethod]
        public void RatingTest()
        {
            IMixologyApp app = CreateTestApp();
            BarDrinkEntryViewModel vm = new BarDrinkEntryViewModel(app);

            bool ratingChanged = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(BarDrinkEntryViewModel.Rating)))
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
            BarDrinkEntryViewModel vm = new BarDrinkEntryViewModel(app);

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
            BarDrinkEntryViewModel vm = new BarDrinkEntryViewModel(app);

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
    }
}
