﻿using MixologyJournalApp.Platform;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MixologyJournalApp.ViewModel
{
    internal class SetupPageItem
    {
        internal enum ItemType
        {
            Login,
            Description,
            Image
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

        public ImageSource ImageSource
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

        public SetupPageItem(String caption, ImageSource imageSource)
        {
            Caption = caption;
            Type = ItemType.Image;
            ImageSource = imageSource;
        }
    }
}
