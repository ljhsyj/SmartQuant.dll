// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class InstrumentList : IEnumerable<Instrument>
    {
        private List<Instrument> instruments = new List<Instrument>();

        public Instrument this [string symbol]
        { 
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Contains(int id)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string symbol)
        {
            throw new NotImplementedException();

        }

        public bool Contains(Instrument instrument)
        {
            throw new NotImplementedException();

        }

        public Instrument GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Instrument GetByIndex(int index)
        {
            throw new NotImplementedException();
        }

        public void Remove(Instrument instrument)
        {
        }

        public void Clear()
        {
            instruments.Clear();
        }

        public IEnumerator<Instrument> GetEnumerator()
        {
            return this.instruments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.instruments.GetEnumerator();
        }
    }
}

