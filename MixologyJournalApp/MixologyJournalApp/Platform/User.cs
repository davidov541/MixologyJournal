using System;
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

        public String AuthToken
        {
            get;
            private set;
        }

        public User(String name, String authToken)
        {
            Name = name;
            AuthToken = authToken;
        }
    }
}
