// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class ComponentStrategy : InstrumentStrategy
    {
        public DataComponent DataComponent { get; set; }

        public AlphaComponent AlphaComponent { get; set; }

        public PositionComponent PositionComponent { get; set; }

        public RiskComponent RiskComponent { get; set; }

        public ExecutionComponent ExecutionComponent { get; set; }

        public ReportComponent ReportComponent { get; set; }

        public ComponentStrategy(Framework framework, string name)
            : base(framework, name)
        {
        }

        protected internal override void OnStrategyStart()
        {
            throw new NotImplementedException();
        }

        protected internal override void OnBar(Instrument instrument, Bar bar)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnTrade(Instrument instrument, Trade trade)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnBid(Instrument instrument, Bid bid)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnAsk(Instrument instrument, Ask ask)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnFill(Fill fill)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnPositionOpened(Position position)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnPositionClosed(Position position)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnPositionChanged(Position position)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnExecutionReport(ExecutionReport report)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnOrderFilled(Order order)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnStopExecuted(Stop stop)
        {
            throw new NotImplementedException();
        }

        protected internal override void OnStopCancelled(Stop stop)
        {
            throw new NotImplementedException();
        }
    }
}

