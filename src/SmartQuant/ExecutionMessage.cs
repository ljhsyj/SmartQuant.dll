// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class ExecutionMessage : DataObject
    {
        public int Id { get; internal set; }

        public Order Order { get; set; }

        public int OrderId { get; set; }

        internal int InstrumentId { get; set; }

        public Instrument Instrument { get; set; }

        internal ObjectTable Fields { get; set; }

        public object this [int index]
        { 
            get
            {
                return  Fields != null ? Fields[index] : null;
            }
            set
            {
                if (Fields == null)
                    Fields = new ObjectTable();
                Fields[index] = value;
            }
        }
    }
}
