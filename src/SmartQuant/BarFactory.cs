// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

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

        public void Add(string symbol, BarType barType, long barSize, BarInput barInput = BarInput.Trade)
        {
            var instrument = this.framework.InstrumentManager.Get(symbol);
            this.Add(instrument, barType, barSize, barInput);
        }

        public void Add(Instrument instrument, BarType barType, long barSize, BarInput barInput = BarInput.Trade)
        {
            throw new NotImplementedException();
        }

        public void Add(InstrumentList instruments, BarType barType, long barSize, BarInput barInput = BarInput.Trade)
        {
            foreach (Instrument instrument in instruments)
                this.Add(instrument, barType, barSize, barInput);
        }

        public void Add(string[] symbols, BarType barType, long barSize, BarInput barInput = BarInput.Trade)
        {
            foreach (string symbol in symbols)
            {
                var instrument = this.framework.InstrumentManager.Get(symbol);
                this.Add(instrument, barType, barSize, barInput);
            }
        }
    }
}
