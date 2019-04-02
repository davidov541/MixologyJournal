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
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		AppKit.NSStackView FirstView { get; set; }

		[Outlet]
		MixologyJournal.OSX.View.RecipeView RecipeView { get; set; }

		[Outlet]
		AppKit.NSStackView SecondView { get; set; }

		[Outlet]
		MixologyJournal.OSX.View.SourceListView SourceList { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (FirstView != null) {
				FirstView.Dispose ();
				FirstView = null;
			}

			if (SecondView != null) {
				SecondView.Dispose ();
				SecondView = null;
			}

			if (SourceList != null) {
				SourceList.Dispose ();
				SourceList = null;
			}

			if (RecipeView != null) {
				RecipeView.Dispose ();
				RecipeView = null;
			}
		}
	}
}
