// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;
using System.Collections;
using System;

namespace SmartQuant
{
    public class PortfolioStatisticsItemList : IEnumerable<PortfolioStatisticsItem>
	{
        private GetByList<PortfolioStatisticsItem> items = new GetByList<PortfolioStatisticsItem>();

        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        public PortfolioStatisticsItem this[int index]
        {
            get
            {
                return this.items.GetByIndex(index);
            }
        }

        public bool Contains(int type)
        {
            return this.items.Contains(type);
        }

        public void Add(PortfolioStatisticsItem item)
        {
            this.items.Add(item);
        }

        public void Remove(int type)
        {
            this.items.Remove(type);
        }

        public PortfolioStatisticsItem GetByType(int type)
        {
            return this.items.GetById(type);
        }

        public PortfolioStatisticsItem GetByIndex(int index)
        {
            return this.items.GetByIndex(index);
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public IEnumerator<PortfolioStatisticsItem> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }
	}
}
