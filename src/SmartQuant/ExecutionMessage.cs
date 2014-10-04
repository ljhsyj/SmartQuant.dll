// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class ExecutionMessage : DataObject
    {
        public int Id { get; private set; }

        public Instrument Instrument { get; set; }

        public Order Order { get; set; }

        public int OrderId { get; set; }

        public object this [int index]
        { 
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
