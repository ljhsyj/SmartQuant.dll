// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class PortfolioList : IEnumerable<Portfolio>
    {
        private GetByList<Portfolio> list = new GetByList<Portfolio>();

        public int Count
        {
            get
            { 
                return this.list.Count;
            }
        }

        public Portfolio this [string name]
        { 
            get
            { 
                return GetByName(name);
            }
        }

        public PortfolioList()
        {
        }

        public void Add(Portfolio portfolio)
        {
            this.list.Add(portfolio);
        }

        public Portfolio GetByName(string name)
        {
            return this.list.GetByName(name);
        }

        public Portfolio GetByIndex(int index)
        {
            return this.list.GetByIndex(index);
        }

        public Portfolio GetById(int id)
        {
            return this.list.GetById(id);
        }

        internal void Remove(Portfolio portfolio)
        {
            this.list.Remove(portfolio);
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public IEnumerator<Portfolio> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator<Portfolio> IEnumerable<Portfolio>.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}