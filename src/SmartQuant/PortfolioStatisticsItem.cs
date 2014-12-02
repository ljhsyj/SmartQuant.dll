// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class PortfolioStatisticsItem
    {
        protected internal double totalValue;
        protected internal double longValue;
        protected internal double shortValue;
        protected internal TimeSeries totalValues;
        protected internal TimeSeries longValues;
        protected internal TimeSeries shortValues;
        protected internal Portfolio portfolio;
        protected internal PortfolioStatistics statistics;

        public virtual int Type
        {
            get
            {
                return 0;
            }
        }

        public virtual string Name
        {
            get
            {
                return "Unknown";
            }
        }

        public virtual string Format
        {
            get
            {
                return "F2";
            }
        }

        public virtual string Description
        {
            get
            {
                return "Unknown";
            }
        }

        public virtual string Category
        {
            get
            {
                return "Unknown";
            }
        }

        public virtual bool Show
        {
            get
            {
                return true;
            }
        }

        public double TotalValue
        {
            get
            {
                return this.totalValue;
            }
        }

        public double LongValue
        {
            get
            {
                return this.longValue;
            }
        }

        public double ShortValue
        {
            get
            {
                return this.shortValue;
            }
        }

        public TimeSeries TotalValues
        {
            get
            {
                return this.totalValues;
            }
        }

        public TimeSeries LongValues
        {
            get
            {
                return this.longValues;
            }
        }

        public TimeSeries ShortValues
        {
            get
            {
                return this.shortValues;
            }
        }

        public Clock Clock
        {
            get
            {
                return this.portfolio.framework.Clock;
            }
        }
            
        public PortfolioStatisticsItem()
        {
            this.totalValues = new TimeSeries(Name, "");
            this.longValues = new TimeSeries(string.Format("{0} Long", Name), "");
            this.shortValues = new TimeSeries(string.Format("{0} Short", Name), "");
        }

        public void Subscribe(int itemType)
        {
            this.statistics.Subscribe(this, itemType);
        }

        public void Unsubscribe(int itemType)
        {
            this.statistics.Unsubscribe(this, itemType);
        }

        protected internal void Emit()
        {
            if (this.statistics == null)
                return;
            this.statistics.OnStatistics(this);
            if (this.portfolio.Parent == null)
                return;
            this.statistics.OnStatistics(this.portfolio, this);  
        }

        protected internal virtual void OnInit()
        {
        }

        protected internal virtual void OnEquity(double equity)
        {
        }

        protected internal virtual void OnFill(Fill fill)
        {
        }

        protected internal virtual void OnTransaction(Transaction transaction)
        {
        }

        protected internal virtual void OnPositionOpened(Position position)
        {
        }

        protected internal virtual void OnPositionClosed(Position position)
        {
        }

        protected internal virtual void OnPositionChanged(Position position)
        {
        }

        protected internal virtual void OnRoundTrip(TradeInfo trade)
        {
        }

        protected internal virtual void OnStatistics(PortfolioStatisticsItem statistics)
        {
        }

        protected internal virtual void OnStatistics(Portfolio portfolio, PortfolioStatisticsItem statistics)
        {
        }

        protected internal virtual void OnClear()
        {
        }
    }
}
