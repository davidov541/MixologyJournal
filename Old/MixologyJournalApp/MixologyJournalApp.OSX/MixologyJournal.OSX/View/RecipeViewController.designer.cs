// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MixologyJournal.OSX.View
{
	[Register ("RecipeViewController")]
	partial class RecipeViewController
	{
		[Outlet]
		AppKit.NSTextView IngredientsText { get; set; }

		[Outlet]
		AppKit.NSTextView InstructionsText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (IngredientsText != null) {
				IngredientsText.Dispose ();
				IngredientsText = null;
			}

			if (InstructionsText != null) {
				InstructionsText.Dispose ();
				InstructionsText = null;
			}
		}
	}
}
