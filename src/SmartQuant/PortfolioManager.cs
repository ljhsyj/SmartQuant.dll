// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class PortfolioManager
    {
        private Framework framework;

        public Pricer Pricer { get; set; }

        public PortfolioList Portfolios { get; private set; }

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
    }
}
