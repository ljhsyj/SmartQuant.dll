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
            Clock.AddReminder(new ReminderCallback(this.OnReminder), dateTime, data);
        }

        public virtual void OnReminder(DateTime dateTime, object data)
        {
            throw new NotImplementedException();
        }

        public void Buy (double qty)
        {
            throw new NotImplementedException();
        }
        public void Buy (double qty, string text)
        {
            throw new NotImplementedException();
        }
        public void BuyLimit (double qty, double price)
        {
            throw new NotImplementedException();
        }
        public void BuyLimit (double qty, double price, string text)
        {
            throw new NotImplementedException();
        }
        public void BuyStop (double qty, double stopPx)
        {
            throw new NotImplementedException();
        }
        public void BuyStop (double qty, double stopPx, string text)
        {
            throw new NotImplementedException();
        }
        public bool HasLongPosition ()
        {
            throw new NotImplementedException();
        }
        public bool HasLongPosition (double qty)
        {
            throw new NotImplementedException();
        }
        public bool HasPosition ()
        {
            throw new NotImplementedException();
        }
        public bool HasPosition (PositionSide side, double qty)
        {
            throw new NotImplementedException();
        }
        public bool HasShortPosition ()
        {
            throw new NotImplementedException();
        }
        public bool HasShortPosition (double qty)
        {
            throw new NotImplementedException();
        }
        public void Log (DateTime dateTime, double value, int groupId)
        {
            throw new NotImplementedException();
        }
        public void Log (DateTime dateTime, double value, Group group)
        {
            throw new NotImplementedException();
        }
        public void Log (DateTime dateTime, string text, int groupId)
        {
            throw new NotImplementedException();
        }
        public void Log (DateTime dateTime, string text, Group group)
        {
            throw new NotImplementedException();
        }
        public void Log (double value, int groupId)
        {
            throw new NotImplementedException();
        }
        public void Log (double value, Group group)
        {
            throw new NotImplementedException();
        }
        public void Log (string text, int groupId)
        {
            throw new NotImplementedException();
        }
        public void Log (string text, Group group)
        {
            throw new NotImplementedException();
        }
        public void Log (DataObject data, int groupId)
        {
            throw new NotImplementedException();
        }
        public void Log (DataObject data, Group group)
        {
            throw new NotImplementedException();
        }
        public virtual void OnStrategyStart ()
        {
            throw new NotImplementedException();
        }
        public void Sell (double qty)
        {
            throw new NotImplementedException();
        }
        public void Sell (double qty, string text)
        {
            throw new NotImplementedException();
        }
        public void SellLimit (double qty, double price)
        {
            throw new NotImplementedException();
        }
        public void SellLimit (double qty, double price, string text)
        {
            throw new NotImplementedException();
        }
        public void SellStop (double qty, double stopPx)
        {
            throw new NotImplementedException();
        }
        public void SellStop (double qty, double stopPx, string text)
        {
            throw new NotImplementedException();
        }
        public Stop SetStop (double level, StopType type, StopMode mode)
        {
            throw new NotImplementedException();
        }
        public void Signal (double value)
        {
            throw new NotImplementedException();
        }
    }
}

