using Newtonsoft.Json;
using System;

namespace MixologyJournalApp.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Unit
    {
        [JsonProperty("id")]
        public String Id
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public String Name
        {
            get;
            set;
        }

        public static Unit CreateEmpty()
        {
            Unit unit = new Unit
            {
                Id = "",
                Name = ""
            };
            return unit;
        }

        public Unit()
        {
        }

        public Unit Clone()
        {
            Unit clone = new Unit
            {
                Id = Id,
                Name = Name
            };
            return clone;
        }
    }
}
