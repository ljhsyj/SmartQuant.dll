// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Linq;

namespace SmartQuant
{
    public class InstrumentManager
    {
        private Framework framework;
        private int maxInstrumentId;
        private InstrumentList deletedInstruments;

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
            this.deletedInstruments = new InstrumentList();
            Server = instrumentServer;
            Instruments = new InstrumentList();
        }

        public void Load()
        {
            Server.Open();
            this.maxInstrumentId = -1;
            foreach (var instrument in Server.Load())
            {
                instrument.Init(this.framework);
                instrument.Loaded = true;
                if (instrument.Deleted)
                    this.deletedInstruments.Add(instrument);
                else
                    Instruments.Add(instrument);
                this.maxInstrumentId = Math.Max(this.maxInstrumentId, instrument.Id);
            }
            ++this.maxInstrumentId;
        }

        public void Save(Instrument instrument)
        {
            if (Server != null && instrument.Loaded)
                Server.Save(instrument);
        }

        public void Add(Instrument instrument, bool save = true)
        {
            if (Contains(instrument.Symbol))
                throw new ApplicationException(string.Format("Instrument with the same symbol is already present in the framework : {0}", instrument.Symbol));
            var i = this.deletedInstruments.Get(instrument.Symbol);
            if (i != null)
            {
                Console.WriteLine("InstrumentManager::Add Using deleted instrument id = {0} for symbol {1}", i.Id, instrument.Symbol);
                instrument.Id = i.Id;
                this.deletedInstruments.Remove(i);
            }
            else
            {
                instrument.Id = this.maxInstrumentId++;
            }
            Instruments.Add(instrument);
            instrument.TryInitWith(this.framework);
            if (save)
            {
                instrument.Loaded = true;
                Save(instrument);
            }
            this.framework.EventServer.OnInstrumentAdded(instrument);
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
            instrument.Deleted = true;
            this.deletedInstruments.Add(instrument);
            Save(instrument);
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
            var i = Instruments.GetById(id);
            if (i == null)
            {
                i = new Instrument(id, InstrumentType.Synthetic, Guid.NewGuid().ToString(), "", CurrencyId.USD);
                Instruments.Add(i);
            }
            return i;
        }

        public void Clear()
        {
            foreach(var i in Instruments)
            {
                i.Bid = null;
                i.Ask = null;
                i.Trade = null;
                i.Bar = null;
            }
            var deleted = new InstrumentList(Instruments.TakeWhile(i => !i.Loaded));
            foreach (var i in deleted)
                Delete(i);
        }

        public void Dump()
        {
            Console.WriteLine("Instrument manager contains {0} intruments:", Instruments.Count);
            foreach (var instrument in Instruments)
                Console.WriteLine(instrument);
        }
    }
}
