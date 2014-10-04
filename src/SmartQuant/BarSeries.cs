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

        public int Count { get { throw new NotImplementedException(); } }

        public int MaxLength { get; set; }

        public List<Indicator> Indicators { get; private set; }

        public BarSeries(int maxLength)
            : this(null, null, maxLength)
        {
        }

        public BarSeries(string name = "", string description = "", int maxLength = -1)
        {
            this.name = name;
            this.description = description;
            this.MaxLength = maxLength;

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

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public double First
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double Last
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DateTime FirstDateTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DateTime LastDateTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        double ISeries.this [int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        double ISeries.this [int index, BarData barData]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region IDataSeries implementation

        public long GetIndex(DateTime dateTime, SearchOption option = SearchOption.Prev)
        {
            throw new NotImplementedException();
        }

        long IDataSeries.Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                return this.name;
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

        public DataObject this [long index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }


}
