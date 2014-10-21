using System;

namespace SmartQuant
{
    public class Fill : DataObject
    {
        public override byte TypeId
        {
            get
            {
                return DataObjectType.Fill;
            }
        }

        public Order Order { get; private set; }

        public Instrument Instrument { get; private set; }

        public byte CurrencyId { get; private set; }

        public OrderSide Side { get; private set; }

        public double Qty { get; private set; }

        public double Price { get; private set; }

        public string Text { get; private set; }

        public double Commission { get; private set; }

        public double Value
        {
            get
            {
                var v = Price * Qty;
                return Instrument.Factor != 0 ? v * Instrument.Factor : v;
            }
        }

        public double NetCashFlow
        {
            get
            {
                return Side == OrderSide.Buy ? -Value : Value;
            }
        }

        public double CashFlow
        {
            get
            {
                return this.NetCashFlow - this.Commission;
            }
        }

        public Fill()
        {
        }

        public Fill(DateTime dateTime, Order order, Instrument instrument, byte currencyId, OrderSide side, double qty, double price, string text = "")
        {
            DateTime = dateTime;
            Order = order;
            Instrument = instrument;
            CurrencyId = currencyId;
            Side = side;
            Qty = qty;
            Price = price;
            Text = text;
        }

        public Fill(ExecutionReport report)
        {
            DateTime = report.DateTime;
            Order = report.Order;
            Instrument = report.Instrument;
            CurrencyId = report.CurrencyId;
            Side = report.Side;
            Qty = report.LastQty;
            Price = report.Price;
            Text = report.Text;
        }

        public Fill(Fill fill)
        {
            DateTime = fill.DateTime;
            Order = fill.Order;
            Instrument = fill.Instrument;
            CurrencyId = fill.CurrencyId;
            Side = fill.Side;
            Qty = fill.Qty;
            Price = fill.Price;
            Commission = fill.Commission;
            Text = fill.Text;
        }

        public string GetSideAsString()
        {
            switch (this.Side)
            {
                case OrderSide.Buy:
                    return "Buy";
                case OrderSide.Sell:
                    return "Sell";
                default:
                    return "Undefined";
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}", DateTime, GetSideAsString(), Instrument.Symbol, Qty, Price, Text);
        }









    }
}

