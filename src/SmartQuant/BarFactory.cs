// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartQuant
{
    public class BarFactory
    {
        private Framework framework;

        private IdArray<List<BarFactoryItem>> itemLists;

        public BarFactory(Framework framework)
        {
            this.framework = framework;
            this.itemLists = new IdArray<List<BarFactoryItem>>(8192);
        }

        public void Add(BarFactoryItem item)
        {
            if (item.factory != null)
                throw new InvalidOperationException("BarFactoryItem is already added to another BarFactory instance.");
            item.factory = this;
            int id = item.instrument.Id;
            var items = this.itemLists[id];
            if (items == null)
            {
                items = new List<BarFactoryItem>();
                this.itemLists[id] = items;
            }
            if (items.Exists(match => item.barType == match.barType && item.barSize == match.barSize && item.barInput == match.barInput))
                Console.WriteLine("{0} BarFactory::Add Item '{1}' is already added", DateTime.Now, item);
            else
                items.Add(item); 
        }

        public void Add(string symbol, BarType barType, long barSize, BarInput barInput = BarInput.Trade, ClockType type = ClockType.Local)
        {
            Add(this.framework.InstrumentManager.Get(symbol), barType, barSize, barInput, type);
        }

        public void Add(Instrument instrument, BarType barType, long barSize, BarInput barInput = BarInput.Trade, ClockType type = ClockType.Local)
        {
            BarFactoryItem item;
            switch (barType)
            {
                case BarType.Time:
                    item = new TimeBarFactoryItem(instrument, barSize, barInput, type);
                    break;
                case BarType.Tick:
                    item = new TickBarFactoryItem(instrument, barSize, barInput);
                    break;
                case BarType.Volume:
                    item = new VolumeBarFactoryItem(instrument, barSize, barInput);
                    break;
                default:
                    throw new ArgumentException(string.Format("Unknown bar type - {0}", barType));
            }
            Add(item);
        }

        public void Add(InstrumentList instruments, BarType barType, long barSize, BarInput barInput = BarInput.Trade, ClockType type = ClockType.Local)
        {
            foreach (var i in instruments)
                Add(i, barType, barSize, barInput, type);
        }

        public void Add(string[] symbols, BarType barType, long barSize, BarInput barInput = BarInput.Trade, ClockType type = ClockType.Local)
        {
            foreach (var s in symbols)
                Add(this.framework.InstrumentManager.Get(s), barType, barSize, barInput, type);
        }

        internal void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
