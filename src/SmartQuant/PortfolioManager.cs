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
            this.Pricer = new Pricer();
            this.Portfolios = new PortfolioList();
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
	}

}
