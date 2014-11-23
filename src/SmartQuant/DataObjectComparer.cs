using System;
using System.Collections.Generic;

namespace SmartQuant
{
    internal class DataObjectComparer : IComparer<DataObject>
    {
        public int Compare(DataObject x, DataObject y)
        {
            return x.DateTime.CompareTo(y.DateTime);
        }
    }

    internal enum Option
    {
        Exact,
        Prev,
        Next
    }

    internal static class DataObjectListSearcher
    {
        public static int GetIndex(List<DataObject> list, DateTime dateTime, DateTime firstDateTime, DateTime lastDateTime, Option option)
        {
            if (dateTime < firstDateTime)
                return option == Option.Exact || option == Option.Prev ? -1 : 0;
            if (dateTime > lastDateTime)
                return option == Option.Exact || option == Option.Next ? -1 : list.Count - 1;

            var i = list.BinarySearch(new DataObject() { DateTime = dateTime }, new DataObjectComparer());
            if (i >= 0)
                return i;
            else if (option == Option.Next)
                return ~i;
            else if (option == Option.Prev)
                return ~i - 1;
            return -1; // option == Option.Exact
        }
    }
}

