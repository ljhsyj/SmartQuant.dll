// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace SmartQuant
{
    public class InstrumentList : IEnumerable<Instrument>
    {
        private GetByList<Instrument> instruments = new GetByList<Instrument>();

        public int Count
        {
            get
            {
                return this.instruments.Count;
            }
        }

        public Instrument this [string symbol]
        { 
            get
            {
                return this.instruments.GetByName(symbol);
            }
        }

        public InstrumentList()
        {
        }

        public bool Contains(int id)
        {
            return this.instruments.Contains(id);
        }

        public bool Contains(string symbol)
        {
            return this.instruments.Contains(symbol);
        }

        public bool Contains(Instrument instrument)
        {
            return this.instruments.Contains(instrument);
        }

        public Instrument Get(string symbol)
        {
            return this.instruments.GetByName(symbol);
        }

        public Instrument GetById(int id)
        {
            return this.instruments.GetById(id);
        }

        public Instrument GetByIndex(int index)
        {
            return this.instruments.GetByIndex(index);
        }

        public void Add(Instrument instrument)
        {
            if (this.instruments.GetById(instrument.Id) == null)
                this.instruments.Add(instrument);
            else
                Console.WriteLine("InstrumentList::Add Instrument {0} with Id = {1} is already in the list", instrument.Symbol, instrument.Id);
        }

        public void Remove(Instrument instrument)
        {
            this.instruments.Remove(instrument);
        }

        public void Clear()
        {
            this.instruments.Clear();
        }

        public IEnumerator<Instrument> GetEnumerator()
        {
            return this.instruments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.instruments.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.instruments.Select(i => i.Symbol));
        }

        #region Extra Methods

        internal InstrumentList(IEnumerable<Instrument> instruments)
            : this()
        {
            Add(instruments);
        }

        internal void Add(IEnumerable<Instrument> instruments)
        {
            foreach (var i in instruments)
                Add(i);
        }

        #endregion
    }
}
