using System;

namespace SmartQuant
{
    public class GroupField
    {
        public string Name { get; private set; }

        public byte Type { get; private set; }

        public object Value { get; set; }

        public GroupField(string name, byte type, object value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }
    }
}

