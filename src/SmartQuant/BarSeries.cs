// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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

        public Bar First
        {
            get
            {
                EnsureNotEmpty();
                return this[0];
            }
        }

        public Bar Last
        {
            get
            {
                EnsureNotEmpty();
                return this[Count - 1];
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
                return this.bars[index];
            }
        }

        public Bar this [DateTime dateTime, IndexOption option = IndexOption.Null]
        {
            get
            {
                return this[GetIndex(dateTime, option)];
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
                        return this[index].Close;
                    case BarData.Open:
                        return this[index].Open;
                    case BarData.High:
                        return this[index].High;
                    case BarData.Low:
                        return this[index].Low;
                    case BarData.Median:
                        return this[index].Median;
                    case BarData.Typical:
                        return this[index].Typical;
                    case BarData.Weighted:
                        return this[index].Weighted;
                    case BarData.Average:
                        return this[index].Average;
                    case BarData.Volume:
                        return this[index].Volume;
                    case BarData.OpenInt:
                        return this[index].OpenInt;
                    case BarData.Range:
                        return this[index].Range;
                    case BarData.Mean:
                        return this[index].Mean;
                    case BarData.Variance:
                        return this[index].Variance;
                    case BarData.StdDev:
                        return this[index].StdDev;
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

        DateTime IDataSeries.DateTime1
        {
            get
            {
                return FirstDateTime;
            }
        }

        DateTime IDataSeries.DateTime2
        {
            get
            {
                return LastDateTime;
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
                this.maxLength = value;
                if (this.maxLength == -1)
                    return;
                while (Count > this.maxLength)
                    Pop();
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

        public int GetIndex(DateTime datetime, IndexOption option = IndexOption.Null)
        {    
            if (datetime < FirstDateTime)
                return option == IndexOption.Null || option == IndexOption.Prev ? -1 : 0;
            if (datetime > LastDateTime)
                return option == IndexOption.Null || option == IndexOption.Next ? -1 : Count - 1;

            var i = this.bars.BinarySearch(new Bar() { DateTime = datetime }, new DataObjectComparer());
            if (i >= 0)
                return i;
            else if (option == IndexOption.Next)
                return ~i;
            else if (option == IndexOption.Prev)
                return ~i - 1;
            else
                return -1;
        }

        public DateTime GetDateTime(int index)
        {
            return this[index].DateTime;
        }

        public double GetMin(DateTime dateTime1, DateTime dateTime2)
        {
            int index1 = GetIndex(dateTime1, IndexOption.Next);
            int index2 = GetIndex(dateTime2, IndexOption.Prev);
            return GetMin(index1, index2, BarData.Low);
        }

        public double GetMin(int index1, int index2, BarData barData)
        {
            if (index1 > index2 || index1 == -1 || index2 == -1)
                return double.NaN;
            return Enumerable.Range(index1, index2 - index1 + 1).Min(i => this[i, barData]);
        }

        public double GetMax(DateTime dateTime1, DateTime dateTime2)
        {
            int index1 = GetIndex(dateTime1, IndexOption.Next);
            int index2 = GetIndex(dateTime2, IndexOption.Prev);
            return GetMax(index1, index2, BarData.High);
        }

        public double GetMax(int index1, int index2, BarData barData)
        {
            if (index1 > index2 || index1 == -1 || index2 == -1)
                return double.NaN;
            return Enumerable.Range(index1, index2 - index1 + 1).Max(i => this[i, barData]);
        }

        long IDataSeries.GetIndex(DateTime dateTime, SearchOption option)
        {
            if (option != SearchOption.Next && option != SearchOption.Prev)
                throw new ApplicationException("Unsupported search option");
            return option == SearchOption.Next ? GetIndex(dateTime, IndexOption.Next) : GetIndex(dateTime, IndexOption.Prev);
        }

        public void Add(Bar bar)
        {
            if (this.min == null)
                this.min = bar;
            else if (bar.High < this.min.Low)
                this.min = bar;
            if (this.max == null)
                this.max = bar;
            else if (bar.High > this.max.High)
                this.max = bar;

            this.bars.Add(bar);
            int lastIndex = this.bars.Count - 1;
            Indicators.ForEach(indicator => indicator.UpdateTo(lastIndex));
            if (this.maxLength == -1 || Count <= this.maxLength)
                return;
            Pop();
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
            if (Count == 0 || index1 > index2 || index1 < 0 || index1 > Count - 1 || index2 < 0 || index2 > Count - 1)
                return null;
            var highest = this[index1];
            for (int i = index1 + 1; i <= index2; ++i)
                if (this[i].High > highest.High)
                    highest = this[i];
            return highest;
        }

        public Bar HighestHighBar(int nBars)
        {
            return HighestHighBar(Count - nBars, Count - 1);
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

        public Bar HighestLowBar(int index1, int index2)
        {
            if (Count == 0 || index1 > index2 || index1 < 0 || index1 > Count - 1 || index2 < 0 || index2 > Count - 1)
                return null;
            var highest = this[index1];
            for (int i = index1 + 1; i <= index2; ++i)
                if (this[i].Low > highest.Low)
                    highest = this[i];
            return highest;
        }

        public Bar HighestLowBar(int nBars)
        {
            return HighestLowBar(Count - nBars, Count - 1);
        }

        public Bar HighestLowBar(DateTime dateTime1, DateTime dateTime2)
        {
            return HighestLowBar(GetIndex(dateTime1, IndexOption.Next), GetIndex(dateTime2, IndexOption.Prev));
        }

        public double HighestLow(int index1, int index2)
        {
            return HighestLowBar(index1, index2).Low;
        }

        public double HighestLow(int nBars)
        {
            return HighestLowBar(nBars).Low;
        }

        public double HighestLow(DateTime dateTime1, DateTime dateTime2)
        {
            return HighestLowBar(dateTime1, dateTime2).Low;
        }

        public Bar LowestLowBar(int index1, int index2)
        {
            if (Count == 0 || index1 > index2 || index1 < 0 || index1 > Count - 1 || index2 < 0 || index2 > Count - 1)
                return null;
            var lowest = this[index1];
            for (int i = index1 + 1; i <= index2; ++i)
                if (this[i].Low < lowest.Low)
                    lowest = this[i];
            return lowest;
        }

        public Bar LowestLowBar(int nBars)
        {
            return LowestLowBar(Count - nBars, Count - 1);
        }

        public Bar LowestLowBar(DateTime dateTime1, DateTime dateTime2)
        {
            return LowestLowBar(GetIndex(dateTime1, IndexOption.Next), GetIndex(dateTime2, IndexOption.Prev));
        }

        public Bar LowestLowBar()
        {
            return this.min;
        }

        public double LowestLow(int index1, int index2)
        {
            return LowestLowBar(index1, index2).Low;
        }

        public double LowestLow(int nBars)
        {
            return LowestLowBar(nBars).Low;
        }

        public double LowestLow(DateTime dateTime1, DateTime dateTime2)
        {
            return LowestLowBar(dateTime1, dateTime2).Low;
        }

        public double LowestLow()
        {
            return LowestLowBar().Low;
        }

        public Bar LowestHighBar(int index1, int index2)
        {
            if (Count == 0 || index1 > index2 || index1 < 0 || index1 > Count - 1 || index2 < 0 || index2 > Count - 1)
                return null;
            var lowest = this[index1];
            for (int index = index1 + 1; index <= index2; ++index)
                if (this[index].High < lowest.High)
                    lowest = this[index];
            return lowest;
        }

        public Bar LowestHighBar(int nBars)
        {
            return LowestHighBar(Count - nBars, Count - 1);
        }

        public Bar LowestHighBar(DateTime dateTime1, DateTime dateTime2)
        {
            return LowestHighBar(GetIndex(dateTime1, IndexOption.Next), GetIndex(dateTime2, IndexOption.Prev));
        }

        public double LowestHigh(int index1, int index2)
        {
            return LowestHighBar(index1, index2).High;
        }

        public double LowestHigh(int nBars)
        {
            return LowestHighBar(nBars).High;
        }

        public double LowestHigh(DateTime dateTime1, DateTime dateTime2)
        {
            return LowestHighBar(dateTime1, dateTime2).High;
        }

        public Bar Ago(int n)
        {
            int index = Count - 1 - n;
            if (index < 0)
                throw new ArgumentException(string.Format("BarSeries::Ago Can not return bar {0} bars ago: bar series is too short, count = {1}", n, Count));
            return this[index];
        }

        public BarSeries Compress(long barSize)
        {
            if (Count == 0)
                return new BarSeries("", "", -1);
            Bar bar = this[0];
            return null;
            throw new NotImplementedException();
//            return Class6.smethod_0(bar.InstrumentId, bar.Type, bar.Size, barSize).method_3((Class10) new Class11(this));
        }

        private void EnsureNotEmpty()
        {
            if (Count <= 0)
                throw new ApplicationException("Array has no elements");
        }

        private void Pop()
        {
            this.bars.RemoveAt(0);
            for (int i = 0; i < Indicators.Count; ++i)
                if (Indicators[i].Count > 0)
                    Indicators[i].Remove(0);
        }
    }

//    internal abstract class Class6
//    {
//        protected int int_0;
//        protected long long_0;
//        protected long long_1;
//        protected Bar bar_0;
//
//        private event EventHandler<EventArgs0> Event_0;
//
//        static Class6()
//        {
//            System.ComponentModel.LicenseManager.Validate(typeof (Class6));
//        }
//
//        protected Class6()
//        {
//            Class53.dbKDBOnzp98yT();
//            // ISSUE: explicit constructor call
//            base.\u002Ector();
//            this.bar_0 = (Bar) null;
//        }
//
//        public static Class6 smethod_0(int int_1, BarType barType_0, long long_2, long long_3)
//        {
//            Class6 class6;
//            switch (barType_0)
//            {
//                case BarType.Time:
//                    class6 = (Class6) new Class8();
//                    break;
//                case BarType.Tick:
//                    class6 = (Class6) new Class7();
//                    break;
//                case BarType.Volume:
//                    class6 = (Class6) new Class9();
//                    break;
//                default:
//                    throw new ArgumentException(string.Format("Unknown bar type - {0}", (object) barType_0));
//            }
//            class6.int_0 = int_1;
//            class6.long_0 = long_2;
//            class6.long_1 = long_3;
//            return class6;
//        }
//
//        protected abstract void Add(Class13 class13_0);
//
//        protected void method_0(Class14[] class14_0)
//        {
//            foreach (Class14 class14_0_1 in class14_0)
//                this.method_4(class14_0_1);
//        }
//
//        protected void method_1(BarType barType_0, DateTime dateTime_0, DateTime dateTime_1, double double_0)
//        {
//            this.bar_0 = new Bar(dateTime_0, dateTime_1, this.int_0, barType_0, this.long_1, double_0, double_0, double_0, double_0, 0L, 0L);
//        }
//
//        protected void method_2()
//        {
//            if (this.eventHandler_0 == null)
//                return;
//            this.eventHandler_0((object) this, new EventArgs0(this.bar_0));
//        }
//
//        public BarSeries method_3(Class10 class10_0)
//        {
//            BarSeries series = new BarSeries("", "", -1);
//            this.Event_0 += (EventHandler<EventArgs0>) ((sender, e) => series.Add(e.Bar));
//            while (class10_0.MoveNext())
//                this.Add(class10_0.Current);
//            this.method_5();
//            return series;
//        }
//
//        private void method_4(Class14 class14_0)
//        {
////            // ISSUE: reference to a compiler-generated method
////            if (class14_0.method_0() < this.bar_0.Low)
////            {
////                // ISSUE: reference to a compiler-generated method
////                this.bar_0.Low = class14_0.method_0();
////            }
////            // ISSUE: reference to a compiler-generated method
////            if (class14_0.method_0() > this.bar_0.High)
////            {
////                // ISSUE: reference to a compiler-generated method
////                this.bar_0.High = class14_0.method_0();
////            }
////            // ISSUE: reference to a compiler-generated method
////            this.bar_0.Close = class14_0.method_0();
////            // ISSUE: reference to a compiler-generated method
////            this.bar_0.Volume += (long) class14_0.method_2();
//        }
//
//        private void method_5()
//        {
//            if (this.bar_0 == null)
//                return;
//            this.method_2();
//        }
//    }
//
//    internal class Class11 : Class10
//    {
//        private BarSeries barSeries_0;
//
//        public override Class13 Current
//        {
//            get
//            {
//                Bar bar = this.barSeries_0[this.nfElsntqbG];
//                return new Class13(bar.OpenDateTime, new Class14[4]
//                {
//                    new Class14(bar.Open, (int) bar.Volume),
//                    new Class14(bar.High, 0),
//                    new Class14(bar.Low, 0),
//                    new Class14(bar.Close, 0)
//                });
//            }
//        }
//
//        public Class11(BarSeries barSeries_1):base(barSeries_1.Count)
//        {
//            this.barSeries_0 = barSeries_1;
//        }
//    }
//
//    internal class Class14
//    {
//        public Class14(double double_1, int int_1)
//        {
////            // ISSUE: reference to a compiler-generated method
////            this.method_1(double_1);
////            // ISSUE: reference to a compiler-generated method
////            this.method_3(int_1);
//        }
//    }
//
//    class Class13
//    {
//        public DateTime DateTime { get; private set; }
//
//        public Class13(DateTime dateTime_1, Class14[] class14_1)
//        {
//            this.DateTime = dateTime_1;
////            // ISSUE: reference to a compiler-generated method
////            this.method_1(class14_1);
//        }
//    }
//
//    internal abstract class Class10 : IEnumerator<Class13>, IDisposable
//    {
//        protected int nfElsntqbG;
//        private int VwvlzyMioU;
//
//        public abstract Class13 Current { get; }
//
//        object IEnumerator.Current
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }
//
//        protected Class10(int int_0)
//        {
//            this.VwvlzyMioU = int_0;
//            this.Reset();
//        }
//
//        public void Dispose()
//        {
//        }
//
//        public bool MoveNext()
//        {
//            return ++this.nfElsntqbG < this.VwvlzyMioU;
//        }
//
//        public void Reset()
//        {
//            this.nfElsntqbG = -1;
//        }
//    }
}
