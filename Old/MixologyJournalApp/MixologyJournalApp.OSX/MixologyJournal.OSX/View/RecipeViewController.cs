using System;
using Foundation;
using AppKit;
using MixologyJournal.ViewModel.Entry;

namespace MixologyJournal.OSX.View
{
    public partial class RecipeViewController : NSViewController
	{
		//strongly typed view accessor
		public new RecipeView View
		{
			get
			{
				return (RecipeView)base.View;
			}
		}

        #region Constructors

        // Called when created from unmanaged code
        public RecipeViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public RecipeViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public RecipeViewController() : base("RecipeView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }
        #endregion

        public void Populate(IBaseRecipePageViewModel viewModel)
        {
            IngredientsText.Value = viewModel.Title;
            InstructionsText.Value = viewModel.Recipe.ToString();
        }
    }
}
