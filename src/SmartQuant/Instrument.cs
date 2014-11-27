// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SmartQuant
{
    public class Instrument
    {
        private Framework framework;
        internal bool loaded;

        [Browsable(false)]
        public ObjectTable Fields { get; internal set; }

        [Browsable(false)]
        public Instrument Parent { get; set; }

        [Category("Appearance")]
        [Description("Instrument symbol")]
        public string Symbol { get; internal set; }

        [Category("Appearance")]
        [Description("Unique instrument id in SmartQuant framework")]
        public int Id { get; internal set; }

        [Category("Appearance")]
        [Description("Instrument Type (Stock, Futures, Option, Bond, ETF, Index, etc.)")]
        public InstrumentType Type { get; internal set; }

        [Category("Appearance")]
        [Description("Instrument description")]
        public string Description { get; set; }

        [Category("Appearance")]
        [Description("Instrument exchange")]
        public string Exchange { get; set; }

        [Category("Appearance")]
        [Description("Instrument currency code (USD, EUR, RUR, CAD, etc.)")]
        public byte CurrencyId { get; set; }

        [Category("FX")]
        [Description("Base currency code")]
        public byte CCY1 { get; set; }

        [Category("FX")]
        [Description("Counter currency code")]
        public byte CCY2 { get; set; }

        [Category("TickSize")]
        [Description("Instrument tick size")]
        [DefaultValue(0)]
        public double TickSize { get; set; }

        [Category("Derivative")]
        [Description("Instrument maturity")]
        public DateTime Maturity { get; set; }


        [Category("Derivative")]
        [Description("Contract Value Factor by which price must be adjusted to determine the true nominal value of one futures/options contract. (Qty * Price) * Factor = Nominal Value")]
        [DefaultValue(0)]
        public double Factor { get; set; }

        [Category("Derivative")]
        [Description("Instrument strike price")]
        [DefaultValue(0)]
        public double Strike { get; set; }

        [Category("Derivative")]
        [Description("Option type : put or call")]
        public PutCall PutCall { get; set; }

        [Category("Margin")]
        [Description("Initial margin (used in simulations)")]
        [DefaultValue(0)]
        public double Margin { get; set; }

        [Category("Display")]
        [Description("C# price format string (example: F4 - show four decimal numbers for Forex contracts)")]
        [DefaultValue("F2")]
        public string PriceFormat { get; set; }

        public AltIdList AltId { get; private set; }

        public List<Leg> Legs { get; private set; }

        [Browsable(false)]
        public Bid Bid { get; private set; }

        [Browsable(false)]
        public Ask Ask { get; private set; }

        [Browsable(false)]
        public Trade Trade { get; private set; }

        [Browsable(false)]
        public Bar Bar { get; private set; }

        public IDataProvider DataProvider { get; private set; }

        public IExecutionProvider ExecutionProvider { get; private set; }

        private Instrument()
        {
            Exchange = string.Empty;
            PriceFormat = "F2";
            AltId = new AltIdList();
            Legs = new List<Leg>();
            Fields = new ObjectTable();
        }

        public Instrument(Instrument instrument)
            : this()
        {
        }

        public Instrument(InstrumentType type, string symbol, string description = "", byte currencyId = global::SmartQuant.CurrencyId.USD)
            : this()
        {
            Type = type;
            Symbol = symbol;
            Description = description;
            CurrencyId = currencyId;
        }

        internal void Init(Framework framework)
        {
            this.framework = framework;
            foreach (var leg in Legs)
                leg.Init(this.framework); 
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Description) ? Symbol : string.Format("{0}({1})", Symbol, Description);
        }

        public Instrument Clone(string symbol = null)
        {
            var instrument = new Instrument(this);
            if (symbol != null)
                instrument.Symbol = symbol;
            return instrument;
        }

        #region Extra Helper Methods
        internal string GetName()
        {
            return Symbol;
        }

        internal int GetId()
        {
            return Id;
        }

        #endregion
    }
}