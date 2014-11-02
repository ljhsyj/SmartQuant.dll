
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SmartQuant
{
    public class ExecutionSimulator : Provider, IExecutionSimulator
    {
        public BarFilter BarFilter { get; private set; }

        public ICommissionProvider CommissionProvider { get; set; }

        public ISlippageProvider SlippageProvider { get; set; }

        public bool FillOnQuote { get; set; }

        public bool FillOnTrade { get; set; }

        public bool FillOnBarOpen { get; set; }

        public bool FillOnBar { get; set; }

        public bool FillOnLevel2 { get; set; }

        public bool FillMarketOnNext { get; set; }

        public bool FillLimitOnNext { get; set; }

        public bool FillStopOnNext { get; set; }

        public bool FillStopLimitOnNext { get; set; }

        public bool FillAtStopPrice { get; set; }

        public bool FillAtLimitPrice { get; set; }

        public bool PartialFills { get; set; }

        public bool Queued { get; set; }

        public ExecutionSimulator(Framework framework)
            : base(framework)
        {
            this.BarFilter = new BarFilter();
            this.CommissionProvider = new CommissionProvider();
            this.SlippageProvider = new SlippageProvider();
        }

        public void OnBid(Bid bid)
        {
            throw new NotImplementedException();
        }

        public void OnAsk(Ask ask)
        {
            throw new NotImplementedException();
        }

        public void OnLevel2(Level2Snapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public void OnLevel2(Level2Update update)
        {
            throw new NotImplementedException();
        }

        public void OnTrade(Trade trade)
        {
            throw new NotImplementedException();
        }

        public void OnBar(Bar bar)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
