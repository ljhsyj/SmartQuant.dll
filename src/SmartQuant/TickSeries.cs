// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;
using System.Collections;
using System;

namespace SmartQuant
{
    public class TickSeries : IDataSeries, IEnumerable<Tick>
    {
        private List<Tick> ticks;
        private Tick min;
        private Tick max;

        public string Name { get; private set; }

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
            throw new NotImplementedException();
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
            Tick max =  null;
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

        // TODO: Rewrite it!!!
        public int GetIndex(DateTime datetime, IndexOption option)
        {
            int index = 0;
            int num1 = 0;
            int num2 = this.ticks.Count - 1;
            bool flag = true;
            while (flag)
            {
                if (num2 < num1)
                    return -1;
                index = (num1 + num2) / 2;
                switch (option)
                {
                    case IndexOption.Null:
                        if (this.ticks[index].DateTime == datetime)
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.ticks[index].DateTime > datetime)
                        {
                            num2 = index - 1;
                            continue;
                        }
                        else if (this.ticks[index].DateTime < datetime)
                        {
                            num1 = index + 1;
                            continue;
                        }
                        else
                            continue;
                    case IndexOption.Next:
                        if (this.ticks[index].DateTime >= datetime && (index == 0 || this.ticks[index - 1].DateTime < datetime))
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.ticks[index].DateTime < datetime)
                        {
                            num1 = index + 1;
                            continue;
                        }
                        else
                        {
                            num2 = index - 1;
                            continue;
                        }
                    case IndexOption.Prev:
                        if (this.ticks[index].DateTime <= datetime && (index == this.ticks.Count - 1 || this.ticks[index + 1].DateTime > datetime))
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.ticks[index].DateTime > datetime)
                        {
                            num2 = index - 1;
                            continue;
                        }
                        else
                        {
                            num1 = index + 1;
                            continue;
                        }
                    default:
                        continue;
                }
            }
            return index;
        }

        public void Clear()
        {
            this.ticks.Clear();
            this.min = this.max = null;
        }

        private void EnsureNotEmpty()
        {
            if (this.Count <= 0)
                throw new ApplicationException("Array has no elements");
        }
    }
}
