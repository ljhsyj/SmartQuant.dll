using System;

namespace SmartQuant
{
    public class PerformanceStrategy : InstrumentStrategy
    {
        public PerformanceStrategy(Framework framework, string name)
            : base(framework, name)
        {
        }

        protected internal override void OnStrategyStart()
        {
        }

        protected internal override void OnTrade(Instrument instrument, Trade trade)
        {
        }
    }
}

