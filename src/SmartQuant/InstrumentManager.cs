// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class InstrumentManager
    {
        private Framework framework;
        private int maxInstrumentId;

        public InstrumentServer Server { get; private set; }

        public InstrumentList Instruments { get; private set ; }

        public Instrument this [string symbol]
        {
            get
            {
                return Instruments.Get(symbol);
            }
        }

        public InstrumentManager(Framework framework, InstrumentServer instrumentServer)
        {
            this.framework = framework;
            Server = instrumentServer;
        }

        public void Load()
        {
            Server.Open();
            this.maxInstrumentId = -1;
            Instruments = Server.Load();
            foreach (var instrument in Instruments)
                instrument.Init(this.framework);
            for (int i = 0; i < Instruments.Count; ++i)
            {
                Instruments.GetByIndex(i).loaded = true;
                int id = Instruments.GetByIndex(i).Id;
                this.maxInstrumentId = Math.Max(this.maxInstrumentId, id);
            }
            ++this.maxInstrumentId;
        }

        public void Save(Instrument instrument)
        {
            if (Server != null && instrument.loaded)
                Server.Save(instrument);
        }

        public void Add(Instrument instrument, bool save = true)
        {
        }

        public void Delete(string symbol)
        {
            var instrument = Get(symbol);
            if (instrument != null)
                Delete(instrument);
        }

        public void Delete(Instrument instrument)
        {
            Instruments.Remove(instrument);
            if (Server != null && instrument.loaded)
                Server.Delete(instrument);
            this.framework.EventServer.OnInstrumentDeleted(instrument);
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
            throw new NotImplementedException();
        }

        public void Dump()
        {
            Console.WriteLine("Instrument manager contains {0} intruments:", Instruments.Count);
            foreach (var instrument in Instruments)
                Console.WriteLine(instrument);
        }
    }
}
