﻿﻿using System;
using AppKit;
using Foundation;

namespace MixologyJournal.OSX.View
{
	[Register("SourceListView")]
	public class SourceListView : NSOutlineView
	{
		#region Computed Properties
		public SourceListDataSource Data
		{
			get { return (SourceListDataSource)DataSource; }
		}
		#endregion

		#region Constructors
		public SourceListView()
		{
		}

		public SourceListView(IntPtr handle) : base(handle)
		{

		}

		public SourceListView(NSCoder coder) : base(coder)
		{

		}

		public SourceListView(NSObjectFlag t) : base(t)
		{

		}
		#endregion

		#region Override Methods
		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}
		#endregion

		#region Public Methods
		public void Initialize()
		{
			// Initialize this instance
			DataSource = new SourceListDataSource(this);
			Delegate = new SourceListDelegate(this);

		}

		public void AddItem(SourceListItem item)
		{
			if (Data != null)
			{
				Data.Items.Add(item);
			}
		}
		#endregion

		#region Events
		public delegate void ItemSelectedDelegate(SourceListItem item);
		public event ItemSelectedDelegate ItemSelected;

        public override void CollapseItem(NSObject item)
        {
            base.CollapseItem(item);
            (item as SourceListItem).Collapse();
        }

        public override void CollapseItem(NSObject item, bool collapseChildren)
        {
            base.CollapseItem(item, collapseChildren);
			(item as SourceListItem).Collapse();
		}

        public override void ExpandItem(NSObject item)
        {
            base.ExpandItem(item);
			(item as SourceListItem).Expand();
		}

        public override void ExpandItem(NSObject item, bool expandChildren)
        {
            base.ExpandItem(item, expandChildren);
			(item as SourceListItem).Expand();
		}

		internal void RaiseItemSelected(SourceListItem item)
		{
			ItemSelected?.Invoke(item);
		}
		#endregion
	}
}