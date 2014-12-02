using SmartQuant;
using System;

namespace SmartQuant
{
    public class NetDataSeries : DataSeries
    {
        public NetDataSeries(string name)
        {
            Name = name;
        }

        public override long GetIndex(DateTime dateTime, SearchOption option = SearchOption.Prev)
        {
            throw new NotImplementedException();
        }

        public override DataObject Get(long index)
        {
            throw new NotImplementedException();
        }

        public override DataObject Get(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public override void Add(DataObject obj)
        {
        }

        public override void Update(long index, DataObject obj)
        {
        }

        public override void Remove(long index)
        {
        }

        public override void Clear()
        {
        }

        public override void Refresh()
        {
        }
    }
}