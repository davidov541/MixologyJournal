﻿using Newtonsoft.Json;
using System;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Unit
    {
        [JsonProperty]
        public String Id
        {
            get;
            set;
        }

        [JsonProperty]
        public String Name
        {
            get;
            set;
        }

        public Unit()
        {
        }
    }
}
