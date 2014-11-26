// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
	public class StrategyManager
	{
        private Framework framework;
        private byte nextId;     
        private StrategyMode mode;
        private Strategy strategy;
        private StrategyStatus status;

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
            this.Mode = StrategyMode.Backtest;
            this.status = StrategyStatus.Stopped;
            Global = new Global();
            Clear();
        }

        public byte GetNextId()
        {
            return nextId++;
        }

        public void StartStrategy(Strategy strategy)
        {
            this.StartStrategy(strategy, this.Mode);
        }

        public void StartStrategy(Strategy strategy, StrategyMode mode)
        {
            this.strategy = strategy;
            this.Mode = mode;
        }

        public void Stop()
        {
        }

        public void RegisterMarketDataRequest(IDataProvider dataProvider, InstrumentList instrumentList)
        {
            this.framework.SubscriptionManager.Subscribe(dataProvider, instrumentList);
        }

        public void Clear()
        {
            this.nextId = 101;
            this.Global.Clear();
        }
	}
}
