// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class DataSeries : IDataSeries
    {
        public long Count
        {
            get { throw new NotImplementedException(); }
        }

        public string Name { get; internal set; }

        public DateTime DateTime1
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime DateTime2
        {
            get { throw new NotImplementedException(); }
        }

        public DataObject this [long index]
        {
            get { throw new NotImplementedException(); }
        }

        public DataObject this [DateTime dateTime]
        {
            get { throw new NotImplementedException(); }
        }

        public bool CacheObjects { get; set; }

        public DataSeries(string name = null)
        {
        }

        public virtual long GetIndex(DateTime dateTime, SearchOption option = SearchOption.Prev)
        {
            throw new NotImplementedException();
        }
            
        public virtual void Add(DataObject obj)
        {
            throw new NotImplementedException();
        }

        public virtual DataObject Get(long index)
        {
            throw new NotImplementedException();
        }

        public virtual DataObject Get(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(long index, DataObject obj)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(long index)
        {
            throw new NotImplementedException();
        }
            
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Dump()
        {
            throw new NotImplementedException();
        }
    }
}
