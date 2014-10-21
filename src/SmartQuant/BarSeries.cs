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
                return this[this.Count - 1];
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

        public DataObject this [long index]
        {
            get
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
            this.MaxLength = maxLength;
            this.bars = new List<Bar>();
            this.Indicators = new List<Indicator>();
        }

        public int GetIndex(DateTime dateTime, IndexOption option = IndexOption.Null)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int index)
        {
            throw new NotImplementedException();
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

        public long GetIndex(DateTime dateTime, SearchOption option = SearchOption.Prev)
        {
            throw new NotImplementedException();
        }

        public void Add(Bar bar)
        {
        }

        public void Clear()
        {
            this.bars.Clear();
            this.min = this.max = null;
            foreach (var indicator in Indicators)
                indicator.Clear();
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

        private void EnsureNotEmpty()
        {
            if (this.Count <= 0)
                throw new ApplicationException("Array has no elements");
        }
    }
}
