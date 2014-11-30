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
                return this.dataSeries[0].DateTime;
            }
        }

        public virtual DateTime LastDateTime
        {
            get
            { 
                return this.dataSeries[this.dataSeries.Count - 1].DateTime;
            }
        }

        public virtual double this[int index]
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
            :this(series.Name, series.Description, series)
        {
        }

        public TimeSeries(string name, string description = "")
            :this(name, description, null)
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
            throw new NotImplementedException();
        }

        public TimeSeriesItem GetByDateTime(DateTime dateTime, SearchOption option = SearchOption.ExactFirst)
        {
            int index = IndexOf(dateTime, option);
            return index != -1 ? (TimeSeriesItem)this.dataSeries[index] : null;
        }

        public Cross Crosses(double level, int index)
        {
            throw new NotImplementedException();
        }

        public Cross Crosses(TimeSeries series, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(DateTime dateTime, SearchOption option = SearchOption.ExactFirst)
        {
            int index = (int) this.dataSeries.Count - 1;
            if (dateTime == GetDateTime(index))
                return index;
            int num1 = 0;
            int num2 = 0;
            int num3 = (int) this.dataSeries.Count - 1;
            bool flag = true;
            while (flag)
            {
                if (num3 < num2)
                    return -1;
                num1 = (num2 + num3) / 2;
                switch (option)
                {
                    case SearchOption.Next:
                        if (this.dataSeries[(long) num1].DateTime >= dateTime && (num1 == 0 || this.dataSeries[(long) (num1 - 1)].DateTime < dateTime))
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
                        if (this.dataSeries[num1].DateTime <= dateTime && ((long) num1 == this.dataSeries.Count - 1 || this.dataSeries[(long) (num1 + 1)].DateTime > dateTime))
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.dataSeries[(long) num1].DateTime > dateTime)
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
                        if (this.dataSeries[(long) num1].DateTime == dateTime)
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.dataSeries[(long) num1].DateTime > dateTime)
                        {
                            num3 = num1 - 1;
                            continue;
                        }
                        else if (this.dataSeries[(long) num1].DateTime < dateTime)
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
            return this.min != null ? min.Value : double.NaN;
        }

        public virtual double GetMin(DateTime dateTime1, DateTime dateTime2)
        {
            throw new NotImplementedException();
        }

        public double GetMin(int index1, int index2)
        {
            return GetMin(index1, index2);
        }

        public virtual double GetMin(int index1, int index2, BarData barData)
        {
            throw new NotImplementedException();
        }

        public double GetMax()
        {
            return this.max != null ? max.Value : double.NaN;
        }

        public virtual double GetMax(DateTime dateTime1, DateTime dateTime2)
        {
            throw new NotImplementedException();
        }

        public double GetMax(int index1, int index2)
        {
            throw new NotImplementedException();
        }

        public virtual double GetMax(int index1, int index2, BarData barData)
        {
            return GetMax(index1, index2);
        }

        public bool Contains(DateTime dateTime)
        {
            return GetByDateTime(dateTime, SearchOption.ExactFirst) != null;
        }

        public void Add(DateTime dateTime, double value)
        {
            var item = new TimeSeriesItem(dateTime, value);
            this.max = this.max == null ? item : (this.max.Value < item.Value ? item : this.max);
            this.min = this.min == null ? item : (this.min.Value > item.Value ? item : this.min);
            this.dataSeries.Add(item);
  
            // Update indicators
//            foreach (var indicator in this.Indicators)
//                if (indicator.AutoUpdate)
//                    indicator.Calculate(this.items.Count - 1);
        }

        public void Remove(int index)
        {
            this.dataSeries.Remove(index);
        }

        public void Clear()
        {
            this.dataSeries.Clear();
        }

        public double GetSum()
        {
            if (this.dirty)
                this.sum = GetSum(0, this.Count, 0);
            return this.sum;
        }

        public double GetSum(int index1, int index2, int row)
        {
            EnsureIndexInRange(index1, index2);
            var len = index2 - index1 + 1;
            return Enumerable.Range(index1, len).Sum(i => this[i, row]);
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
            return GetMean(0, this.Count - 1, row);
        }

        public double GetMean(int index1, int index2, int row)
        {
            EnsureNotEmpty();
            EnsureIndexInRange(index1, index2);
            var len = index2 - index1 + 1;
            return Enumerable.Range(index1, len).Sum(i => this[i, row]) / len;
        }

        public double GetMean(DateTime dateTime1, DateTime dateTime2, int row)
        {
            EnsureNotEmpty();
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
            return GetMedian(0, this.Count - 1, row);
        }

        public double GetMedian()
        {
            EnsureNotEmpty();
            if (this.dirty)
                this.median = GetMedian(0, this.Count - 1);
            return this.median;
        }

        public double GetMedian(int index1, int index2, int row)
        {
            EnsureNotEmpty();
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
            return this.GetMedian(idx1, idx2, row);
        }

        public double GetVariance()
        {
            EnsureAtLeastOneElement();
            if (this.dirty)
                this.variance = GetVariance(0, this.Count - 1);
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
            return GetVariance(0, this.Count - 1, row);
        }

        public double GetVariance(int index1, int index2, int row)
        {
            EnsureAtLeastOneElement();
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
            return this.GetPositiveVariance(0);
        }

        public double GetPositiveVariance(int index1, int index2, int row)
        {
            EnsureAtLeastOneElement();
            EnsureIndexInRange(index1, index2);

            int num1 = 0;
            double num2 = 0.0;
            for (int index = index1; index <= index2; ++index)
            {
                if (this[index, row] > 0.0)
                {
                    num2 += this[index, row];
                    ++num1;
                }
            }
            double num3 = num2 / (double)num1;
            double num4 = 0.0;
            for (int index = index1; index <= index2; ++index)
            {
                if (this[index, row] > 0.0)
                    num4 += (num3 - this[index, row]) * (num3 - this[index, row]);
            }
            return num4 / (double)num1;
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
            return this.GetPositiveVariance(index1, index2, 0);
        }

        public virtual double GetPositiveVariance(DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetPositiveVariance(dateTime1, dateTime2, 0);
        }

        public virtual double GetPositiveVariance(int row)
        {
            return this.GetPositiveVariance(0, this.Count - 1, row);
        }

        public virtual double GetNegativeVariance()
        {
            return this.GetNegativeVariance(0);
        }

        public virtual double GetNegativeVariance(int index1, int index2)
        {
            return this.GetNegativeVariance(index1, index2, 0);
        }

        public virtual double GetNegativeVariance(DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetNegativeVariance(dateTime1, dateTime2, 0);
        }

        public virtual double GetNegativeVariance(int row)
        {
            return this.GetNegativeVariance(0, this.Count - 1, row);
        }

        public double GetNegativeVariance(int index1, int index2, int row)
        {
            EnsureAtLeastOneElement();
            EnsureIndexInRange(index1, index2);

            int num1 = 0;
            double num2 = 0.0;
            for (int index = index1; index <= index2; ++index)
            {
                if (this[index, row] < 0.0)
                {
                    num2 += this[index, row];
                    ++num1;
                }
            }
            double num3 = num2 / (double)num1;
            double num4 = 0.0;
            for (int index = index1; index <= index2; ++index)
            {
                if (this[index, row] < 0.0)
                    num4 += (num3 - this[index, row]) * (num3 - this[index, row]);
            }
            return num4 / (double)num1;
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
            EnsureNotEmpty();
            EnsureIndexInRange(index1, index2);
            double m = k != 1 ? GetMean(index1, index2, row) : 0;
            var len = index2 - index1 + 1;
            return Enumerable.Range(index1, len).Sum(i => Math.Pow(this[i, row] - m, k)) / len;
        }

        public double GetAsymmetry(int index1, int index2, int row)
        {
            EnsureNotEmpty();
            EnsureIndexInRange(index1, index2);
            var sd = GetStdDev(index1, index2, row);
            return sd != 0 ? GetMoment(3, index1, index2, row) / Math.Pow(sd, 3) : 0;
        }

        public double GetExcess(int index1, int index2, int row)
        {
            EnsureNotEmpty();
            EnsureIndexInRange(index1, index2);
            var sd = GetStdDev(index1, index2, row);
            return sd != 0 ? GetMoment(4, index1, index2, row) / Math.Pow(sd, 3) : 0;
        }

        public double GetCovariance(int row1, int row2, int index1, int index2)
        {
            EnsureNotEmpty();
            EnsureIndexInRange(index1, index2);
            double m1 = this.GetMean(index1, index2, row1);
            double m2 = this.GetMean(index1, index2, row2);
            var len = index2 - index1 + 1;
            return len <= 1 ? 0 : Enumerable.Range(index1, index2 - index1 + 1).Sum(i => (this[i, row1] - m1) * (this[i, row2] - m2)) / len;
        }

        public double GetCorrelation(int row1, int row2, int index1, int index2)
        {
            return GetCovariance(row1, row2, index1, index2) / (GetStdDev(index1, index2, row1) * GetStdDev(index1, index2, row2));
        }

        public double GetCovariance(TimeSeries series)
        {
            EnsureNotNull(series);
            var mean1 = this.GetMean();
            var mean2 = series.GetMean();
            double num1 = 0.0;
            double num2 = 0.0;
            for (int i = 0; i < this.Count; ++i)
            {
                DateTime dateTime = this.GetDateTime(i);
                if (series.Contains(dateTime))
                {
                    num2 += (this[i] - mean1) * (series[dateTime, SearchOption.ExactFirst] - mean2);
                    ++num1;
                }
            }
            if (num1 <= 1)
                return 0;
            else
                return num2 / (num1 - 1);
        }

        public double GetCorrelation(TimeSeries series)
        {
            return GetCovariance(series) / (GetStdDev() * series.GetStdDev());
        }

        public virtual TimeSeries Log()
        {
            var ts = this.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
            ts.name = string.Format("Log({0})", this.name);
            ts.description = this.description;
            for (int i = 0; i < this.Count; ++i)
                ts.Add(GetDateTime(i), Math.Log(this[i, 0]));
            return ts;
        }

        public TimeSeries Log10()
        {
            var ts = this.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
            ts.name = string.Format("Log10({0})", this.name);
            ts.description = this.description;
            for (int i = 0; i < this.Count; ++i)
                ts.Add(GetDateTime(i), Math.Log10(this[i, 0]));
            return ts;
        }

        public TimeSeries Sqrt()
        {
            var ts = this.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
            ts.name = string.Format("Sqrt({0})", this.name);
            ts.description = this.description;
            for (int i = 0; i < this.Count; ++i)
                ts.Add(GetDateTime(i), Math.Sqrt(this[i, 0]));
            return ts;
        }

        public TimeSeries Exp()
        {
            var ts = this.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
            ts.name = string.Format("Exp({0})", this.name);
            ts.description = this.description;
            for (int i = 0; i < this.Count; ++i)
                ts.Add(this.GetDateTime(i), Math.Exp(this[i, 0]));
            return ts;
        }

        public TimeSeries Pow(double pow)
        {
            var ts = this.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
            ts.name = string.Format("Pow({0}", this.name);
            ts.description = this.description;
            for (int i = 0; i < this.Count; ++i)
                ts.Add(GetDateTime(i), Math.Pow(this[i, 0], pow));
            return ts;
        }

        public virtual double GetAutoCovariance(int lag)
        {
            if (lag >= this.Count)
                throw new ApplicationException("Not enough data points in the series to calculate autocovariance");
            double mean = GetMean();
            return Enumerable.Range(lag, this.Count - 1).Sum(i => (this[i, 0] - mean) * (this[i - lag, 0] - mean)) / (this.Count - lag);
        }

        public double GetAutoCorrelation(int Lag)
        {
            return GetAutoCovariance(Lag) / GetVariance();
        }

        public virtual TimeSeries GetReturnSeries()
        {
            var ts = new TimeSeries(this.name, string.Format("{0} (return)", this.description));
            if (this.Count < 2)
                return ts;

            double prev = this[0];
            for (int i = 0; i < this.Count; ++i)
            {
                double current = this[i];
                ts.Add(GetDateTime(i), prev != 0 ? current / prev : 0);
                prev = current;
            }
            return ts;
        }

        public virtual TimeSeries GetPercentReturnSeries()
        {
            var ts = new TimeSeries(this.name, string.Format("{0} (% return)", this.description));
            if (this.Count < 2)
                return ts;

            double prev = this[0];
            for (int i = 0; i < this.Count; ++i)
            {
                double current = this[i];
                ts.Add(GetDateTime(i), prev != 0 ? (current / prev - 1) * 100 : 0);
                prev = current;
            }
            return ts;
        }

        public virtual TimeSeries GetPositiveSeries()
        {
            var ts = new TimeSeries();
            for (int i = 0; i < this.Count; ++i)
                if (this[i] > 0)
                    ts.Add(this.GetDateTime(i), this[i]);
            return ts;
        }

        public virtual TimeSeries GetNegativeSeries()
        {
            var ts = new TimeSeries();
            for (int i = 0; i < this.Count; ++i)
                if (this[i] < 0)
                    ts.Add(this.GetDateTime(i), this[i]);
            return ts;
        }

        public TimeSeries Shift(int offset)
        {
            var ts = new TimeSeries(this.name, this.description);
            int start = offset < 0 ? Math.Abs(offset) : 0;
            for (int i = start; i < this.Count - offset; ++i)
                ts[GetDateTime(i + offset), SearchOption.ExactFirst] = this[i];
            return ts;
        }

        public static TimeSeries operator +(TimeSeries series1, TimeSeries series2)
        {
            EnsureNotNull(series1, series2);
            var ts = new TimeSeries(string.Format("({0}+{1})", series1.Name, series2.Name), "");
            for (int i = 0; i < series1.Count; ++i)
            {
                var dt = series1.GetDateTime(i);
                if (series2.Contains(dt))
                    ts.Add(dt, series1[dt, 0, SearchOption.ExactFirst] + series2[dt, 0, SearchOption.ExactFirst]);
            }
            return ts;
        }

        public static TimeSeries operator -(TimeSeries series1, TimeSeries series2)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator *(TimeSeries series1, TimeSeries series2)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator /(TimeSeries series1, TimeSeries series2)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator +(TimeSeries series, double value)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator -(TimeSeries series, double value)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator *(TimeSeries series, double value)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator /(TimeSeries series, double value)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator +(double value, TimeSeries series)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator -(double value, TimeSeries series)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator *(double value, TimeSeries series)
        {
            throw new NotImplementedException();
        }

        public static TimeSeries operator /(double value, TimeSeries series)
        {          
            throw new NotImplementedException();
        }

        public double Ago(int n)
        {
            int index = this.Count - 1 - n;
            if (index < 0)
                throw new ArgumentException(string.Format("Can not return an entry {0} entries ago: time series is too short.", n));
            return this[index];
        }

        private static void EnsureNotNull(params object[] objs)
        {   
            foreach (var o in objs)
                if (o == null)
                    throw new ArgumentException("Operator argument can not be null");
        }

        private void EnsureAtLeastOneElement()
        {
            if (this.Count <= 1)
                throw new ApplicationException("Can not calculate. Insufficient number of elements in the array.");
        }

        private void EnsureNotEmpty()
        {
            if (this.Count <= 0)
                throw new ApplicationException("Can not calculate. Array is empty.");
        }

        private void EnsureIndexInRange(int idx1, int idx2)
        {
            if (idx1 > idx2)
                throw new ApplicationException("index1 must be smaller than index2");
            if (idx1 < 0 || idx2 > this.Count - 1)
                throw new ApplicationException("index1 is out of range");
            if (idx2 < 0 || idx2 > this.Count - 1)
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
