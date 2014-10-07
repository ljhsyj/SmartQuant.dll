using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class TradeDetector
    {
        public List<TradeInfo> Trades { get; private set; }

        public DateTime OpenDateTime { get; private set; }

        public bool HasPosition { get; private set; }

        internal Portfolio Portfolio { get; set; }

        public TradeDetector(TradeDetectionType type, Portfolio portfolio)
        {
            Portfolio = portfolio;
            Trades = new List<TradeInfo>();
        }

        public void Add(Fill fill)
        {
        }

        public void OnEquity(double equity)
        {
        }
    }
}

