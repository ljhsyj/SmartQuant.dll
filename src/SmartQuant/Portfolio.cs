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

        private Portfolio parent;

        internal short Id { get; set; }

        public string Name { get; private set; }

        [Browsable(false)]
        public Account Account { get; private set; }

        public List<Portfolio> Children { get; private set; }

        [Browsable(false)]
        public Portfolio Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if (this.parent == value)
                    return;
                if (this.parent != null)
                    this.parent.Children.Remove(this);
                this.parent = value;
                if (this.parent != null)
                {
                    Account.Parent = this.parent.Account;
                    this.parent.Children.Add(this);
                }
                else
                    Account.Parent = null;
                this.framework.EventServer.OnPortfolioParentChanged(this, true);
            }
        }

        public Pricer Pricer { get; set; }

        [Browsable(false)]
        public List<Transaction> Transactions { get; private set; }

        [Browsable(false)]
        public PortfolioPerformance Performance { get; private set; }

        [Browsable(false)]
        internal IdArray<Position> PositionArray { get; private set; }

        [Browsable(false)]
        public List<Position> Positions { get; private set; }

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
                return Account.Value;
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
            Children = new List<Portfolio>();
            PositionArray = new IdArray<Position>(8192);
            Positions = new List<Position>();
        }

        public void Add(Fill fill)
        {
            throw new NotImplementedException();
        }
          
        internal Position Add(Instrument instrument)
        {
            var position =  PositionArray[instrument.Id];
            if (position == null)
            {
                position = new Position(this, instrument);
                PositionArray[instrument.Id] = position;
                Positions.Add(position);
            }
           return position;
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
