// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class EventServer
    {
        private Framework framework;
        private EventBus bus;

        public EventServer(Framework framework, EventBus bus)
        {
            this.framework = framework;
            this.bus = bus;
        }

        public void OnFrameworkCleared(Framework framework)
        {
            throw new NotImplementedException();
        }

        public void OnLog(Event e)
        {
            throw new NotImplementedException();
        }

        public void OnEvent(Event e)
        {
            throw new NotImplementedException();
        }

        public void OnData(DataObject data)
        {
            throw new NotImplementedException();
        }

        public void OnInstrumentAdded(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public void OnInstrumentDeleted(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public void OnInstrumentDefinition(InstrumentDefinition definition)
        {
            throw new NotImplementedException();
        }

        public void OnInstrumentDefintionEnd(InstrumentDefinitionEnd end)
        {
            throw new NotImplementedException();
        }

        public void OnHistoricalData(HistoricalData data)
        {
            throw new NotImplementedException();
        }

        public void OnHistoricalDataEnd(HistoricalDataEnd end)
        {
            throw new NotImplementedException();
        }

        public void OnProviderAdded(IProvider provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderRemoved(Provider provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderStatusChanged(Provider provider)
        {
            throw new NotImplementedException();

        }

        public void OnProviderError(ProviderError error)
        {
            throw new NotImplementedException();
        }

        public void OnProviderConnected(Provider provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderDisconnected(Provider provider)
        {
            throw new NotImplementedException();
        }

        public void OnPortfolioAdded(Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public void OnPortfolioDeleted(Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public void OnPositionOpened(Portfolio portfolio, Position position, bool queued = true)
        {
            throw new NotImplementedException();
        }

        public void EmitQueued()
        {
            throw new NotImplementedException();

        }
    }
}
