// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;
using System.Collections;
using System;

namespace SmartQuant
{
    public class TickSeries : IDataSeries, IEnumerable<Tick>
    {
        // This list should be ordered by tick's DateTime.
        private List<Tick> ticks;
        private Tick min;
        private Tick max;

        public string Name { get; private set; }

        public string Description { get; private set; }

        public int Count
        {
            get
            {
                return this.ticks.Count;
            }
        }

        long IDataSeries.Count
        {
            get
            {
                return (long)this.ticks.Count;
            }
        }

        public DateTime DateTime1
        {
            get
            {
                return this.FirstDateTime;
            }
        }

        public DateTime DateTime2
        {
            get
            {
                return this.LastDateTime;
            }
        }

        public DateTime FirstDateTime
        {
            get
            {
                EnsureNotEmpty();
                return this.ticks[0].DateTime;
            }
        }

        public DateTime LastDateTime
        {
            get
            {
                EnsureNotEmpty();
                return this.ticks[this.Count - 1].DateTime;
            }
        }

        public Tick this [int index]
        {
            get
            {
                return this.ticks[index];
            }
        }

        DataObject IDataSeries.this [long index]
        {
            get
            {
                return this.ticks[(int)index];
            }
        }

        public TickSeries(string name = "")
        {
            Name = name;
            this.ticks = new List<Tick>();
        }

        public void Add(Tick tick)
        {
            this.min = (this.min == null || this.min.Price > tick.Price) ? tick : this.min;
            this.max = (this.max == null || this.max.Price < tick.Price) ? tick : this.max;
            this.ticks.Add(tick);
        }

        public Tick GetMin()
        {
            return this.min;
        }

        public Tick GetMax()
        {
            return this.max;
        }

        public IEnumerator<Tick> GetEnumerator()
        {
            return this.ticks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ticks.GetEnumerator();
        }

        public long GetIndex(DateTime dateTime, SearchOption option = SearchOption.Prev)
        {
            switch (option)
            {
                case SearchOption.Next:
                    return GetIndex(dateTime, IndexOption.Next);
                case SearchOption.Prev:
                    return GetIndex(dateTime, IndexOption.Prev);
                default:
                    throw new ApplicationException("Unsupported search option");
            }
        }

        // Assumption: dateTime1 <= dateTime2
        public Tick GetMin(DateTime dateTime1, DateTime dateTime2)
        {
            Tick min = null;
            for (int i = 0; i < this.ticks.Count; ++i)
            {
                var tick = this.ticks[i];
                if (tick.DateTime > dateTime2)
                    break;
                if (tick.DateTime < dateTime1)
                    continue;
                min = min == null ? tick : ((min != null && min.Price > tick.Price) ? tick : min);
            }
            return min;
        }

        public Tick GetMax(DateTime dateTime1, DateTime dateTime2)
        {
            Tick max = null;
            for (int i = 0; i < this.ticks.Count; ++i)
            {
                var tick = this.ticks[i];
                if (tick.DateTime > dateTime2)
                    break;
                if (tick.DateTime < dateTime1)
                    continue;
                max = max == null ? tick : ((max != null && max.Price < tick.Price) ? tick : max);
            }
            return max;
        }

        public int GetIndex(DateTime datetime, IndexOption option)
        {
            if (datetime < FirstDateTime)
                return option == IndexOption.Null || option == IndexOption.Prev ? -1 : 0;
            if (datetime > LastDateTime)
                return option == IndexOption.Null || option == IndexOption.Next ? -1 : Count - 1;

            var i = this.ticks.BinarySearch(new Tick() { DateTime = datetime }, new DataObjectComparer());
            if (i >= 0)
                return i;
            else if (option == IndexOption.Next)
                return ~i;
            else if (option == IndexOption.Prev)
                return ~i - 1;
            return -1; // option == IndexOption.Null
        }

        public BarSeries Compress(BarType barType, long barSize)
        {
            throw new NotImplementedException();
        }

        public bool Contains(DateTime dateTime)
        {
            return GetIndex(dateTime, IndexOption.Null) != -1;
        }

        void IDataSeries.Add(DataObject obj)
        {
            Add((Tick)obj);
        }

        void IDataSeries.Remove(long index)
        {
            this.ticks.RemoveAt((int)index);
        }

        public void Clear()
        {
            this.ticks.Clear();
            this.min = this.max = null;
        }

        private void EnsureNotEmpty()
        {
            if (Count <= 0)
                throw new ApplicationException("Array has no elements");
        }
    }
}
