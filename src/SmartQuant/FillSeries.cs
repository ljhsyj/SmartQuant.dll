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
        private List<Fill> fills = new List<Fill>();
        private Fill min;
        private Fill max; 

        public int Count
        {
            get
            {
                return this.fills.Count;
            }
        }

        public Fill Min
        {
            get
            {
                return this.min;
            }
        }

        public Fill Max
        {
            get
            {
                return this.max;
            }
        }

        public Fill this[int index]
        {
            get
            {
                return this.fills[index];
            }
        }
            
        public FillSeries(string name = "")
        {
            this.name = name;
        }

        public void Clear()
        {
            this.fills.Clear();
            this.min = this.max = null;
        }

        public void Add(Fill fill)
        {
            this.max = this.max == null ? fill : this.max.Price < fill.Price ? fill : this.max;
            this.min = this.min == null ? fill : this.min.Price > fill.Price ? fill : this.min;

            if (this.fills.Count != 0 && fill.DateTime < this.fills[this.fills.Count - 1].DateTime)
                Console.WriteLine("FillSeries::Add {0} + incorrect fill order : {1}", this.name, fill);
            this.fills.Add(fill);
        }

        public IEnumerator<Fill> GetEnumerator()
        {
            return this.fills.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.fills.GetEnumerator();
        }

        public int GetIndex(DateTime datetime, IndexOption option)
        {
            var firstDateTime = this[0].DateTime;
            var lastDateTime = this[Count - 1].DateTime;
            if (datetime < firstDateTime)
                return option == IndexOption.Null || option == IndexOption.Prev ? -1 : 0;
            if (datetime > lastDateTime)
                return option == IndexOption.Null || option == IndexOption.Next ? -1 : Count - 1;
            var i = this.fills.BinarySearch(new Fill() { DateTime = datetime }, new DataObjectComparer());
            if (i >= 0)
                return i;
            else if (option == IndexOption.Next)
                return ~i;
            else if (option == IndexOption.Prev)
                return ~i - 1;
            return -1; // option == IndexOption.Null
        }
    }
}
