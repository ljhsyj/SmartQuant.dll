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
            Console.WriteLine(string.Format("{0} Scenario::StartStrategy {1}", DateTime.Now, mode));
            this.framework.StrategyManager.StartStrategy(strategy, mode);

            // Wait for it
            while (this.strategy.Status != StrategyStatus.Stopped)
                Thread.Sleep(10);

            Console.WriteLine(string.Format("{0} Scenario::StartStrategy Done", DateTime.Now));
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

