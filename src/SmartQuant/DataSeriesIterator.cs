// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class DataSeriesIterator
    {
        private DataSeries series;
        private long index1;
        private long index2;
        private long current;

        public DataSeriesIterator(DataSeries series, long index1 = -1, long index2 = -1)
        {
            this.series = series;
            this.index1 = index1 != -1 ? index1 : 0;
            this.index2 = index2 != -1 ? series.Count - 1 : 0;
            this.current = index1;
        }

        public DataObject GetNext()
        {
            return this.current > this.index2 ? null : this.series.Get(this.current++);
        }
    }
}

