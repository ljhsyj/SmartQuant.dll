// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;
using System.Collections.Generic;

namespace SmartQuant
{
    public class TickSeries : IDataSeries, IEnumerable<Tick>
	{
        #region IEnumerable implementation

        public IEnumerator<Tick> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

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
