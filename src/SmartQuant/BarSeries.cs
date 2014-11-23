// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;
using System.Collections.Generic;

namespace SmartQuant
{
    public class BarSeries : IDataSeries, ISeries
    {
        protected string name;
        protected string description;

        private List<Bar> bars;
        private int maxLength;
        private Bar min;
        private Bar max;

        public List<Indicator> Indicators { get; private set; }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        double ISeries.First
        {
            get
            {
                return First.Close;
            }
        }

        double ISeries.Last
        {
            get
            {
                return Last.Close;
            }
        }

        public Bar First
        {
            get
            {
                return this[0];
            }
        }

        public Bar Last
        {
            get
            {
                return this[Count - 1];
            }
        }

        public DateTime FirstDateTime
        {
            get
            {
                return First.DateTime;
            }
        }

        public DateTime LastDateTime
        {
            get
            {
                return Last.DateTime; 
            }
        }

        DataObject IDataSeries.this [long index]
        {
            get
            {
                return this[(int)index];
            }
        }

        public Bar this [int index]
        {
            get
            {
                EnsureNotEmpty();
                return this.bars[index];
            }
        }

        public Bar this [DateTime dateTime, IndexOption option = IndexOption.Null]
        {
            get
            {
                return this.bars[GetIndex(dateTime, option)];
            }
        }

        double ISeries.this [int index]
        {
            get
            {
                return this[index, BarData.Close];
            }
        }

        public double this [int index, BarData barData]
        {
            get
            {
                switch (barData)
                {
                    case BarData.Close:
                        return this.bars[index].Close;
                    case BarData.Open:
                        return this.bars[index].Open;
                    case BarData.High:
                        return this.bars[index].High;
                    case BarData.Low:
                        return this.bars[index].Low;
                    case BarData.Median:
                        return this.bars[index].Median;
                    case BarData.Typical:
                        return this.bars[index].Typical;
                    case BarData.Weighted:
                        return this.bars[index].Weighted;
                    case BarData.Average:
                        return this.bars[index].Average;
                    case BarData.Volume:
                        return  this.bars[index].Volume;
                    case BarData.OpenInt:
                        return  this.bars[index].OpenInt;
                    case BarData.Range:
                        return this.bars[index].Range;
                    case BarData.Mean:
                        return this.bars[index].Mean;
                    case BarData.Variance:
                        return this.bars[index].Variance;
                    case BarData.StdDev:
                        return this.bars[index].StdDev;
                    default:
                        throw new ArgumentException(string.Format("Unknown BarData value {0}", barData));
                }
            }
        }

        public int Count
        {
            get
            {
                return this.bars.Count;
            }
        }

