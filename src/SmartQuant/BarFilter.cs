// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;
using System.Linq;

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
            if (!Contains(barType, barSize))
                Items.Add(new BarFilterItem(barType, barSize));
        }

        public bool Contains(BarType barType, long barSize)
        {
            return Items.Any(item => item.BarType == barType && item.BarSize == barSize);
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}