// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartQuant
{
    public class StrategyManager
    {
        private Framework framework;
        private byte nextId;
        private StrategyMode mode;
        private Strategy strategy;
        private StrategyStatus status;
        private Dictionary<IDataProvider, InstrumentList> subscriptions;

        public StrategyMode Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                if (this.mode == value)
                    return;
                this.mode = value;
                this.framework.Mode = this.mode == StrategyMode.Backtest ? FrameworkMode.Simulation : FrameworkMode.Realtime;
            }
        }

        public Global Global { get; private set; }

        public StrategyManager(Framework framework)
        {
            this.framework = framework;
            this.mode = StrategyMode.Backtest;
            this.status = StrategyStatus.Stopped;
            this.nextId = 101;
            this.subscriptions = new Dictionary<IDataProvider, InstrumentList>();
            Global = new Global();
        }

        public byte GetNextId()
        {
            return nextId++;
        }

        public void StartStrategy(Strategy strategy)
        {
            StartStrategy(strategy, Mode);
        }

        public void StartStrategy(Strategy strategy, StrategyMode mode)
        {
            this.strategy = strategy;
            Mode = mode;

            if (this.framework.Mode == FrameworkMode.Simulation)
            {
                this.framework.Clock.DateTime = this.framework.ProviderManager.DataSimulator.DateTime1;
                this.framework.ExchangeClock.DateTime = DateTime.MinValue;
            }
            if (this.framework.EventManager.Status != EventManagerStatus.Running)
                this.framework.EventManager.Start();
            this.framework.EventServer.OnLog(new GroupEvent(new StrategyStatusInfo(this.framework.Clock.DateTime, StrategyStatusType.Started) { Solution = strategy.Name == null ? "Solution" : strategy.Name, Mode = mode.ToString() }, null));
            strategy.Init();
            strategy.Start();
//            if (!this.framework.IsExternalDataQueue)
//            {
//                var dictionary1 = new Dictionary<IDataProvider, InstrumentList>();
//                while (this.subscriptions.Count != 0)
//                {
//                    var dictionary2 = new Dictionary<IDataProvider, InstrumentList>(this.subscriptions);
//                    this.subscriptions.Clear();
//                    foreach (var keyValuePair in new Dictionary<IDataProvider, InstrumentList>(dictionary2))
//                    {
//                        InstrumentList instrumentList = null;
//                        if (!dictionary1.TryGetValue(keyValuePair.Key, out instrumentList))
//                        {
//                            instrumentList = new InstrumentList();
//                            dictionary1[keyValuePair.Key] = instrumentList;
//                        }
//                        InstrumentList instruments = new InstrumentList();
//                        foreach (Instrument instrument in keyValuePair.Value)
//                        {
//                            if (!instrumentList.Contains(instrument))
//                            {
//                                instrumentList.Add(instrument);
//                                instruments.Add(instrument);
//                            }
//                        }
//                        if (keyValuePair.Key is SellSideStrategy && this.framework.SubscriptionManager != null)
//                            this.framework.SubscriptionManager.Subscribe(keyValuePair.Key, instruments);
//                    }
//                }
//                this.status = StrategyStatus.Running;
//                this.subscriptions = dictionary1;
//                if (this.subscriptions.Count == 0)
//                {
//                    Console.WriteLine("{0} StrategyManager::StartStrategy {1} has no data requests, stopping...", DateTime.Now, strategy.Name);
//                    StopStrategy();
//                }
//                else
//                {
//                    foreach (var subscription in this.subscriptions)
//                    {
//                        if (!(subscription.Key is SellSideStrategy) && this.framework.SubscriptionManager != null)
//                            this.framework.SubscriptionManager.Subscribe(subscription.Key, subscription.Value);
//                    }
//                }
//            }
//            else
                this.status = StrategyStatus.Running;
        }

        private void StopStrategy()
        {
            Console.WriteLine("{0} StrategyManager::StopStrategy {1}", DateTime.Now, this.strategy.Name);
            this.framework.EventServer.OnLog(new GroupEvent(new StrategyStatusInfo(this.framework.Clock.DateTime, StrategyStatusType.Stopped) { Solution = this.strategy.Name == null ? "Solution" : this.strategy.Name, Mode = Mode.ToString() }, null));
            if (!this.framework.IsExternalDataQueue)
            {
                foreach (var subscription in this.subscriptions)
                    this.framework.SubscriptionManager.Unsubscribe(subscription.Key, subscription.Value);
            }
            if (this.strategy.Status == StrategyStatus.Stopped)
            {
                Console.WriteLine("StrategyManager::Stop Error: Can not stop stopped strategy ({0})", this.strategy.Name);
            }
            else
            {
                this.strategy.Stop();
                if (this.framework.Mode == FrameworkMode.Simulation)
                {
                    this.framework.ProviderManager.DataSimulator.Disconnect();
                    this.framework.ProviderManager.ExecutionSimulator.Disconnect();
                }
                if (this.strategy.DataProvider != null)
                    this.strategy.DataProvider.Disconnect();
                if (this.strategy.ExecutionProvider != null)
                    this.strategy.ExecutionProvider.Disconnect();
                this.status = StrategyStatus.Stopped;
            }
        }

        public void Stop()
        {
            if (this.status != StrategyStatus.Stopped)
            {
                this.status = StrategyStatus.Stopped;
                StopStrategy();
            }
            Clear();
        }

        public void Clear()
        {
            this.nextId = 101;
            this.subscriptions.Clear();
            Global.Clear();
        }

        #region MarketData

        class InstrumentComparer : IEqualityComparer<Instrument>
        {
            public int GetHashCode(Instrument i)
            {
                return i.Id.GetHashCode();
            }

            public bool Equals(Instrument x, Instrument y)
            {
                return x.Id == y.Id;
            }
        }

        public void RegisterMarketDataRequest(IDataProvider provider, InstrumentList instruments)
        {
            InstrumentList subscribed = null;
            if (!this.subscriptions.TryGetValue(provider, out subscribed))
            {
                subscribed = new InstrumentList();
                this.subscriptions[provider] = subscribed;
            }
            var newInstruments = new InstrumentList(instruments.Except(subscribed, new InstrumentComparer()));
            subscribed.Add(newInstruments);

//            if (this.status == StrategyStatus.Running && newInstruments.Count > 0 && this.framework.SubscriptionManager != null)
//                this.framework.SubscriptionManager.Subscribe(provider, newInstruments);
            if (newInstruments.Count > 0 && this.framework.SubscriptionManager != null)
                this.framework.SubscriptionManager.Subscribe(provider, newInstruments);
        }

        internal void UnregisterMarketDataRequest(IDataProvider povider, InstrumentList instruments)
        {
            if (this.status == StrategyStatus.Running && instruments.Count > 0 && this.framework.SubscriptionManager != null)
                this.framework.SubscriptionManager.Unsubscribe(povider, instruments);
        }

        #endregion
    }
}
