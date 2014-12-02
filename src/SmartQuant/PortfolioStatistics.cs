// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Linq;

namespace SmartQuant
{
    public class PortfolioStatistics
    {
        private Portfolio portfolio;

        public PortfolioStatisticsItemList Items { get; private set; }

        internal PortfolioStatistics(Portfolio portfolio)
        {
            this.portfolio = portfolio;
            Items = new PortfolioStatisticsItemList();
            foreach (var item in Items)
                Add(item);
            throw new NotImplementedException();
        }

        public void Add(PortfolioStatisticsItem item)
        {
        }

        public PortfolioStatisticsItem Get(int type)
        {
            return Items.GetByType(type);
        }

        internal void Subscribe(PortfolioStatisticsItem item, int type)
        {
            throw new NotImplementedException();
        }

        internal void Unsubscribe(PortfolioStatisticsItem item, int type)
        {  
            throw new NotImplementedException();
        }

        internal void Add(Fill fill)
        {
            throw new NotImplementedException();
        }

        internal void OnFill(Fill fill)
        {
            Add(fill);
            foreach (var item in Items)
                item.OnFill(fill);
        }

        internal void OnTransaction(Transaction transaction)
        {
            foreach (var item in Items)
                item.OnTransaction(transaction);
        }

        internal void OnPositionOpened(Position position)
        {
            foreach (var item in Items)
                item.OnPositionOpened(position);
        }

        internal void OnPositionClosed(Position position)
        {
            foreach (var item in Items)
                item.OnPositionClosed(position);
        }

        internal void OnPositionChanged(Position position)
        {
            foreach (var item in Items)
                item.OnPositionChanged(position);
        }

        internal void OnStatistics(PortfolioStatisticsItem item)
        {
            throw new NotImplementedException();
        }

        internal void OnStatistics(Portfolio portfolio, PortfolioStatisticsItem item)
        {
            foreach (var i in Items)
                if (i != item)
                    i.OnStatistics(portfolio, item);
        }

        internal void OnClear()
        {
            foreach (var item in Items)
                item.OnClear();
        }
    }
}
