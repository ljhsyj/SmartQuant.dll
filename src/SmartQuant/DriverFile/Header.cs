// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant.DriverFile
{
    public sealed class Header
    {
        public const string INT32 = "Int32";
        public const string DOUBLE = "Double";
        public const string STRING = "String";
        public const string DATETIME = "DateTime";
        public Dictionary<string, int> indexByField;
        public Dictionary<string, string> typeByField;
        public Dictionary<string, string> paramByField;

        public Header()
        {
            this.indexByField = new Dictionary<string, int>();
            this.typeByField = new Dictionary<string, string>();
            this.paramByField = new Dictionary<string, string>();
        }

        public static Header GetCsvHeader(string headerLine, char delimiter)
        {
            throw new NotImplementedException();
        }
    }
}

