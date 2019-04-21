using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MixologyJournalApp.ViewModel
{
    public class ResourceLoader
    {
        private Dictionary<String, String> _strings = new Dictionary<String, String>();

        public ResourceLoader(String name) : this(XDocument.Load(Path.Combine(name, CultureInfo.CurrentCulture.ToString(), "Resources.resx")))
        {
        }

        public ResourceLoader(Stream stream) : this(XDocument.Load(stream))
        {
        }

        private ResourceLoader(XDocument resourceFile) 
        {
            IEnumerable<XElement> dataNodes = resourceFile.Root.Elements(XName.Get("data"));
            foreach (XElement dataNode in dataNodes)
            {
                String key = dataNode.Attribute(XName.Get("name")).Value;
                String val = dataNode.Descendants(XName.Get("value")).First().Value;
                _strings[key] = val;
            }
        }

        public String GetString(String key)
        {
            return _strings[key];
        }
}

}
