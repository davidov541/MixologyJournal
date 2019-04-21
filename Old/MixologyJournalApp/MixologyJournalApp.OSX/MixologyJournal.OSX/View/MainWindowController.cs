﻿using System;
using System.Linq;

using Foundation;
using AppKit;
using MixologyJournal.OSX.ViewModel;
using MixologyJournal.ViewModel.Entry;
using MixologyJournal.ViewModel.List;
using System.Collections.Generic;

namespace MixologyJournal.OSX.View
{
    public partial class MainWindowController : NSWindowController
    {
        private IOverviewPageViewModel _viewModel;
        private MixologyApp _app;

        public MainWindowController(IntPtr handle) : base(handle)
        {
            SecondView.Hidden = true;
        }

        [Export("initWithCoder:")]
        public MainWindowController(NSCoder coder) : base(coder)
        {
		}

        internal MainWindowController(MixologyApp app) : base("MainWindow")
        {
            _app = app;
		}

        public override async void AwakeFromNib()
        {
            base.AwakeFromNib();
            await _app.InitializeAsync();
            _viewModel = _app.GetOverviewPage();
            _viewModel.MinimumEntriesPerGrouping = 10;
			PopulateSideBar();
		}

        private void PopulateSideBar()
        {
            SourceList.Initialize();

            // Add Recipes
            SourceListItem recipes = new SourceListItem("Recipes");
            bool recipesExist = PopulateHeaderedData(recipes, _viewModel.Recipes);
            SourceList.AddItem(recipes);

            // Add Drinks 
            var drinks = new SourceListItem("Drinks");
            bool drinksExist = PopulateHeaderedData(drinks, _viewModel.Entries.OfType<IDrinkEntryViewModel>());
            SourceList.AddItem(drinks);

            // Add Journal Entries
            var journalEntries = new SourceListItem("Journal Entries");
            bool entriesExist = PopulateHeaderedData(journalEntries, _viewModel.Entries.OfType<IJournalEntryViewModel>());
            SourceList.AddItem(journalEntries);

            // Display side list
            SourceList.ReloadData();
            if (recipesExist)
            {
				SourceList.ExpandItem(recipes, false);
			}
			if (drinksExist)
			{
				SourceList.ExpandItem(drinks, false);
			}
            if (entriesExist)
            {
                SourceList.ExpandItem(journalEntries, false);
            }
        }

        private bool PopulateHeaderedData(SourceListItem topHeader, IEnumerable<IListEntry> entries)
		{
			if (entries.Any())
			{
				SourceListItem lastHeader = topHeader;
				foreach (IListEntry entry in entries)
				{
					if (entry is IListEntryHeader)
					{
						lastHeader = new SourceListItem(entry.Title);
						topHeader.AddItem(lastHeader);
					}
                    else if (entry is IBaseRecipePageViewModel)
					{
                        SourceListItem item = new SourceListItem(entry.Title, String.Empty, () => {});
						lastHeader.AddItem(item);
					}
                    else
                    {
						SourceListItem item = new SourceListItem(entry.Title, String.Empty);
						lastHeader.AddItem(item);
                    }
				}
				return true;
			}
            else
            {
                // Add something underneath with an empty string so that it still shows up as a header.
                topHeader.AddItem(String.Empty);
                return false;
            }
        }

        public new MainWindow Window
        {
            get { return (MainWindow)base.Window; }
        }
    }
}
