// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class QuoteSeries : IEnumerable<Quote>    
    {
        private string name;
        public QuoteSeries(string name = "")       
        {
            this.name = name;
        }
            
        public IEnumerator<Quote> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

