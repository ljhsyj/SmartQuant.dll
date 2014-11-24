// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace SmartQuant
{
	public class Portfolio
	{
        private Framework framework;
        private Account account;

        public string Name { get; private set; }

        public Pricer Pricer { get; set; }

        [Browsable(false)]
        public Account Account { get; private set; }

        [Browsable(false)]
        public List<Transaction> Transactions { get; private set; }

        [Browsable(false)]
        public PortfolioPerformance Performance { get; private set; }

        public double Value 
        {
            get
            {
                return AccountValue + PositionValue;
            }
        }

        public double AccountValue
        {
            get
            {
                return this.account.Value;
            }
        }

        public double PositionValue
        {
            get
            {
                double num = 0;
//                for (int index = 0; index < this.list_1.Count; ++index)
//                    num += this.framework_0.ginterface4_0.Convert(this.list_1[index].Value, this.list_1[index].instrument_0.byte_0, this.account_0.byte_0);
                return num;
            }
        }

        public Portfolio(Framework framework, string name = "")
        {
            this.framework = framework;
            this.Name = name;
        }

        public void Add(Fill fill)
        {
            throw new NotImplementedException();
        }
          
        public void Add(Instrument instrument)
        {
//            Position position = this.idArray_1[instrument_0.int_0];
//            if (position == null)
//            {
//                position = new Position(this, instrument_0);
//                this.idArray_1[instrument_0.Id] = position;
//                this.list_1.Add(position);
//            }
//            return position;
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
