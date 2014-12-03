// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class FieldList
    {
        internal IdArray<double> Fields { get; set; }

        public FieldList()
        {
            Fields = new IdArray<double>(1024);
        }
    }
}

