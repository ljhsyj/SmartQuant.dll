using System;

namespace SmartQuant
{
    public class PortfolioPerformance
    {
        internal Portfolio Portfolio { get; private set; }

        public TimeSeries EquitySeries { get; private set; }

        public TimeSeries DrawdownSeries { get; private set; }

        public event EventHandler Updated;

        internal PortfolioPerformance(Portfolio portfolio)
        {
            Portfolio = portfolio;
            EquitySeries = new TimeSeries("Equity", "Equity");
            DrawdownSeries = new TimeSeries("Drawdown", "Drawdown");
        }

        public void Update()
        {
        }

        public void Reset()
        {
        }
    }
}

