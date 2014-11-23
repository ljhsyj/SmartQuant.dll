using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SmartQuant
{
    public class MemorySeries : IDataSeries
    {
        private List<DataObject> objs = new List<DataObject>();

        public long Count
        {
            get
            {
                return (long)this.objs.Count;
            }
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public DateTime DateTime1
        {
            get
            {
                return this.objs[0].DateTime;
            }
        }

        public DateTime DateTime2
        {
            get
            {
                return this.objs[this.objs.Count - 1].DateTime;
            }
        }

        public DataObject this [long index]
        {
            get
            {
                return this.objs[(int)index];
            }
        }

        public MemorySeries()
            : this(null, null)
        {
        }

        public MemorySeries(string name, string description = "")
        {
            Name = name;
            Description = description;
        }

        public void Add(DataObject obj)
        {
            this.objs.Add(obj);
        }

        public void Remove(long index)
        {
            this.objs.RemoveAt((int)index);
        }

        public bool Contains(DateTime dateTime)
        {
            return GetIndex(dateTime, SearchOption.ExactFirst) != -1;
        }

        public long GetIndex(DateTime dateTime, SearchOption option = SearchOption.Prev)
        {
            if (option == SearchOption.ExactLast)
                throw new NotSupportedException();

            if (dateTime < DateTime1)
                return option == SearchOption.ExactFirst || option == SearchOption.Prev ? -1 : 0;
            if (dateTime > DateTime2)
                return option == SearchOption.ExactFirst || option == SearchOption.Next ? -1 : Count - 1;

            var i = this.objs.BinarySearch(new DataObject { DateTime = dateTime }, new DataObjectComparer());
            if (i >= 0)
                return i;
            else if (option == SearchOption.Next)
                return ~i;
            else if (option == SearchOption.Prev)
                return ~i - 1;
            return -1; // option == IndexOption.Null
        }

        public void Clear()
        {
            this.objs.Clear();
        }
    }
}
