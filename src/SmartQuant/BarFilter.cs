// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class BarFilter
    {
        public List<BarFilterItem> Items { get; private set; }

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public BarFilter()
        {
            Items = new List<BarFilterItem>();
        }

        public void Add(BarType barType, long barSize)
        {
            if (!this.Contains(barType, barSize))
                Items.Add(new BarFilterItem(barType, barSize));
        }

        public bool Contains(BarType barType, long barSize)
        {
            foreach (var item in Items)
            {
                if (item.BarType == barType && item.BarSize == barSize)
                    return true;
            }
            return false;
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}