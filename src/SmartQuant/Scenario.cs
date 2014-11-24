// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;

namespace SmartQuant
{
    public class Scenario
    {
        protected Framework framework;
        protected Strategy strategy;

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

        public DataManager DataManager
        { 
            get
            {
                return this.framework.DataManager;
            }
        }

        public ProviderManager ProviderManager { get { return this.framework.ProviderManager; } }

        public OrderManager OrderManager { get { return this.framework.OrderManager; } }

        public IDataSimulator DataSimulator { get { return this.framework.ProviderManager.DataSimulator; } }

        public IExecutionSimulator ExecutionSimulator  { get { return this.framework.ProviderManager.ExecutionSimulator; } }

        public BarFactory BarFactory { get { return this.EventManager.BarFactory; } }

        public EventManager EventManager { get { return this.framework.EventManager; } }

        public StrategyManager StrategyManager { get { return this.framework.StrategyManager; } }

        public StatisticsManager StatisticsManager { get { return this.framework.StatisticsManager; } }

        public GroupManager GroupManager { get { return this.framework.GroupManager; } }

        public DataFileManager DataFileManager { get { return this.framework.DataFileManager; } }

        public Scenario(Framework framework)
        {
            this.framework = framework;
        }

        public void StartStrategy()
        {
            this.StartStrategy(this.strategy, this.framework.StrategyManager.Mode);
        }

        public void StartStrategy(Strategy strategy)
        {
            this.StartStrategy(strategy, this.framework.StrategyManager.Mode);
        }

        public void StartStrategy(StrategyMode mode)
        {
            this.StartStrategy(this.strategy, mode);
        }

        private void StartStrategy(Strategy strategy, StrategyMode mode)
        {
            Console.WriteLine("{0} Scenario::StartStrategy {1}", DateTime.Now, mode);
            this.framework.StrategyManager.StartStrategy(strategy, mode);

            // Wait for it
            while (strategy.Status != StrategyStatus.Stopped)
                Thread.Sleep(10);

            Console.WriteLine("{0} Scenario::StartStrategy Done", DateTime.Now);
        }

        public void StartBacktest()
        {
            this.StartStrategy(StrategyMode.Backtest);
        }

        public void StartPaper()
        {
            this.StartStrategy(StrategyMode.Paper);
        }

        public void StartLive()
        {
            this.StartStrategy(StrategyMode.Live);
        }

        public virtual void Run()
        {
        }
    }
}

