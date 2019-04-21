using System;
using Foundation;
using AppKit;
using System.ComponentModel;

namespace MixologyJournal.OSX.View
{
    [DesignTimeVisible(true)]
    public partial class RecipeView : NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public RecipeView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public RecipeView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

		#endregion
    }
}
