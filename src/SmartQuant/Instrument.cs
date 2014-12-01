// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SmartQuant
{
    public class Instrument
    {
      //  private Framework framework;
        private ObjectTable fields;

        internal bool Loaded { get; set; }

        internal bool Deleted { get; set; }

        internal Framework Framework { get; private set; }

        [Browsable(false)]
        public ObjectTable Fields
        {
            get
            { 
                if (fields == null)
                    fields = new ObjectTable();
                return fields;
            }
        }

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
        public Bid Bid { get; internal set; }

        [Browsable(false)]
        public Ask Ask { get; internal set; }

        [Browsable(false)]
        public Trade Trade { get; internal set; }

        [Browsable(false)]
        public Bar Bar { get; internal set; }

        public IDataProvider DataProvider { get; private set; }

        public IExecutionProvider ExecutionProvider { get; private set; }

        private Instrument()
        {
            Exchange = string.Empty;
            PriceFormat = "F2";
            AltId = new AltIdList();
            Legs = new List<Leg>();
        }

        public Instrument(Instrument instrument)
            : this()
        {
            throw new NotImplementedException();
        }

        public Instrument(InstrumentType type, string symbol, string description = "", byte currencyId = global::SmartQuant.CurrencyId.USD)
            : this()
        {
            Type = type;
            Symbol = symbol;
            Description = description;
            CurrencyId = currencyId;
        }

        internal Instrument(int id, InstrumentType type, string symbol, string description = "", byte currencyId = global::SmartQuant.CurrencyId.USD)
            : this(type, symbol, description, currencyId)
        {
            Id = id;
        }

        internal void Init(Framework framework)
        {
            Framework = framework;
            foreach (var leg in Legs)
                leg.Init(Framework);
        }

        public string GetSymbol(byte providerId)
        {
            var altId = AltId.Get(providerId);
            return altId != null && !string.IsNullOrEmpty(altId.Symbol) ? altId.Symbol : Symbol;
        }

        public string GetExchange(byte providerId)
        {
            var altId = AltId.Get(providerId);
            return altId != null && !string.IsNullOrEmpty(altId.Exchange) ? altId.Exchange : Exchange;
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

        #region Some Extensions


        internal Instrument(BinaryReader reader)
            : this()
        {
            var version = reader.ReadByte();
            Id = reader.ReadInt32();
            Type = (InstrumentType)reader.ReadByte();
            Symbol = reader.ReadString();
            Description = reader.ReadString();
            CurrencyId = reader.ReadByte();
            Exchange = reader.ReadString();
            TickSize = reader.ReadDouble();
            Maturity = new DateTime(reader.ReadInt64());
            Factor = reader.ReadDouble();
            Strike = reader.ReadDouble();
            PutCall = (PutCall)reader.ReadByte();
            Margin = reader.ReadDouble();
            int altIdCount = reader.ReadInt32();
            for (int i = 0; i < altIdCount; ++i)
                AltId.Add((AltId)Framework.StreamerManager.Deserialize(reader));
            int legCount = reader.ReadInt32();
            for (int i = 0; i < legCount; ++i)
                Legs.Add((Leg)Framework.StreamerManager.Deserialize(reader));
            this.fields = new ObjectTable();
            if (version == 0)
            {
                int fieldCount = reader.ReadInt32();
                for (int i = 0; i < fieldCount; ++i)
                    this.fields[i] = (object)reader.ReadDouble();
            }
            if (version >= 1)
            {
                PriceFormat = reader.ReadString();
                if (reader.ReadInt32() != -1)
                    this.fields = (ObjectTable)Framework.StreamerManager.Deserialize(reader);
            }
            if (version >= 2)
            {
                CCY1 = reader.ReadByte();
                CCY2 = reader.ReadByte();
            }
            if (version >= 3)
                Deleted = reader.ReadBoolean();

        }

        internal void Write(BinaryWriter writer, StreamerManager streamerManager)
        {
            var instrument = this;
            byte version = 2;
            writer.Write(version);
            writer.Write(instrument.Id);
            writer.Write((byte)instrument.Type);
            writer.Write(instrument.Symbol);
            writer.Write(instrument.Description);
            writer.Write(instrument.CurrencyId);
            writer.Write(instrument.Exchange);
            writer.Write(instrument.TickSize);
            writer.Write(instrument.Maturity.Ticks);
            writer.Write(instrument.Factor);
            writer.Write(instrument.Strike);
            writer.Write((byte)instrument.PutCall);
            writer.Write(instrument.Margin);
            writer.Write(instrument.AltId.Count);
            foreach (var altId in instrument.AltId)
                streamerManager.Serialize(writer, altId);
            writer.Write(instrument.Legs.Count);
            foreach (var leg in instrument.Legs)
                streamerManager.Serialize(writer, leg);
            if (version == 0)
            {
                writer.Write(instrument.Fields.Size);
                for (int i = 0; i < instrument.Fields.Size; ++i)
                    writer.Write((double)instrument.Fields[i]);
            }
            if (version >= 1)
            {
                writer.Write(instrument.PriceFormat);
                if (instrument.Fields == null)
                    writer.Write(-1);
                else
                {
                    writer.Write(instrument.Fields.Size);
                    streamerManager.Serialize(writer, instrument.Fields);
                }
            }
            if (version >= 2)
            {
                writer.Write(instrument.CCY1);
                writer.Write(instrument.CCY2);
            }
            if (version >= 3)
                writer.Write(instrument.Deleted);
        }

        internal string GetName()
        {
            return Symbol;
        }

        internal int GetId()
        {
            return Id;
        }

        internal void TryInitWith(Framework framework)
        {
            if (Framework == null)
                Init(framework);
        }
        #endregion
    }
}