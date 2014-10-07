// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class SellSideInstrumentStrategy : SellSideStrategy
    {
        protected Instrument Instrument;

        public IdArray<SellSideInstrumentStrategy> StrategyByInstrument { get; private set; }

        public SellSideInstrumentStrategy(Framework framework, string name)
            : base(framework, name)
        {
            StrategyByInstrument = new IdArray<SellSideInstrumentStrategy>();
        }

        public override void Subscribe(InstrumentList instruments)
        {
            foreach (Instrument instrument in instruments)
                this.Subscribe(instrument);
        }

        public override void Subscribe(Instrument instrument)
        { 
            throw new NotImplementedException();
        }

        public override void Unsubscribe(InstrumentList instruments)
        {
            foreach (Instrument instrument in instruments)
                this.Unsubscribe(instrument);
        }

        public override void Unsubscribe(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public override void Send(ExecutionCommand command)
        {
            throw new NotImplementedException();
        }
    }
}

