// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class Strategy
    {
        protected internal Framework framework;
        protected bool raiseEvents;
        private LinkedList<Strategy> strategies;

        public byte Id { get; private set; }

        public string Name { get; private set; }

        public StrategyStatus Status { get; private set; }

        public Portfolio Portfolio { get; private set; }

        public InstrumentList Instruments { get; private set; }

        public BarSeries Bars { get; private set; }

        public TimeSeries Equity { get; private set; }

        public TickSeries Bids { get; private set; }

        public TickSeries Asks { get; private set; }

        public IDataSimulator DataSimulator
        {
            get
            {
                return this.ProviderManager.DataSimulator;
            }
        }

        public IExecutionSimulator ExecutionSimulator
        { 
            get
            {
                return this.ProviderManager.ExecutionSimulator;
            }
        }

        public bool Enabled { get; set; }

        public Strategy Parent { get; private set; }

        public LinkedList<Strategy> Strategies { get; private set; }

        public StrategyManager StrategyManager
        { 
            get
            {
                return this.framework.StrategyManager;
            }
        }

        public virtual IDataProvider DataProvider
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual IExecutionProvider ExecutionProvider
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Clock Clock
        {
            get
            {
                return this.framework.Clock;
            }
        }

        public StrategyMode Mode
        {
            get
            { 
                return this.framework.StrategyManager.Mode;
            }
        }


        public InstrumentManager InstrumentManager
        {
            get
            {
                return this.framework.InstrumentManager;
            }
        }

        public DataManager DataManager
        {
            get
            {
                return this.framework.DataManager;
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

        public EventManager EventManager
        {
            get
            {
                return this.framework.EventManager;
            }
        }

        public BarFactory BarFactory
        {
            get
            {
                return this.EventManager.BarFactory;
            }
        }

        public GroupManager GroupManager
        {
            get
            {
                return this.framework.GroupManager;
            }
        }

        public Global Global
        {
            get
            {
                return this.framework.StrategyManager.Global;
            }
        }

        public Strategy(Framework framework, string name)
        {
            this.framework = framework;
            Name = name;
            Enabled = true;
            Strategies = new LinkedList<Strategy>();
            Instruments = new InstrumentList();
        }

        public void AddReminder(DateTime dateTime, object data = null)
        {
            this.framework.Clock.AddReminder((dt, obj) => OnReminder(dt, obj), dateTime, data);
        }

        public void AddExchangeReminder(DateTime dateTime, object data = null)
        {
            this.framework.ExchangeClock.AddReminder((dt, obj) => OnExchangeReminder(dt, obj), dateTime, data);
        }

        public void AddInstruments(string[] symbols)
        {
            foreach (string symbol in symbols)
                AddInstrument(this.framework.InstrumentManager.Get(symbol));
        }

        public void AddInstruments(InstrumentList instruments)
        {
            foreach (Instrument instrument in instruments)
                AddInstrument(instrument);
        }

        public void AddInstrument(string symbol)
        {      
            AddInstrument(this.framework.InstrumentManager.Get(symbol));
        }

        public void AddInstrument(int id)
        {
            AddInstrument(this.framework.InstrumentManager.GetById(id));
        }

        public virtual void AddInstrument(Instrument instrument)
        {
            if (Instruments.Contains(instrument))
                Console.WriteLine("Strategy::AddInstrument {0} is already added", instrument);
            else
            {
                Instruments.Add(instrument);
                Portfolio.Add(instrument);
                if (Status == StrategyStatus.Running)
                    ;
                    //  this.method_1(instrument);
            }  
        }

        public virtual void RemoveInstrument(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public bool HasPosition(Instrument instrument)
        {
            return this.Portfolio.HasPosition(instrument);
        }

        public bool HasPosition(Instrument instrument, PositionSide side, double qty)
        {
            return this.Portfolio.HasPosition(instrument, side, qty);
        }

        public bool HasLongPosition(Instrument instrument)
        {
            return this.Portfolio.HasLongPosition(instrument);
        }

        public bool HasLongPosition(Instrument instrument, double qty)
        {
            return this.Portfolio.HasLongPosition(instrument, qty);
        }

        public bool HasShortPosition(Instrument instrument)
        {
            return this.Portfolio.HasShortPosition(instrument);
        }

        public bool HasShortPosition(Instrument instrument, double qty)
        {
            return this.Portfolio.HasShortPosition(instrument, qty);
        }

        public void AddStrategy(Strategy strategy)
        {
        }

        public void AddStrategy(Strategy strategy, bool callOnStrategyStart)
        {
        }

        public void Deposit(DateTime dateTime, double value, byte currencyId = global::SmartQuant.CurrencyId.USD, string text = null, bool updateParent = true)
        {
            Portfolio.Account.Deposit(dateTime, value, currencyId, text, updateParent);
        }

        public void Withdraw(DateTime dateTime, double value, byte currencyId = global::SmartQuant.CurrencyId.USD, string text = null, bool updateParent = true)
        {
            Portfolio.Account.Withdraw(dateTime, value, currencyId, text, updateParent);
        }

        public void Deposit(double value, byte currencyId = global::SmartQuant.CurrencyId.USD, string text = null, bool updateParent = true)
        {
            Portfolio.Account.Deposit(value, currencyId, text, updateParent);
        }

        public void Withdraw(double value, byte currencyId = global::SmartQuant.CurrencyId.USD, string text = null, bool updateParent = true)
        {
            Portfolio.Account.Withdraw(value, currencyId, text, updateParent);
        }

        public virtual void Init()
        {
        }

        public virtual double Objective()
        {
            throw new NotImplementedException();
        }

        public void Log(DataObject data, Group group)
        {
            this.framework.EventServer.OnLog(new GroupEvent(data, group));
        }

        public void Log(DataObject data, int groupId)
        {
            throw new NotImplementedException();
        }

        public void Log(DateTime dateTime, double value, Group group)
        {
            this.framework.EventServer.OnLog(new GroupEvent(new TimeSeriesItem(dateTime, value), group));
        }

        public void Log(DateTime dateTime, double value, int groupId)
        {
            throw new NotImplementedException();
        }

        public void Log(DateTime dateTime, string text, Group group)
        {
            this.framework.EventServer.OnLog(new GroupEvent(new TextInfo(dateTime, text), group));
        }

        public void Log(DateTime dateTime, string text, int groupId)
        {
            throw new NotImplementedException();
        }

        public void Log(double value, Group group)
        {
            this.framework.EventServer.OnLog(new GroupEvent(new TimeSeriesItem(this.framework.Clock.DateTime, value), group));
        }

        public void Log(double value, int groupId)
        {
            throw new NotImplementedException();
        }

        public void Log(string text, Group group)
        {
            this.framework.EventServer.OnLog(new GroupEvent(new TextInfo(this.framework.Clock.DateTime, text), group));
        }

        public void Log(string text, int groupId)
        {
            throw new NotImplementedException();
        }

        protected internal virtual void OnStrategyInit()
        {
        }

        protected internal virtual void OnStrategyStart()
        {
        }

        protected internal virtual void OnStrategyStop()
        {
        }

        protected internal virtual void OnStrategyEvent(object data)
        {
        }

        protected internal virtual void OnReminder(DateTime dateTime, object data)
        {
        }

        protected internal virtual void OnExchangeReminder(DateTime dateTime, object data)
        {
        }

        protected internal virtual void OnProviderConnected(Provider provider)
        {
        }

        protected internal virtual void OnProviderDisconnected(Provider provider)
        {
        }

        protected internal virtual void OnBid(Instrument instrument, Bid bid)
        {
        }

        protected internal virtual void OnAsk(Instrument instrument, Ask ask)
        {
        }

        protected internal virtual void OnTrade(Instrument instrument, Trade trade)
        {
        }

        protected internal virtual void OnLevel2(Instrument instrument, Level2Snapshot snapshot)
        {
        }

        protected internal virtual void OnLevel2(Instrument instrument, Level2Update update)
        {
        }

        protected internal virtual void OnBarOpen(Instrument instrument, Bar bar)
        {
        }

        protected internal virtual void OnBar(Instrument instrument, Bar bar)
        {
        }

        protected internal virtual void OnBarSlice(BarSlice slice)
        {
        }

        protected internal virtual void OnNews(Instrument instrument, News news)
        {
        }

        protected internal virtual void OnFundamental(Instrument instrument, Fundamental fundamental)
        {
        }

        protected internal virtual void OnExecutionReport(ExecutionReport report)
        {
        }

        protected internal virtual void OnSendOrder(Order order)
        {
        }

        protected internal virtual void OnPendingNewOrder(Order order)
        {
        }

        protected internal virtual void OnNewOrder(Order order)
        {
        }

        protected internal virtual void OnOrderStatusChanged(Order order)
        {
        }

        protected internal virtual void OnOrderFilled(Order order)
        {
        }

        protected internal virtual void OnOrderPartiallyFilled(Order order)
        {
        }

        protected internal virtual void OnOrderCancelled(Order order)
        {
        }

        protected internal virtual void OnOrderReplaced(Order order)
        {
        }

        protected internal virtual void OnOrderDone(Order order)
        {
        }

        protected internal virtual void OnFill(Fill fill)
        {
        }

        protected internal virtual void OnTransaction(Transaction transaction)
        {
        }

        protected internal virtual void OnPositionOpened(Position position)
        {
        }

        protected internal virtual void OnPositionClosed(Position position)
        {
        }

        protected internal virtual void OnPositionChanged(Position position)
        {
        }

        protected internal virtual void OnStopExecuted(Stop stop)
        {
        }

        protected internal virtual void OnStopCancelled(Stop stop)
        {
        }

        protected internal virtual void OnStopStatusChanged(Stop stop)
        {
        }

        protected internal virtual void OnUserCommand(string command)
        {
        }

        public void SendStrategyEvent(object data)
        {
            this.framework.EventServer.OnEvent(new OnStrategyEvent(data));
        }

        public void Send(Order order)
        {
            this.framework.OrderManager.Send(order);
        }

        public void Cancel(Order order)
        {
            this.framework.OrderManager.Cancel(order);
        }

        public void CancelAll()
        {
            throw new NotImplementedException();
        }

        public void CancelAll(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public void Reject(Order order)
        {
            this.framework.OrderManager.Reject(order);
        }

        public void Replace(Order order, double price)
        {
        }

        public Order BuyOrder(Instrument instrument, double qty, string text = "")
        {
            return null;
//            Order order = new Order(this.method_3(instrument), this.portfolio_0, instrument, OrderType.Market, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, (byte) 0, "");
//            order.strategyId = (int) this.byte_0;
//            order.string_1 = text;
//            this.framework.orderManager_0.Register(order);
//            return order;
        }

        public Order SellOrder(Instrument instrument, double qty, string text = "")
        {
            return null;
//            Order order = new Order(this.method_3(instrument), this.portfolio_0, instrument, OrderType.Market, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, (byte) 0, "");
//            order.strategyId = (int) this.byte_0;
//            order.string_1 = text;
//            this.framework.orderManager_0.Register(order);
//            return order;
        }

        public Order BuyLimitOrder(Instrument instrument, double qty, double price, string text = "")
        {
            throw new NotImplementedException();

        }

        public Order SellLimitOrder(Instrument instrument, double qty, double price, string text = "")
        {
            throw new NotImplementedException();

        }

        public string GetStatusAsString()
        {
            switch (this.Status)
            {
                case StrategyStatus.Running:
                    return "Running";
                case StrategyStatus.Stopped:
                    return "Stopped";
                default:
                    return "Undefined";
            }
        }

        public string GetModeAsString()
        {
            switch (this.Mode)
            {
                case StrategyMode.Backtest:
                    return "Backtest";
                case StrategyMode.Paper:
                    return "Paper";
                case StrategyMode.Live:
                    return "Live";
                default:
                    return "Undefined";
            }
        }
    }
}
