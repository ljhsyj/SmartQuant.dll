// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Linq;
using System.Collections.Generic;

namespace SmartQuant
{
    public class PortfolioStatistics
    {
        private Portfolio portfolio;
        internal IdArray<TradeDetector> detectors = new IdArray<TradeDetector>(8192);
        internal IdArray<List<int>> idArray_1 = new IdArray<List<int>>(1024);

        public PortfolioStatisticsItemList Items { get; private set; }

        internal PortfolioStatistics(Portfolio portfolio)
        {
            this.portfolio = portfolio;
            Items = new PortfolioStatisticsItemList();
            foreach (var item in Items)
                Add(item);
        }

        public void Add(PortfolioStatisticsItem item)
        {
            if (item.statistics != null)
            {
                Console.WriteLine("PortfolioStatistics::Add Error. Item already belongs to other statistics {0}", item);
                return;
            }

            item.statistics = this;
            item.portfolio = this.portfolio;
            Items.Add(item);
            item.OnInit();
        }

        public PortfolioStatisticsItem Get(int type)
        {
            return Items.GetByType(type);
        }

        internal void Subscribe(PortfolioStatisticsItem item, int type)
        {
            if (Items.GetByType(type) == null)
                Add(this.portfolio.framework.StatisticsManager.Clone(type));
            if (this.idArray_1[type] == null)
                this.idArray_1[type] = new List<int>();
            else if (this.idArray_1[type].Contains(item.Type))
            {
                Console.WriteLine("PortfolioStatistics::Subscribe Item {0} is already subscribed for item {1}", item.Type, type);
                return;
            }
            this.idArray_1[type].Add(item.Type);
        }

        internal void Unsubscribe(PortfolioStatisticsItem item, int type)
        {  
            if (this.idArray_1[type] != null && this.idArray_1[type].Contains(item.Type))
                this.idArray_1[type].Remove(item.Type);
            else
                Console.WriteLine("PortfolioStatistics::Unsubscribe Item {0} is not subscribed for item {1}", item.Type, type);
        }

        internal void Add(Fill fill)
        {
            var id = fill.Instrument.Id;
            if (this.detectors[id] == null)
            {
                var detector = new TradeDetector(TradeDetectionType.FIFO, this.portfolio);
                detector.Detected += (sender, e) =>
                {
                    var info = e.TradeInfo;
                    foreach (var item in Items)
                        item.OnRoundTrip(info);
                };
                this.detectors[id] = detector;
            }
            this.detectors[id].Add(fill);
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
