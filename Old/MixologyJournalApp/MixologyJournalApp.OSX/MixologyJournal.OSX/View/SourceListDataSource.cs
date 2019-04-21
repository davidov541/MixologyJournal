﻿﻿﻿using System;
using System.Collections.Generic;
using AppKit;
using Foundation;

namespace MixologyJournal.OSX.View
{
	public class SourceListDataSource : NSOutlineViewDataSource
	{
		#region Private Variables
		private SourceListView _controller;
		#endregion

		#region Public Variables
		public List<SourceListItem> Items = new List<SourceListItem>();
		#endregion

		#region Constructors
		public SourceListDataSource(SourceListView controller)
		{
			// Initialize
			_controller = controller;
		}
		#endregion

		#region Override Properties
		public override nint GetChildrenCount(NSOutlineView outlineView, NSObject item)
		{
			if (item == null)
			{
				return Items.Count;
			}
			else
			{
				return ((SourceListItem)item).Count;
			}
		}

		public override bool ItemExpandable(NSOutlineView outlineView, NSObject item)
		{
			return ((SourceListItem)item).HasChildren;
		}

		public override NSObject GetChild(NSOutlineView outlineView, nint childIndex, NSObject item)
		{
			if (item == null)
			{
				return Items[(int)childIndex];
			}
			else
			{
				return ((SourceListItem)item)[(int)childIndex];
			}
		}

		public override NSObject GetObjectValue(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
		{
			return new NSString(((SourceListItem)item).Title);
		}
		#endregion

		#region Internal Methods
		internal SourceListItem ItemForRow(int row)
		{
			int index = 0;

            Stack<SourceListItem> itemsToProcess = new Stack<SourceListItem>();
            // Put the items in reverse so that the top item is the first one on the stack.
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                itemsToProcess.Push(Items[i]);
            }

            while (itemsToProcess.Count > 0)
            {
                SourceListItem item = itemsToProcess.Pop();
                if (row == index)
                {
                    return item;
                }
                if (item.IsExpanded)
                {
                    for (int i = item.Count - 1; i >= 0; i--)
                    {
                        itemsToProcess.Push(item[i]);
                    }
				}
                index++;
            }

			// Not found 
			return null;
		}
		#endregion
	}
}