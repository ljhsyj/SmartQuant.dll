// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class PortfolioManager
    {
        private Framework framework;
        private int nextId;

        public Pricer Pricer { get; set; }

        public PortfolioList Portfolios { get; private set; }

        public Portfolio this [string name]
        {
            get
            {
                return Portfolios[name];
            }
        }

        public PortfolioManager(Framework framework)
        {
            this.framework = framework;
            Pricer = new Pricer(framework);
            Portfolios = new PortfolioList();
        }

        public void Add(Portfolio portfolio)
        {
            Add(portfolio, true);
        }

        public void Remove(string name)
        {
            var portfolio = this[name];
            if (portfolio != null)
                Remove(portfolio);
        }

        public void Remove(int id)
        {
            var portfolio = GetById(id);
            if (portfolio != null)
                Remove(portfolio);
        }

        public void Remove(Portfolio portfolio)
        {
            Portfolios.Remove(portfolio);
            this.framework.EventServer.OnPortfolioDeleted(portfolio);
        }

        public Portfolio GetById(int id)
        {
            return Portfolios.GetById(id);
        }

        public void Clear()
        {
            foreach (var portfolio in Portfolios)
                this.framework.EventServer.OnPortfolioDeleted(portfolio);
            Portfolios.Clear();
            this.nextId = 0;
        }

        private void Add(Portfolio portfolio, bool raiseEvents)
        {
            portfolio.Id = this.nextId++;
            Portfolios.Add(portfolio);
            if (raiseEvents)
                this.framework.EventServer.OnPortfolioAdded(portfolio);
        }

        internal void OnExecutionReport(ExecutionReport report)
        {
            report.Order.Portfolio.OnExecutionReport(report);
        }

        internal void Save(BinaryWriter writer)
        {
            writer.Write(Portfolios.Count);
            foreach (var portfolio in Portfolios)
                this.framework.StreamerManager.Serialize(writer, portfolio);
        }

        internal void Load(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            for (int i = 0; i < num; ++i)
            {
                var portfolio = (Portfolio)this.framework.StreamerManager.Deserialize(reader);
                portfolio.framework = this.framework;
                Add(portfolio);
            }
        }
    }
}
