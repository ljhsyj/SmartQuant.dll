using System;
using System.Linq;
using System.Collections.Generic;

namespace SmartQuant
{
    public class Transaction
    {
        private List<Fill> fills = new List<Fill>();

        public List<Fill> Fills
        {
            get
            {
                return this.fills;
            }
        }

        public Instrument Instrument
        {
            get
            {
                return Fills[0].Instrument;
            }
        }

        public Order Order
        {
            get
            {
                return Fills[0].Order;
            }
        }

        public OrderSide Side
        {
            get
            {
                return Fills[0].Side;
            }
        }

        public string Text
        {
            get
            {
                return Fills[0].Order.Text;
            }
        }

        public double Price { get; private set; }

        public double Qty { get; private set; }

        public double Commission { get; private set; }

        public bool IsDone { get; internal set; }

        public double Amount
        {
            get
            {
                return Side == OrderSide.Buy ? Qty : -Qty;
            }
        }

        public virtual double Value
        {
            get
            {
                var v = Qty * Price;
                return Instrument.Factor != 0 ? v * Instrument.Factor : v;
            }
        }

        public virtual double NetCashFlow
        {
            get
            {
                return Instrument.Factor != 0 ? Amount * Price * Instrument.Factor : Amount * Price;
            }
        }

        public virtual double CashFlow
        {
            get
            {
                return NetCashFlow - Commission;
            }
        }

        public Transaction()
        {
        }

        public Transaction(Fill fill)
        {
            Add(fill);
        }

        public void Add(Fill fill)
        {
            Fills.Add(fill);
            Qty += fill.Qty;
            Commission += fill.Commission;
            Price = Fills.Sum(f => f.Qty * f.Price) / Qty;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {4}", Side, Qty, Instrument.Symbol, Price);
        }
    }
}

