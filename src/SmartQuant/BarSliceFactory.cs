// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    class Slice
    {
        internal List<Bar> bars;
        DateTime dateTime;
    }

    public class BarSliceFactory
	{
        private Framework framework;
        private IdArray<Slice> slices;

        public BarSliceFactory(Framework framework)
        {
            this.framework = framework;
            this.slices = new IdArray<Slice>(86400);
        }

        internal void Clear()
        {
            slices.Clear();
        }
	}
}
