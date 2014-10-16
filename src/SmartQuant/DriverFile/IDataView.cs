// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;

namespace SmartQuant.DriverFile
{
    public interface IDataView
    {
        T GetValue<T>(Key key, string fieldName);

        List<T> GetUniqueRange<T>(Key key, string fieldName);

        List<T> GetFullRange<T>(Key key, string fieldName);

        IDataView GetView(Key key, params string[] fieldNames);

        string[] GetFieldNames();
    }
}

