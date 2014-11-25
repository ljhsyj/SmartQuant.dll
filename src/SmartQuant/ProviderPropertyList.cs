// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;

namespace SmartQuant
{
    public class ProviderPropertyList
    {
        private Dictionary<string, string> properties  = new Dictionary<string, string>();

        public void SetValue(string name, string value)
        {
            this.properties[name] = value;
        }

        public string GetStringValue(string name, string defaultValue)
        {
            string s;
            return this.properties.TryGetValue(name, out s) ? s : defaultValue;
        }

        public ProviderPropertyList()
        {
        }

        internal ProviderPropertyList(IEnumerable<XmlProviderProperty> properties)
        {
            foreach (var p in properties)
                SetValue(p.Name, p.Value);
        }

        internal List<XmlProviderProperty> ToXmlProviderProperties()
        {
            var list = new List<XmlProviderProperty>();
            foreach (var p in properties)
            {
                XmlProviderProperty xml;
                xml.Name = p.Key;
                xml.Value = p.Value;
                list.Add(xml);
            }
            return list;
        }
    }
}
