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
            Server = instrumentServer;
            Instruments = new InstrumentList();
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
            return Instruments.Contains(symbol);
        }

        public Instrument Get(string symbol)
        {
            return Instruments[symbol];
        }

        public Instrument GetById(int id)
        {
            return Instruments.GetById(id);
        }

        public void Clear()
        {
            Instruments.Clear();
        }
    }
}
