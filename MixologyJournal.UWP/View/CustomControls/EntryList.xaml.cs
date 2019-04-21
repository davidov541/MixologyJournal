using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MixologyJournal.ViewModel;
using MixologyJournal.ViewModel.List;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MixologyJournal.View
{
    public sealed partial class EntryList : UserControl, INotifyPropertyChanged
    {
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool AnyEntries
        {
            get
            {
                return ItemsSource.Any();
            }
        }

        public IEnumerable<IListEntry> ItemsSource
        {
            get { return (IEnumerable<IListEntry>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<IListEntry>), typeof(EntryList),
                new PropertyMetadata(new List<IListEntry>(), ItemsSourceChanged));

        public static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryList list = d as EntryList;
            list.OnPropertyChanged(nameof(AnyEntries));
            list.OnPropertyChanged(nameof(ItemsToShow));
        }

        private bool _showAllItems = true;
        public IEnumerable<IListEntry> ItemsToShow
        {
            get
            {
                if (_showAllItems)
                {
                    return ItemsSource;
                }
                else
                {
                    return ItemsSource.OfType<IListEntryHeader>();
                }
            }
        }

        public IListEntry SelectedItem
        {
            get;
            private set;
        }

        public EntryList()
        {
            this.InitializeComponent();
        }

        private void ListView_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            ListView list = sender as ListView;
            IListEntry entry = list.SelectedItem as IListEntry;
            if (entry == null)
            {
                // Do nothing since we are just removing the selection.
            }
            else if (entry is IListEntryHeader)
            {
                _showAllItems = !_showAllItems;
                OnPropertyChanged(nameof(ItemsToShow));
                list.ScrollIntoView(entry, ScrollIntoViewAlignment.Leading);
            }
            else
            {
                SelectedItem = entry;
                SelectionChanged?.Invoke(this, e);
            }
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
