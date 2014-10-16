using System;
using System.Globalization;
using System.Collections.Generic;

namespace SmartQuant.DriverFile
{
    public sealed class CsvDataView : IDataView
    {
        private string path;
        private char delimiter;
        private CultureInfo cultureInfo;

        public CsvDataView(string path, char delimiter)
        {
            this.path = path;
            this.delimiter = delimiter;
        }

        public CsvDataView(string path, char delimiter, CultureInfo cultureInfo)
            : this(path, delimiter)
        {

            this.cultureInfo = cultureInfo;
        }

        public T GetValue<T>(Key key, string fieldName)
        {
            throw new NotImplementedException();
        }

        public List<T> GetFullRange<T>(Key key, string fieldName)
        {
            throw new NotImplementedException();
        }

        public List<T> GetUniqueRange<T>(Key key, string fieldName)
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
