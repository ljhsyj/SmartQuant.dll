// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class EventServer
    {
        private Framework framework;
        private EventBus bus;
        // Not yet used

        public EventServer(Framework framework, EventBus bus)
        {
            this.framework = framework;
            this.bus = bus;
        }

        public void OnEvent(Event e)
        {
            this.framework.EventManager.OnEvent(e);
        }

        public void OnFrameworkCleared(Framework framework)
        {
            throw new NotImplementedException();
        }

        public void OnLog(Event e)
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
            OnEvent(new OnProviderAdded(provider));
        }

        public void OnProviderRemoved(Provider provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderStatusChanged(Provider provider)
        {
            if (provider.Status == ProviderStatus.Connected)
                this.OnProviderConnected(provider);
            if (provider.Status == ProviderStatus.Disconnected)
                this.OnProviderDisconnected(provider);
            this.OnEvent(new OnProviderStatusChanged(provider));
        }

        public void OnProviderError(ProviderError error)
        {
            throw new NotImplementedException();
        }

        public void OnProviderConnected(Provider provider)
        {
            this.OnEvent(new OnProviderConnected(provider));
        }

        public void OnProviderDisconnected(Provider provider)
        {
            this.OnEvent(new OnProviderDisconnected(provider));
        }

        public void OnPortfolioAdded(Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public void OnPortfolioDeleted(Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        internal void OnPortfolioParentChanged(Portfolio portfolio, bool queued = true)
        {
            if (queued)
                OnEvent(new OnPortfolioParentChanged(portfolio));
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
