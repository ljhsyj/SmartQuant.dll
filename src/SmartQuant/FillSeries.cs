// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class FillSeries : IEnumerable<Fill>
    {
        private string name;

        public FillSeries(string name = "")
        {
            this.name = name;
        }

        public int GetIndex(DateTime datetime, IndexOption option)
        {
            throw new NotImplementedException();
        }

        public void Add(Fill fill)
        {
        }

        public void Clear()
        {
        }

        public IEnumerator<Fill> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
 
    }
}

