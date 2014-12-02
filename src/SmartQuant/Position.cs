// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class Position
    {
        private Fill entryFill;

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
                return Amount < 0 ? PositionSide.Short : PositionSide.Long;
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
                return Instrument.Factor != 0 ? Price * Amount * Instrument.Factor : Price * Amount;
            }
        }

        public double EntryPrice
        {
            get
            {
                return this.entryFill.Price;
            }
        }

        public DateTime EntryDate
        {
            get
            {
                return this.entryFill.DateTime;
            }
        }

        public double EntryQty
        {
            get
            {
                return this.entryFill.Qty;
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
            Fills.Add(fill);
            if (Qty == 0)
                this.entryFill = fill;
            if (fill.Side == OrderSide.Buy)
                QtyBought += fill.Qty;
            else
                QtySold += fill.Qty;
            Amount = QtyBought - QtySold;
            Qty = Math.Abs(Amount);
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
            return string.Format("{0} {1} {2}", Instrument, Side, Qty);
        }
    }
}
