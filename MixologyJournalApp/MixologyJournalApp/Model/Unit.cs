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

        [JsonProperty("plural")]
        public String Plural
        {
            get;
            set;
        }

        [JsonProperty("format")]
        public String Format
        {
            get;
            set;
        }
      
        public static Unit CreateEmpty()
        {
            Unit unit = new Unit
            {
                Id = "",
                Name = "",
                Plural = "",
                Format = "{0} {1} of {2}"
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
                Name = Name,
                Plural = Plural,
                Format = Format
            };
            return clone;
        }
    }
}
