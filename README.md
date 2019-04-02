This repo contains the code for the Mixology Journal app. This app allows users
to track cocktails they have created and tried, as well as take notes about
mixing drinks. The app currently only works on Windows 10 for desktops, laptops,
and phones, but is made with Xamarin so that it can be made to work with 
Android and iOS as well.

The MixologyJournal/MixologyJournal.sln Visual Studio solution has the following
projects in it:

MixologyJournalApp: This library contains all of the common code between all of
the platforms, including source model and view model, as well as generic
persistence mechanisms.
MixologyJournalApp.Droid: The Android specific application. This portion 
contains only the view code for the Android app.
MixologyJournalApp.Droid: The iOS specific application. This portion 
contains only the view code for the iOS app. In order to build this application,
you need access to a Mac.
MixologyJournalApp.Test: Unit tests for the MixologyJournalApp library. This is
intended to be platform independent and make sure we don't break underlying
code when refactoring.
MixologyJournalApp.UWP: The Universal Windows Platform (UWP) app. This app is
meant to work on Windows 10 for laptops, tablets and phones, and contains only
the view code.