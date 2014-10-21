// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.ComponentModel;

namespace SmartQuant
{
	public class Portfolio
	{
        private Framework framework;

        public string Name { get; private set; }

        public Pricer Pricer { get; set; }

        [Browsable(false)]
        public Account Account { get; private set; }

        public Portfolio(Framework framework, string name = "")
        {
            this.framework = framework;
            this.Name = name;
        }

        public void Add(Fill fill)
        {
            throw new NotImplementedException();
        }
          
        public bool HasPosition(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public bool HasPosition(Instrument instrument, PositionSide side, double qty)
        {
            throw new NotImplementedException();
        }

        public bool GetPosition(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public bool HasLongPosition(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public bool HasLongPosition(Instrument instrument, double qty)
        {
            throw new NotImplementedException();
        }

        public bool HasShortPosition(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public bool HasShortPosition(Instrument instrument, double qty)
        {
            throw new NotImplementedException();
        }
	}
}
