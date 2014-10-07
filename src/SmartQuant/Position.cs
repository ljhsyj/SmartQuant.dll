// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class Position
    {
        public Portfolio Portfolio { get; private set; }

        public Instrument Instrument { get; private set; }

        public List<Fill> Fills { get; private set; }

        public double Amount { get; private set; }

        public double Qty { get; private set; }

        public double QtyBought { get; private set; }

        public double QtySold { get; private set; }

        public PositionSide Side
        {
            get
            {
                return this.Amount < 0.0 ? PositionSide.Short : PositionSide.Long;
            }
        }

        public double Price
        {
            get
            {
                return Portfolio.Pricer.GetPrice(this);
            }
        }

        public double Value
        {
            get
            {
                return Instrument.Factor != 0.0 ? Price * Amount * Instrument.Factor : Price * Amount;
            }
        }

        public double EntryPrice
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DateTime EntryDate
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double EntryQty
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Position()
            : this(null, null)
        {
        }

        public Position(Portfolio portfolio, Instrument instrument)
        {
            Fills = new List<Fill>();
            Portfolio = portfolio;
            Instrument = instrument;
        }

        public void Add(Fill fill)
        {
            throw new NotImplementedException();
        }

        public string GetSideAsString()
        {
            switch (this.Side)
            {
                case PositionSide.Long:
                    return "Long";
                case PositionSide.Short:
                    return "Short";
                default:
                    return "Undefined";
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.Instrument, this.Side, this.Qty);
        }
    }
}
