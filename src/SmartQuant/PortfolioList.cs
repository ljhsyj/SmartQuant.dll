// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class PortfolioList : IEnumerable<Portfolio>
    {
        public int Count { get { throw new NotImplementedException(); } }

        public Portfolio this [string name]  { get { throw new NotImplementedException(); } }

        public PortfolioList()
        {
        }

        public void Add(Portfolio portfolio)
        {
        }

        public Portfolio GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Portfolio GetByIndex(int index)
        {
            throw new NotImplementedException();
        }

        public Portfolio GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
        }

        public IEnumerator<Portfolio> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
