using System;
using System.Collections.Generic;

namespace SmartQuant
{
    class EventArgs1 : EventArgs
    {
        public TradeInfo TradeInfo { get; set; }
        public EventArgs1(TradeInfo tradeInfo)
        {
            TradeInfo = tradeInfo;
        }
    }

    internal delegate void Delegate1(object sender, EventArgs1 e);

    public class TradeDetector
    {
        public List<TradeInfo> Trades { get; private set; }

        public DateTime OpenDateTime { get; private set; }

        public bool HasPosition { get; private set; }

        internal Portfolio Portfolio { get; set; }

        internal event Delegate1 Detected;

        public TradeDetector(TradeDetectionType type, Portfolio portfolio)
        {
            Portfolio = portfolio;
            Trades = new List<TradeInfo>();
        }

        public void Add(Fill fill)
        {
            throw new NotImplementedException();
        }

        public void OnEquity(double equity)
        {
            throw new NotImplementedException();
        }
    }
}

