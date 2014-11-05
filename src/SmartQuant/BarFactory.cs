// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartQuant
{
    public partial class BarFactory
    {
        private Framework framework;

        public BarFactory(Framework framework)
        {
            this.framework = framework;
        }

        public void Add(BarFactoryItem item)
        {
            throw new NotImplementedException();
        }

        public void Add(string symbol, BarType barType, long barSize, BarInput barInput = BarInput.Trade, ClockType type = ClockType.Local)
        {
            var instrument = this.framework.InstrumentManager.Get(symbol);
            this.Add(instrument, barType, barSize, barInput);
        }

        public void Add(Instrument instrument, BarType barType, long barSize, BarInput barInput = BarInput.Trade, ClockType type = ClockType.Local)
        {
            throw new NotImplementedException();
        }

        public void Add(InstrumentList instruments, BarType barType, long barSize, BarInput barInput = BarInput.Trade, ClockType type = ClockType.Local)
        {
            Parallel.ForEach(instruments, instrument => this.Add(instrument, barType, barSize, barInput));
        }

        public void Add(string[] symbols, BarType barType, long barSize, BarInput barInput = BarInput.Trade, ClockType type = ClockType.Local)
        {
            Parallel.ForEach(symbols, symbol => this.Add(this.framework.InstrumentManager.Get(symbol), barType, barSize, barInput));
        }

        internal void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
