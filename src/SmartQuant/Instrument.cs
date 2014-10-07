// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class Instrument
    {
        public string Symbol { get; private set; }

        public double Factor { get; set; }

        public ObjectTable Fields { get; private set; }

        public Instrument Parent { get; set; }

        public int Id { get; set; }

        public InstrumentType Type { get; private set; }

        public string Description { get; set; }

        public string Exchange { get; set; }

        public byte CurrencyId { get; set; }

        public byte CCY1 { get; set; }

        public byte CCY2 { get; set; }

        public double TickSize { get; set; }

        public DateTime Maturity { get; set; }

        public double Strike { get; set; }

        public PutCall PutCall { get; set; }

        public double Margin { get; set; }

        public string PriceFormat { get; set; }

        public AltIdList AltId { get; private set; }

        public List<Leg> Legs { get; private set; }

        public Bid Bid { get; private set; }

        public Ask Ask { get; private set; }

        public Trade Trade { get; private set; }

        public Bar Bar { get; private set; }

        public IDataProvider DataProvider { get; private set; }

        public IExecutionProvider ExecutionProvider { get; private set; }

        public Instrument(Instrument instrument)
        {
        }

        public Instrument(InstrumentType type, string symbol, string description = "", byte currencyId = global::SmartQuant.CurrencyId.USD)
        {
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Description) ? Symbol : string.Format("{0}({1})", Symbol, Description);
        }

        public Instrument Clone(string symbol = null)
        {
            Instrument instrument = new Instrument(this);
            if (symbol != null)
                instrument.Symbol = symbol;
            return instrument;
        }
    }
}

