// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace SmartQuant.DriverFile
{
    public sealed class OdbcDataView : IDataView
    {
        public OdbcDataView(string connectionString, string tableName)
        {
        }

        public IDataView GetView(Key anyKey)
        {
            throw new NotImplementedException();
        }

        public List<T> GetUniqueRange<T>(Key singleKey)
        {
            throw new NotImplementedException();
        }

        public List<T> GetFullRange<T>(Key singleKey)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(Key uniqueKey, string fieldName)
        {
            throw new NotImplementedException();
        }

        public List<T> GetUniqueRange<T>(Key key, string fieldName)
        {
            throw new NotImplementedException();
        }

        public List<T> GetFullRange<T>(Key key, string fieldName)
        {
            throw new NotImplementedException();
        }

        public IDataView GetView(Key key, params string[] fieldNames)
        {
            throw new NotImplementedException();
        }

        public string[] GetFieldNames()
        {
            throw new NotImplementedException();
        }
    }
}
