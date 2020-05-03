﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MixologyJournalApp.Platform
{
    public class User
    {
        public String Name
        {
            get;
            private set;
        }

        public Uri IconPath
        {
            get;
            private set;
        }

        public String AuthToken
        {
            get;
            private set;
        }

        public User(String name, Uri iconPath, String authToken)
        {
            Name = name;
            IconPath = iconPath;
            AuthToken = authToken;
        }
    }
}
