// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

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
            Name = name;
            Type = type;
            Value = value;
        }
    }
}

