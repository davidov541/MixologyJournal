using MixologyJournalApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MixologyJournalApp.View
{
    public class SetupItemDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate DescriptionTemplate { get; set; }
        public DataTemplate LoginTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (((SetupPageItem)item).Type)
            {
                case SetupPageItem.ItemType.Description:
                    return DescriptionTemplate;
                case SetupPageItem.ItemType.Login:
                    return LoginTemplate;
            }
            return null;
        }
    }
}
