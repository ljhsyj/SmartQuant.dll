// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SmartQuant
{
    public class TimeSeries : ISeries
    {
        protected internal string name;
        protected internal string description;

        private IDataSeries dataSeries;
        private TimeSeriesItem min;
        private TimeSeriesItem max;
        private bool dirty;
        private double sum;
        private double mean;
        private double median;
        private double variance;

        private static Func<double, double, double> opAdd = (a, b) => a + b;
        private static Func<double, double, double> opSub = (a, b) => a - b;
        private static Func<double, double, double> opMul = (a, b) => a * b;
        private static Func<double, double, double> opDiv = (a, b) => a / b;

        public string Name
        {
            get
            {
                return this.name;
            }
            protected internal set
            {
                this.name = value;
            }
        }

        public string Description
        {
            get
            { 
                return this.description;
            }
            protected internal set
            {
                this.description = value;
            }
        }

        public List<Indicator> Indicators { get; private set; }

        public virtual int Count
        {
            get
            { 
                return (int)this.dataSeries.Count;
            }
        }

        public virtual double First
        {
            get
            {
                return ((TimeSeriesItem)this.dataSeries[0]).Value;
            }
        }

        public virtual double Last
        {
            get
            {
                return ((TimeSeriesItem)this.dataSeries[this.dataSeries.Count - 1]).Value;
            }
        }

        public virtual DateTime FirstDateTime
        {
            get
            { 
                EnsureNotEmpty("Array has no elements");
                return this.dataSeries[0].DateTime;
            }
        }

        public virtual DateTime LastDateTime
        {
            get
            { 
                EnsureNotEmpty("Array has no elements");
                return this.dataSeries[this.dataSeries.Count - 1].DateTime;
            }
        }

        public virtual double this [int index]
        {
            get
            {
                return ((TimeSeriesItem)this.dataSeries[index]).Value;
            }
        }

        public virtual double this [int index, BarData barData]
        {
            get
            { 
                return this[index];
            }
        }

        public double this [int index, int row]
        {
            get
            { 
                return this[index];
            }
        }

        public double this [DateTime dateTime, SearchOption option = SearchOption.ExactFirst]
        {
            get
            {
                return GetByDateTime(dateTime, option).Value;
            }
            set
            {
                Add(dateTime, value);
            }
        }

        public double this [DateTime dateTime, int row, SearchOption option = SearchOption.ExactFirst]
        {
            get
            {
                return GetByDateTime(dateTime, option).Value;
            }
        }

        public TimeSeries()
            : this(null, null, null)
        {
        }

        public TimeSeries(IDataSeries series)
            : this(series.Name, series.Description, series)
        {
        }

        public TimeSeries(string name, string description = "")
            : this(name, description, null)
        {
        }

        private TimeSeries(string name, string description, IDataSeries series)
        {
            this.name = name;
            this.description = description;
            this.dirty = true;
            Indicators = new List<Indicator>();
            this.dataSeries = series ?? new MemorySeries(name, description);
        }

        public TimeSeriesItem GetItem(int index)
        {
            return (TimeSeriesItem)this.dataSeries[index];
        }

        public TimeSeriesItem GetMinItem()
        {
            return this.min;
        }

        public TimeSeriesItem GetMaxItem()
        {
            return this.max;
        }

        public virtual DateTime GetDateTime(int index)
        {
            return GetItem(index).DateTime;
        }

        public double GetValue(int index)
        {
            return GetItem(index).Value;
        }

        public virtual int GetIndex(DateTime datetime, IndexOption option = IndexOption.Null)
        {
            int num1 = 0;
            int num2 = 0;
            int num3 = (int)this.dataSeries.Count - 1;
            bool flag = true;
            while (flag)
            {
                if (num3 < num2)
                    return -1;
                num1 = (num2 + num3) / 2;
                switch (option)
                {
                    case IndexOption.Null:
                        if (this.dataSeries[(long)num1].dateTime == datetime)
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.dataSeries[(long)num1].dateTime > datetime)
                        {
                            num3 = num1 - 1;
                            continue;
                        }
                        else if (this.dataSeries[(long)num1].dateTime < datetime)
                        {
                            num2 = num1 + 1;
                            continue;
                        }
                        else
                            continue;
                    case IndexOption.Next:
                        if (this.dataSeries[(long)num1].dateTime >= datetime && (num1 == 0 || this.dataSeries[(long)(num1 - 1)].dateTime < datetime))
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.dataSeries[(long)num1].dateTime < datetime)
                        {
                            num2 = num1 + 1;
                            continue;
                        }
                        else
                        {
                            num3 = num1 - 1;
                            continue;
                        }
                    case IndexOption.Prev:
                        if (this.dataSeries[(long)num1].dateTime <= datetime && ((long)num1 == this.dataSeries.Count - 1L || this.dataSeries[(long)(num1 + 1)].dateTime > datetime))
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.dataSeries[(long)num1].dateTime > datetime)
                        {
                            num3 = num1 - 1;
                            continue;
                        }
                        else
                        {
                            num2 = num1 + 1;
                            continue;
                        }
                    default:
                        continue;
                }
            }
            return num1;
        }

        public TimeSeriesItem GetByDateTime(DateTime dateTime, SearchOption option = SearchOption.ExactFirst)
        {
            int index = IndexOf(dateTime, option);
            return index != -1 ? (TimeSeriesItem)this.dataSeries[index] : null;
        }

        public Cross Crosses(double level, int index)
        {
            if (index <= 0 || index > this.dataSeries.Count - 1)
                return Cross.None;
            var last = GetItem(index - 1).Value;
            var current = GetItem(index).Value;
            if (last <= level && level < current)
                return Cross.Above;
            if (last >= level && level > current)
                return Cross.Below;
            return Cross.None;
        }

        // TODO:rewrite it
        public Cross Crosses(TimeSeries series, DateTime dateTime)
        {
            int index1 = this.IndexOf(dateTime, SearchOption.ExactFirst);
            int index2 = series.IndexOf(dateTime, SearchOption.ExactFirst);
            if (index1 <= 0 || (long)index1 >= this.dataSeries.Count || (index2 <= 0 || index2 >= series.Count))
                return Cross.None;
            DateTime dateTime1 = this.GetDateTime(index1 - 1);
            DateTime dateTime2 = series.GetDateTime(index2 - 1);
            if (dateTime1 == dateTime2)
            {
                if (this.GetValue(index1 - 1) <= series.GetValue(index2 - 1) && this.GetValue(index1) > series.GetValue(index2))
                    return Cross.Above;
                if (this.GetValue(index1 - 1) >= series.GetValue(index2 - 1) && this.GetValue(index1) < series.GetValue(index2))
                    return Cross.Below;
            }
            else
            {
                double num1;
                double num2;
                if (dateTime1 < dateTime2)
                {
                    DateTime dateTime3 = this.GetDateTime(index1 - 1);
                    num1 = this.GetValue(index1 - 1);
                    num2 = series.IndexOf(dateTime3, SearchOption.Next) == index2 ? series.GetValue(series.IndexOf(dateTime3, SearchOption.Prev)) : series.GetValue(series.IndexOf(dateTime3, SearchOption.Next));
                }
                else
                {
                    DateTime dateTime3 = series.GetDateTime(index2 - 1);
                    num2 = series.GetValue(index2 - 1);
                    num1 = this.IndexOf(dateTime3, SearchOption.Prev) == index1 ? this.GetValue(this.IndexOf(dateTime3, SearchOption.Prev)) : this.GetValue(this.IndexOf(dateTime3, SearchOption.Next));
                }
                if (num1 <= num2 && this.GetValue(index1) > series.GetValue(index2))
                    return Cross.Above;
                if (num1 >= num2 && this.GetValue(index1) < series.GetValue(index2))
                    return Cross.Below;
            }
            return Cross.None;
        }

        // TODO: rewrite it
        public int IndexOf(DateTime dateTime, SearchOption option = SearchOption.ExactFirst)
        {
            int index = (int)this.dataSeries.Count - 1;
            if (dateTime == GetDateTime(index))
                return index;
            int num1 = 0;
            int num2 = 0;
            int num3 = (int)this.dataSeries.Count - 1;
            bool flag = true;
            while (flag)
            {
                if (num3 < num2)
                    return -1;
                num1 = (num2 + num3) / 2;
                switch (option)
                {
                    case SearchOption.Next:
                        if (this.dataSeries[(long)num1].DateTime >= dateTime && (num1 == 0 || this.dataSeries[(long)(num1 - 1)].DateTime < dateTime))
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.dataSeries[num1].dateTime < dateTime)
                        {
                            num2 = num1 + 1;
                            continue;
                        }
                        else
                        {
                            num3 = num1 - 1;
                            continue;
                        }
                    case SearchOption.Prev:
                        if (this.dataSeries[num1].DateTime <= dateTime && ((long)num1 == this.dataSeries.Count - 1 || this.dataSeries[(long)(num1 + 1)].DateTime > dateTime))
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.dataSeries[(long)num1].DateTime > dateTime)
                        {
                            num3 = num1 - 1;
                            continue;
                        }
                        else
                        {
                            num2 = num1 + 1;
                            continue;
                        }
                    case SearchOption.ExactFirst:
                        if (this.dataSeries[(long)num1].DateTime == dateTime)
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.dataSeries[(long)num1].DateTime > dateTime)
                        {
                            num3 = num1 - 1;
                            continue;
                        }
                        else if (this.dataSeries[(long)num1].DateTime < dateTime)
                        {
                            num2 = num1 + 1;
                            continue;
                        }
                        else
                            continue;
                    default:
                        continue;
                }
            }
            return num1;
        }

        public double GetMin()
        {
            return this.min != null ? this.min.Value : double.NaN;
        }

        public virtual double GetMin(DateTime dateTime1, DateTime dateTime2)
        {
            TimeSeriesItem result = null;
            for (int i = 0; i < this.dataSeries.Count; ++i)
            {
                var item = this.dataSeries[i] as TimeSeriesItem;
                if (dateTime1 <= item.DateTime && item.DateTime <= dateTime2)
                    result = result == null ? item : item.Value < result.Value ? item : result;
            }
            return result != null ? result.Value : double.NaN;
        }

        public double GetMin(int index1, int index2)
        {
            TimeSeriesItem result = null;
            for (int i = index1; i <= index2; ++i)
            {
                var item = this.dataSeries[i] as TimeSeriesItem ;
                result = result == null ? item : item.Value < result.Value ? item : result;
            }
            return result != null ? result.Value : double.NaN;
        }

        public virtual double GetMin(int index1, int index2, BarData barData)
        {
            return GetMin(index1, index2);
        }

        public double GetMax()
        {
            return this.max != null ? this.max.Value : double.NaN;
        }

        public virtual double GetMax(DateTime dateTime1, DateTime dateTime2)
        {
            TimeSeriesItem result = null;
            for (int i = 0; i < this.dataSeries.Count; ++i)
            {
                var item = this.dataSeries[i] as TimeSeriesItem;
                if (dateTime1 <= item.DateTime && item.DateTime <= dateTime2)
                    result = result == null ? item : item.Value > result.Value ? item : result;
            }
            return result != null ? result.Value : double.NaN;
        }

        public double GetMax(int index1, int index2)
        {
            TimeSeriesItem result = null;
            for (int i = index1; i <= index2; ++i)
            {
                var item = this.dataSeries[i] as TimeSeriesItem ;
                result = result == null ? item : item.Value > result.Value ? item : result;
            }
            return result != null ? result.Value : double.NaN;
        }

        public virtual double GetMax(int index1, int index2, BarData barData)
        {
            return GetMax(index1, index2);
        }

        public bool Contains(DateTime dateTime)
        {
            return IndexOf(dateTime, SearchOption.ExactFirst) != -1;
        }

        public void Add(DateTime dateTime, double value)
        {
            var item = new TimeSeriesItem(dateTime, value);
            this.max = this.max == null ? item : (this.max.Value < item.Value ? item : this.max);
            this.min = this.min == null ? item : (this.min.Value > item.Value ? item : this.min);
            this.dataSeries.Add(item);
  
            // Update indicators
            foreach (var indicator in Indicators)
                if (indicator.AutoUpdate)
                    indicator.Calculate((int)this.dataSeries.Count - 1);
        }

        public void Remove(int index)
        {
            this.dataSeries.Remove(index);
        }

        public double GetSum()
        {
            if (this.dirty)
                this.sum = GetSum(0, Count - 1, 0);
            return this.sum;
        }

        public double GetSum(int index1, int index2, int row)
        {
            EnsureIndexInRange(index1, index2); 
            return Enumerable.Range(index1, index2 - index1 + 1).Sum(i => this[i, row]);
        }

        public double GetMean()
        {
            EnsureNotEmpty();
            if (this.dirty)
                this.mean = GetMean(0, Count - 1);
            return this.mean;
        }

        public virtual double GetMean(int index1, int index2)
        {
            return GetMean(index1, index2, 0);
        }

        public virtual double GetMean(DateTime dateTime1, DateTime dateTime2)
        {
            return GetMean(dateTime1, dateTime2, 0);
        }

        public virtual double GetMean(int row)
        {
            return GetMean(0, Count - 1, row);
        }

        public double GetMean(int index1, int index2, int row)
        {
            EnsureNotEmpty("Can not calculate mean. Array is empty.");
            EnsureIndexInRange(index1, index2);
            var len = index2 - index1 + 1;
            return Enumerable.Range(index1, len).Sum(i => this[i, row]) / len;
        }

        public double GetMean(DateTime dateTime1, DateTime dateTime2, int row)
        {
            EnsureNotEmpty("Can not calculate mean. Array is empty.");
            int idx1, idx2;
            EnsureIndexInRange(dateTime1, dateTime2, out idx1, out idx2);
            return GetMean(idx1, idx2, row);
        }

        public virtual double GetMedian(int index1, int index2)
        {
            return GetMedian(index1, index2, 0);
        }

        public virtual double GetMedian(DateTime dateTime1, DateTime dateTime2)
        {
            return GetMedian(dateTime1, dateTime2, 0);
        }

        public virtual double GetMedian(int row)
        {
            return GetMedian(0, Count - 1, row);
        }

        public double GetMedian()
        {
            EnsureNotEmpty("Can not calculate median. Array is empty.");
            if (this.dirty)
                this.median = GetMedian(0, Count - 1);
            return this.median;
        }

        public double GetMedian(int index1, int index2, int row)
        {
            EnsureNotEmpty("Can not calculate median. Array is empty.");
            EnsureIndexInRange(index1, index2);
            var list = new List<double>();
            for (int i = index1; i <= index2; ++i)
                list.Add(this[i, row]);
            list.Sort();
            return list[list.Count / 2];
        }

        public double GetMedian(DateTime dateTime1, DateTime dateTime2, int row)
        {
            EnsureNotEmpty();
            int idx1, idx2;
            EnsureIndexInRange(dateTime1, dateTime2, out idx1, out idx2);
            return GetMedian(idx1, idx2, row);
        }

        public double GetVariance()
        {
            EnsureAtLeastOneElement("Can not calculate variance. Insufficient number of elements in the array.");
            if (this.dirty)
                this.variance = GetVariance(0, Count - 1);
            return this.variance;
        }

        public virtual double GetVariance(int index1, int index2)
        {
            return GetVariance(index1, index2, 0);
        }

        public virtual double GetVariance(DateTime dateTime1, DateTime dateTime2)
        {
            return GetVariance(dateTime1, dateTime2, 0);
        }

        public virtual double GetVariance(int row)
        {
            return GetVariance(0, Count - 1, row);
        }

        public double GetVariance(int index1, int index2, int row)
        {
            EnsureAtLeastOneElement("Can not calculate variance. Insufficient number of elements in the array.");
            EnsureIndexInRange(index1, index2);
            var m = GetMean(index1, index2, row);
            var len = index2 - index1 + 1;
            return Enumerable.Range(index1, len).Sum(i => (m - this[i, row]) * (m - this[i, row])) / (len - 1);
        }

        public virtual double GetVariance(DateTime dateTime1, DateTime dateTime2, int row)
        {
            EnsureAtLeastOneElement();
            int idx1, idx2;
            EnsureIndexInRange(dateTime1, dateTime2, out idx1, out idx2);
            return GetVariance(idx1, idx2, row);
        }

        public virtual double GetPositiveVariance()
        {
            return GetPositiveVariance(0);
        }

        private void AggregateWhere(int index1, int index2, int row, Predicate<double> p, Func<double,double,double> func, out double result, out int count)
        {
            count = 0;
            result = 0;
            for (int i = index1; i <= index2; ++i)
            {
                if (p(this[i, row]))
                {
                    result = func(this[i, row], result);
                    ++count;
                }
            }
        }

        private double GetVarianceUsing(int index1, int index2, int row, Predicate<double> p)
        {
            EnsureAtLeastOneElement();
            EnsureIndexInRange(index1, index2);
            int count1 = 0;
            double result1 = 0.0;
            AggregateWhere(index1, index2, row, p, (a, sum) => sum + a, out result1, out count1);
            double num3 = result1 / (double)count1;
            int count2 = 0;
            double result2 = 0;
            AggregateWhere(index1, index2, row, p, (a, sum) => sum + (num3 - a) * (num3 - a), out result2, out count2);
            return result2 / (double)count1;
        }

        public double GetPositiveVariance(int index1, int index2, int row)
        {
            return GetVarianceUsing(index1, index2, row, val => val > 0);
//            EnsureAtLeastOneElement();
//            EnsureIndexInRange(index1, index2);
//            int count1 = 0;
//            double result1 = 0.0;
//            AggregateWhere(index1, index2, row, val => val > 0, (a, sum) => sum + a, out result1, out count1);
//            double num3 = result1 / (double)count1;
//            int count2 = 0;
//            double result2 = 0;
//            AggregateWhere(index1, index2, row, val => val > 0, (a, sum) => sum + (num3 - a) * (num3 - a), out result2, out count2);
//            return result2 / (double)count1;
        }

        public double GetNegativeVariance(int index1, int index2, int row)
        {
            return GetVarianceUsing(index1, index2, row, val => val < 0);

//            EnsureAtLeastOneElement();
//            EnsureIndexInRange(index1, index2);
//
//            int count = 0;
//            double num2 = 0.0;
//            for (int index = index1; index <= index2; ++index)
//            {
//                if (this[index, row] < 0)
//                {
//                    num2 += this[index, row];
//                    ++count;
//                }
//            }
//            double num3 = num2 / (double)count;
//            double num4 = 0.0;
//            for (int index = index1; index <= index2; ++index)
//            {
//                if (this[index, row] < 0)
//                    num4 += (num3 - this[index, row]) * (num3 - this[index, row]);
//            }
//            return num4 / (double)count;
        }

        public virtual double GetPositiveVariance(DateTime dateTime1, DateTime dateTime2, int row)
        {
            EnsureAtLeastOneElement();
            int idx1, idx2;
            EnsureIndexInRange(dateTime1, dateTime2, out idx1, out idx2);
            return GetPositiveVariance(idx1, idx2, row);
        }

        public virtual double GetPositiveVariance(int index1, int index2)
        {
            return GetPositiveVariance(index1, index2, 0);
        }

        public virtual double GetPositiveVariance(DateTime dateTime1, DateTime dateTime2)
        {
            return GetPositiveVariance(dateTime1, dateTime2, 0);
        }

        public virtual double GetPositiveVariance(int row)
        {
            return GetPositiveVariance(0, Count - 1, row);
        }

        public virtual double GetNegativeVariance()
        {
            return GetNegativeVariance(0);
        }

        public virtual double GetNegativeVariance(int index1, int index2)
        {
            return GetNegativeVariance(index1, index2, 0);
        }

        public virtual double GetNegativeVariance(DateTime dateTime1, DateTime dateTime2)
        {
            return GetNegativeVariance(dateTime1, dateTime2, 0);
        }

        public virtual double GetNegativeVariance(int row)
        {
            return GetNegativeVariance(0, Count - 1, row);
        }

        public virtual double GetNegativeVariance(DateTime dateTime1, DateTime dateTime2, int row)
        {
            EnsureAtLeastOneElement();
            int idx1, idx2;
            EnsureIndexInRange(dateTime1, dateTime2, out idx1, out idx2);
            return GetNegativeVariance(idx1, idx2, row);
        }

        public double GetStdDev()
        {
            return Math.Sqrt(GetVariance());
        }

        public double GetStdDev(int index1, int index2)
        {
            return Math.Sqrt(GetVariance(index1, index2));
        }

        public double GetStdDev(DateTime dateTime1, DateTime dateTime2)
        {
            return Math.Sqrt(GetVariance(dateTime1, dateTime2));
        }

        public double GetStdDev(int row)
        {
            return Math.Sqrt(GetVariance(row));
        }

        public double GetStdDev(int index1, int index2, int row)
        {
            return Math.Sqrt(GetVariance(index1, index2, row));
        }

        public double GetStdDev(DateTime dateTime1, DateTime dateTime2, int row)
        {
            return Math.Sqrt(GetVariance(dateTime1, dateTime2, row));
        }

        public double GetPositiveStdDev()
        {
            return Math.Sqrt(GetPositiveVariance());
        }

        public double GetPositiveStdDev(int index1, int index2)
        {
            return Math.Sqrt(GetPositiveVariance(index1, index2));
        }

        public double GetPositiveStdDev(DateTime dateTime1, DateTime dateTime2)
        {
            return Math.Sqrt(GetPositiveVariance(dateTime1, dateTime2));
        }

        public double GetPositiveStdDev(int row)
        {
            return Math.Sqrt(GetPositiveVariance(row));
        }

        public double GetPositiveStdDev(int index1, int index2, int row)
        {
            return Math.Sqrt(GetPositiveVariance(index1, index2, row));
        }

        public double GetPositiveStdDev(DateTime dateTime1, DateTime dateTime2, int row)
        {
            return Math.Sqrt(GetPositiveVariance(dateTime1, dateTime2, row));
        }

        public double GetNegativeStdDev()
        {
            return Math.Sqrt(GetNegativeVariance());
        }

        public double GetNegativeStdDev(int index1, int index2)
        {
            return Math.Sqrt(GetNegativeVariance(index1, index2));
        }

        public double GetNegativeStdDev(DateTime dateTime1, DateTime dateTime2)
        {
            return Math.Sqrt(GetNegativeVariance(dateTime1, dateTime2));
        }

        public double GetNegativeStdDev(int row)
        {
            return Math.Sqrt(GetNegativeVariance(row));
        }

        public double GetNegativeStdDev(int index1, int index2, int row)
        {
            return Math.Sqrt(GetNegativeVariance(index1, index2, row));
        }

        public double GetNegativeStdDev(DateTime dateTime1, DateTime dateTime2, int row)
        {
            return Math.Sqrt(GetNegativeVariance(dateTime1, dateTime2, row));
        }

        public double GetMoment(int k, int index1, int index2, int row)
        {
            EnsureNotEmpty(string.Format("Can not calculate momentum. Series {0} is empty.", this.name));
            EnsureIndexInRange(index1, index2);
            double m = k != 1 ? GetMean(index1, index2, row) : 0;
            var len = index2 - index1 + 1;
            return Enumerable.Range(index1, len).Sum(i => Math.Pow(this[i, row] - m, k)) / len;
        }

        public double GetAsymmetry(int index1, int index2, int row)
        {
            EnsureNotEmpty(string.Format("Can not calculate asymmetry. Series {0} is empty.", this.name));
            EnsureIndexInRange(index1, index2);
            var sd = GetStdDev(index1, index2, row);
            return sd != 0 ? GetMoment(3, index1, index2, row) / Math.Pow(sd, 3) : 0;
        }

        public double GetExcess(int index1, int index2, int row)
        {
            EnsureNotEmpty(string.Format("Can not calculate excess. Series {0} is empty.", this.name));
            EnsureIndexInRange(index1, index2);
            var sd = GetStdDev(index1, index2, row);
            return sd != 0 ? GetMoment(4, index1, index2, row) / Math.Pow(sd, 4) : 0;
        }

        public double GetCovariance(int row1, int row2, int index1, int index2)
        {
            EnsureNotEmpty("Can not calculate covariance. Array is empty.");
            EnsureIndexInRange(index1, index2);
            double m1 = GetMean(index1, index2, row1);
            double m2 = GetMean(index1, index2, row2);
            var len = index2 - index1 + 1;
            return len <= 1 ? 0 : Enumerable.Range(index1, len).Sum(i => (this[i, row1] - m1) * (this[i, row2] - m2)) / (len - 1);
        }

        public double GetCorrelation(int row1, int row2, int index1, int index2)
        {
            return GetCovariance(row1, row2, index1, index2) / (GetStdDev(index1, index2, row1) * GetStdDev(index1, index2, row2));
        }

        public double GetCovariance(TimeSeries series)
        {
            EnsureNotNull(series);
            var m1 = GetMean();
            var m2 = series.GetMean();
            int count = 0;
            double sum = 0;
            for (int i = 0; i < Count; ++i)
            {
                var dateTime = GetDateTime(i);
                if (series.Contains(dateTime))
                {
                    sum += (this[i] - m1) * (series[dateTime, SearchOption.ExactFirst] - m2);
                    ++count;
                }
            }
            return count <= 1 ? 0 : sum / (count - 1d);
        }

        public double GetCorrelation(TimeSeries series)
        {
            return GetCovariance(series) / (GetStdDev() * series.GetStdDev());
        }

        private TimeSeries Transform0(string name, Func<double, double> func)
        {
            var ts = GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
            ts.name = name;
            ts.description = this.description;
            for (int i = 0; i < Count; ++i)
                ts.Add(GetDateTime(i), func(this[i, 0]));
            return ts;
        }

        private TimeSeries Transform1(string name, Func<double, double, double> func, double param1)
        {
            var ts = GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
            ts.name = name;
            ts.description = this.description;
            for (int i = 0; i < Count; ++i)
                ts.Add(GetDateTime(i), func(this[i, 0], param1));
            return ts;
        }

        public virtual TimeSeries Log()
        {
            return Transform0(string.Format("Log({0})", this.name), Math.Log);
        }

        public TimeSeries Log10()
        {
            return Transform0(string.Format("Log10({0})", this.name), Math.Log10);
        }

        public TimeSeries Sqrt()
        {
            return Transform0(string.Format("Sqrt({0})", this.name), Math.Sqrt);
        }

        public TimeSeries Exp()
        {
            return Transform0(string.Format("Exp({0})", this.name), Math.Exp);
        }

        public TimeSeries Pow(double pow)
        {
            return Transform1(string.Format("Pow({0})", this.name), Math.Pow, pow);
        }

        public virtual double GetAutoCovariance(int lag)
        {
            if (lag >= Count)
                throw new ApplicationException("Not enough data points in the series to calculate autocovariance");
            double m = GetMean();
            double len = Count - lag;
            return Enumerable.Range(lag, (int)len).Sum(i => (this[i, 0] - m) * (this[i - lag, 0] - m)) / len;
        }

        public double GetAutoCorrelation(int Lag)
        {
            return GetAutoCovariance(Lag) / GetVariance();
        }

        private TimeSeries GetReturnUsing(Func<double, double, double> func, string desc)
        {
            var ts = new TimeSeries(this.name, desc);
            if (Count < 2)
                return ts;
            double prev = this[0];
            for (int i = 0; i < Count; ++i)
            {
                double current = this[i];
                ts.Add(GetDateTime(i), func(prev, current));
                prev = current;
            }
            return ts;
        }

        public virtual TimeSeries GetReturnSeries()
        {
            return GetReturnUsing((prev, current) => prev != 0 ? current / prev : 0, string.Format("{0} (return)", this.description));
        }

        public virtual TimeSeries GetPercentReturnSeries()
        {
            return GetReturnUsing((prev, current) => prev != 0 ? (current / prev - 1) * 100 : 0, string.Format("{0} (% return)", this.description));
        }

        public virtual TimeSeries GetPositiveSeries()
        {
            var ts = new TimeSeries();
            for (int i = 0; i < Count; ++i)
                if (this[i] > 0)
                    ts.Add(GetDateTime(i), this[i]);
            return ts;
        }

        public virtual TimeSeries GetNegativeSeries()
        {
            var ts = new TimeSeries();
            for (int i = 0; i < Count; ++i)
                if (this[i] < 0)
                    ts.Add(GetDateTime(i), this[i]);
            return ts;
        }

        public TimeSeries Shift(int offset)
        {
            var ts = new TimeSeries(this.name, this.description);
            int start = offset < 0 ? Math.Abs(offset) : 0;
            for (int i = start; i < Count - offset; ++i)
                ts[GetDateTime(i + offset), SearchOption.ExactFirst] = this[i];
            return ts;
        }

        private static TimeSeries OpTwoTimeSeries(TimeSeries ts1, TimeSeries ts2, string name, Func<double, double, double> op, bool checkZero = false)
        {
            var ts = new TimeSeries(name, "");
            for (int i = 0; i < ts1.Count; ++i)
            {
                var datetime = ts1.GetDateTime(i);
                if (ts2.Contains(datetime))
                {
                    var d2 = ts2[datetime, SearchOption.ExactFirst];
                    if (checkZero && d2 == 0)
                        continue;
                    ts.Add(datetime, op(ts1[datetime, 0, SearchOption.ExactFirst], d2));
                }
            }
            return ts;
        }

        public static TimeSeries operator +(TimeSeries series1, TimeSeries series2)
        {
            EnsureNotNull(series1, series2);
            string name = string.Format("({0}+{1})", series1.Name, series2.Name);
            return OpTwoTimeSeries(series1, series2, name, opAdd);
        }

        public static TimeSeries operator -(TimeSeries series1, TimeSeries series2)
        {
            EnsureNotNull(series1, series2);
            var name = string.Format("({0}-{1})", series1.Name, series2.Name);
            return OpTwoTimeSeries(series1, series2, name, opSub);
        }

        public static TimeSeries operator *(TimeSeries series1, TimeSeries series2)
        {
            EnsureNotNull(series1, series2);
            var name = string.Format("({0}*{1})", series1.Name, series2.Name);
            return OpTwoTimeSeries(series1, series2, name, opMul);
        }

        public static TimeSeries operator /(TimeSeries series1, TimeSeries series2)
        {
            EnsureNotNull(series1, series2);
            var name = string.Format("({0}/{1})", series1.Name, series2.Name);
            return OpTwoTimeSeries(series1, series2, name, opDiv, true);
        }

        private static TimeSeries OpTimeSeriesWithValue(TimeSeries series, double value, string name, Func<double, double, double> op)
        {
            var ts = new TimeSeries(name, "");
            for (int i = 0; i < series.Count; ++i)
                ts.Add(series.GetDateTime(i), op(series[i, 0], value));
            return ts;
        }

        public static TimeSeries operator +(TimeSeries series, double value)
        {
            EnsureNotNull(series);
            var name = string.Format("({0}+{1:F2})", series.Name, value);
            return OpTimeSeriesWithValue(series, value, name, opAdd);
        }

        public static TimeSeries operator -(TimeSeries series, double value)
        {
            EnsureNotNull(series);
            var name = string.Format("({0}-{1:F2})", series.Name, value);
            return OpTimeSeriesWithValue(series, value, name, opSub);
        }

        public static TimeSeries operator *(TimeSeries series, double value)
        {
            EnsureNotNull(series);
            var name = string.Format("({0}*{1:F2})", series.Name, value);
            return OpTimeSeriesWithValue(series, value, name, opMul);
        }

        public static TimeSeries operator /(TimeSeries series, double value)
        {
            EnsureNotNull(series);
            var name = string.Format("({0}/{1:F2})", series.Name, value);
            return OpTimeSeriesWithValue(series, value, name, opDiv);
        }

        private static TimeSeries OpValueWithTimeSeries(TimeSeries series, double value, string name, Func<double, double, double> op, bool checkZero = false)
        {
            var ts = new TimeSeries(name, "");
            for (int i = 0; i < series.Count; ++i)
            {
                if (checkZero && series[i, 0] == 0)
                    continue;
                ts.Add(series.GetDateTime(i), op(value, series[i, 0]));
            }
            return ts;
        }

        public static TimeSeries operator +(double value, TimeSeries series)
        {
            EnsureNotNull(series);
            var name = string.Format("({0:F2}+{1})", value, series.Name);
            return OpValueWithTimeSeries(series, value, name, opAdd);
        }

        public static TimeSeries operator -(double value, TimeSeries series)
        {
            EnsureNotNull(series);
            var name = string.Format("({0:F2}-{1})", value, series.Name);
            return OpValueWithTimeSeries(series, value, name, opSub);
        }

        public static TimeSeries operator *(double value, TimeSeries series)
        {
            EnsureNotNull(series);
            var name = string.Format("({0:F2}*{1})", value, series.Name);
            return OpValueWithTimeSeries(series, value, name, opMul);
        }

        public static TimeSeries operator /(double value, TimeSeries series)
        {          
            EnsureNotNull(series);
            var name = string.Format("({0:F2}/{1})", value, series.Name);
            return OpValueWithTimeSeries(series, value, name, opDiv, true);
        }

        public double Ago(int n)
        {
            int index = Count - 1 - n;
            if (index < 0)
                throw new ArgumentException(string.Format("Can not return an entry {0} entries ago: time series is too short.", n));
            return this[index];
        }

        public void Clear()
        {
            this.dataSeries.Clear();
            this.max = this.min = null;
            this.dirty = true;
        }

        private static void EnsureNotNull(params object[] objs)
        {   
            foreach (var o in objs)
                if (o == null)
                    throw new ArgumentException("Operator argument can not be null");
        }

        private void EnsureAtLeastOneElement(string message = "")
        {
            if (Count <= 1)
                throw new ApplicationException("Can not calculate. Insufficient number of elements in the array.");
        }

        private void EnsureNotEmpty(string message = "")
        {
            if (Count <= 0)
                throw new ApplicationException(message);
        }

        private void EnsureIndexInRange(int idx1, int idx2)
        {
            if (idx1 > idx2)
                throw new ApplicationException("index1 must be smaller than index2");
            if (idx1 < 0 || idx2 > Count - 1)
                throw new ApplicationException("index1 is out of range");
            if (idx2 < 0 || idx2 > Count - 1)
                throw new ApplicationException("index2 is out of range");
        }

        private void EnsureIndexInRange(DateTime dt1, DateTime dt2, out int idx1, out int idx2)
        {
            if (dt1 >= dt2)
                throw new ApplicationException("dateTime1 must be smaller than dateTime2");
            idx1 = GetIndex(dt1, IndexOption.Null);
            idx2 = GetIndex(dt2, IndexOption.Null);
            if (idx1 == -1)
                throw new ApplicationException("dateTime1 is out of range");
            if (idx2 == -1)
                throw new ApplicationException("dateTime2 is out of range");
        }
    }
}
