// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class PortfolioManager
    {
        private Framework framework;

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
            this.Pricer = new Pricer(framework);
            this.Portfolios = new PortfolioList();
        }

        public void Add(Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public void Remove(string name)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public Portfolio GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            Portfolios.Clear();
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
