using System;
using MixologyJournal.ViewModel.List;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MixologyJournal.View
{
    public class JournalListDataTemplateSelector : DataTemplateSelector
    {
        public JournalListDataTemplateSelector()
        {
        }

        protected override DataTemplate SelectTemplateCore(Object item)
        {
            return base.SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(Object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null)
            {
                IListEntry entry = item as IListEntry;
                if (entry.Icon.HasValue)
                {
                    return element.FindName("JournalEntry_Icon") as DataTemplate;
                }
                if (item is IListEntryHeader)
                {
                    return element.FindName("JournalEntry_Date") as DataTemplate;
                }
                else if (!String.IsNullOrEmpty(entry.Caption))
                {
                    return element.FindName("JournalEntry_Entry") as DataTemplate;
                }
                else
                {
                    return element.FindName("JournalEntry_SingleLineEntry") as DataTemplate;
                }
            }
            return null;
        }
    }
}
