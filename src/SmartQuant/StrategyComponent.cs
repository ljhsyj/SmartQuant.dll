// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class StrategyComponent
    {
        protected internal Framework framework;

        protected internal ComponentStrategy strategy;

        public Clock Clock
        {
            get
            {
                return this.framework.Clock;
            }
        }

        public InstrumentManager InstrumentManager
        {
            get
            {
                return this.framework.InstrumentManager;
            }
        }

        public ProviderManager ProviderManager
        {
            get
            {
                return this.framework.ProviderManager;
            }
        }

        public OrderManager OrderManager
        {
            get
            {
                return this.framework.OrderManager;
            }
        }

        public BarFactory BarFactory
        {
            get
            {
                return this.EventManager.BarFactory;
            }
        }

        public EventManager EventManager
        {
            get
            {
                return this.framework.EventManager;
            }
        }

        public GroupManager GroupManager
        {
            get
            {
                return this.framework.GroupManager;
            }
        }

        public Instrument Instrument
        {
            get
            {
                return this.strategy.Instrument;
            }
        }

        public Position Position
        {
            get
            {
                return this.strategy.Position;
            }
        }

        public BarSeries Bars
        {
            get
            {
                return this.strategy.Bars;
            }
        }

        public TimeSeries Equity
        {
            get
            {
                return this.strategy.Equity;
            }
        }

        public Portfolio Portfolio
        {
            get
            {
                return this.strategy.Portfolio;
            }
        }

        public StrategyComponent()
        {
        }

        public void AddReminder(DateTime dateTime, object data = null)
        {
            this.Clock.AddReminder(new ReminderCallback(this.OnReminder), dateTime, data);
        }

        public virtual void OnReminder(DateTime dateTime, object data)
        {
            throw new NotImplementedException();
        }
    }
}