        long IDataSeries.Count
        {
            get
            {
                return (long)this.bars.Count;
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

        public int MaxLength
        {
            get
            {
                return this.maxLength;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public BarSeries(int maxLength)
            : this(null, null, maxLength)
        {
        }

        public BarSeries(string name = "", string description = "", int maxLength = -1)
        {
            this.name = name;
            this.description = description;
            this.maxLength = maxLength;
            this.bars = new List<Bar>();
            Indicators = new List<Indicator>();
        }

        public int GetIndex(DateTime dateTime, IndexOption option = IndexOption.Null)
        {    
            if (dateTime < FirstDateTime)
                return option == IndexOption.Null || option == IndexOption.Prev ? -1 : 0;
            if (dateTime > LastDateTime)
                return option == IndexOption.Null || option == IndexOption.Next ? -1 : Count - 1;

            var i = this.bars.BinarySearch(new Bar() { DateTime = dateTime }, new DataObjectComparer());
            if (i >= 0)
                return i;
            else if (option == IndexOption.Next)
                return ~i;
            else if (option == IndexOption.Prev)
                return ~i - 1;
            return -1; // option == IndexOption.Null
        }

        public DateTime GetDateTime(int index)
        {
            return this.bars[index].DateTime;
        }

        public double GetMin(DateTime dateTime1, DateTime dateTime2)
        {
            throw new NotImplementedException();
        }

        public double GetMin(int index1, int index2, BarData barData)
        {
            throw new NotImplementedException();
        }

        public double GetMax(DateTime dateTime1, DateTime dateTime2)
        {
            throw new NotImplementedException();
        }

        public double GetMax(int index1, int index2, BarData barData)
        {
            throw new NotImplementedException();
        }

        long IDataSeries.GetIndex(DateTime dateTime, SearchOption option)
        {
            if (option != SearchOption.Next && option != SearchOption.Prev)
                throw new ApplicationException("Unsupported search option");
            return option == SearchOption.Next ? GetIndex(dateTime, IndexOption.Next) : GetIndex(dateTime, IndexOption.Prev);
        }

        public void Add(Bar bar)
        {
//            if (this.bar_0 == null)
//                this.bar_0 = bar;
//            else if (bar.double_0 < this.bar_0.double_1)
//                this.bar_0 = bar;
//            if (this.bar_1 == null)
//                this.bar_1 = bar;
//            else if (bar.double_0 > this.bar_1.double_0)
//                this.bar_1 = bar;
//            this.list_0.Add(bar);
//            int int_0 = this.list_0.Count - 1;
//            for (int index = 0; index < this.list_1.Count; ++index)
//                this.list_1[index].method_0(int_0);
//            if (this.int_0 == -1 || this.Count <= this.int_0)
//                return;
//            this.method_0();
        }

        void IDataSeries.Add(DataObject obj)
        {
            Add((Bar)obj);
        }

        public bool Contains(DateTime dateTime)
        {
            return GetIndex(dateTime, IndexOption.Null) != -1;
        }

        void IDataSeries.Remove(long index)
        {
            this.bars.RemoveAt((int)index);
        }

        public void Clear()
        {
            this.bars.Clear();
            this.min = this.max = null;
            Indicators.ForEach(i => i.Clear());
            Indicators.Clear();
        }

        public Bar GetMin()
        {
            return this.min;
        }

        public Bar GetMax()
        {
            return this.max;
        }

        public Bar HighestHighBar(int index1, int index2)
        {
            if (this.Count == 0 || index1 > index2 || index1 < 0 || index1 > this.Count - 1 || index2 < 0 || index2 > this.Count - 1)
                return null;
            Bar highest = this.bars[index1];
            for (int i = index1 + 1; i <= index2; ++i)
            {
                Bar bar = this.bars[i];
                highest = bar.High > highest.High ? bar : highest;
            }
            return highest;
        }

        public Bar HighestHighBar(int nBars)
        {
            return HighestHighBar(this.Count - nBars, this.Count - 1);
        }

        public Bar HighestHighBar(DateTime dateTime1, DateTime dateTime2)
        {
            return HighestHighBar(GetIndex(dateTime1, IndexOption.Next), GetIndex(dateTime2, IndexOption.Prev));
        }

        public Bar HighestHighBar()
        {
            return this.max;
        }

        public double HighestHigh(int index1, int index2)
        {
            return HighestHighBar(index1, index2).High;
        }

        public double HighestHigh(int nBars)
        {
            return HighestHighBar(nBars).High;
        }

        public double HighestHigh(DateTime dateTime1, DateTime dateTime2)
        {
            return HighestHighBar(dateTime1, dateTime2).High;
        }

        public double HighestHigh()
        {
            return HighestHighBar().High;
        }

        public Bar Ago(int n)
        {
            int index = this.Count - 1 - n;
            if (index < 0)
                throw new ArgumentException(string.Format("BarSeries::Ago Can not return bar {0} bars ago: bar series is too short, count = {1}", n, Count));
            return this[index];
        }

        private void EnsureNotEmpty()
        {
            if (Count <= 0)
                throw new ApplicationException("Array has no elements");
        }

        private void method_0()
        {
            this.bars.RemoveAt(0);
            for (int index = 0; index < this.Indicators.Count; ++index)
            {
                if (this.Indicators[index].Count > 0)
                    this.Indicators[index].Remove(0);
            }
        }
    }
}
