This repo contains the code for the Mixology Journal app. This app allows users
to track cocktails they have created and tried, as well as take notes about
mixing drinks. The app currently only works on Android, but is built with 
Xamarin Forms in order to allow for cross-platform support later.

The MixologyJournal.sln Visual Studio solution has the following
projects in it:

* MixologyJournalApp: This library contains all of the common code between all of
the platforms, including source model and view model, as well as generic
persistence mechanisms.
* MixologyJournalApp.Android: The Android specific application. This portion 
contains only the view code for the Android app.
