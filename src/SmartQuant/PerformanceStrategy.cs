using System;

namespace SmartQuant
{
    public class PerformanceStrategy : InstrumentStrategy
    {
        public PerformanceStrategy(Framework framework)
            : base(framework, "PerformanceStrategy")
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

