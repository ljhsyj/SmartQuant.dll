// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class PortfolioStatistics
    {
        private Portfolio portfolio;

        public PortfolioStatisticsItemList Items { get; private set; }

        internal PortfolioStatistics(Portfolio portfolio)
        {
            this.portfolio = portfolio;
            this.Items = new PortfolioStatisticsItemList();
        }

        public void Add(PortfolioStatisticsItem item)
        {
        }

        public PortfolioStatisticsItem Get(int type)
        {
            return Items.GetByType(type);
        }
    }
}
