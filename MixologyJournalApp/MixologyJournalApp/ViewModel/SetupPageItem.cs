using MixologyJournalApp.Platform;
using System;
using System.Collections.Generic;

namespace MixologyJournalApp.ViewModel
{
    internal class SetupPageItem
    {
        internal enum ItemType
        {
            Login,
            Description
        }

        public String Caption
        {
            get;
            private set;
        }

        public ItemType Type
        {
            get;
            private set;
        }

        public IEnumerable<ILoginMethod> LoginMethods
        {
            get;
            private set;
        }

        public SetupPageItem(String caption)
        {
            Caption = caption;
            Type = ItemType.Description;
        }

        public SetupPageItem(String caption, IEnumerable<ILoginMethod> loginMethods)
        {
            Caption = caption;
            Type = ItemType.Login;
            LoginMethods = loginMethods;
        }
    }
}
