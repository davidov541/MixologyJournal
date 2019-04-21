using AppKit;
using Foundation;
using MixologyJournal.OSX.View;
using MixologyJournal.OSX.ViewModel;

namespace MixologyJournal.OSX
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        MainWindowController mainWindowController;
		private MixologyApp _app = new MixologyApp();

		public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            mainWindowController = new MainWindowController(_app);
            mainWindowController.Window.MakeKeyAndOrderFront(this);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
