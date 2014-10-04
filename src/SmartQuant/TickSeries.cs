// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;
using System.Collections;
using System;

namespace SmartQuant
{
    public class TickSeries : IDataSeries, IEnumerable<Tick>
	{
        public IEnumerator<Tick> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #region IDataSeries implementation
        public long GetIndex(DateTime dateTime, SearchOption option = SearchOption.Prev)
        {
            throw new NotImplementedException();
        }
        public long Count
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
                throw new NotImplementedException();
            }
        }
        public DateTime DateTime1
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public DateTime DateTime2
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public DataObject this[long index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion
	}
}
