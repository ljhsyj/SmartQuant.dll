// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class InstrumentManager
    {
        private Framework framework;

        public InstrumentServer Server { get; private set; }

        public InstrumentList Instruments { get; private set ; }

        public InstrumentManager(Framework framework, InstrumentServer instrumentServer)
        {
            this.framework = framework;
            this.Server = instrumentServer;
            this.Instruments = new InstrumentList();
        }

        public void Load()
        {
        }

        public void Save(Instrument instrument)
        {
        }

        public void Add(Instrument instrument, bool save = true)
        {
        }

        public void Delete(string symbol)
        {
        }

        public void Delete(Instrument instrument)
        {
        }

        public bool Contains(string symbol)
        {
            return this.Instruments.Contains(symbol);
        }

        public Instrument Get(string symbol)
        {
            return this.Instruments[symbol];
        }

        public Instrument GetById(int id)
        {
            return this.Instruments.GetById(id);
        }

        public void Clear()
        {
            this.Instruments.Clear();
        }
    }
}
