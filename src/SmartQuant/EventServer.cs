// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class EventServer
    {
        private Framework framework;
        private EventQueue queue;
        private EventBus bus;

        public EventServer(Framework framework, EventBus bus)
        {
            this.framework = framework;
            this.bus = bus;
            this.queue = new EventQueue(EventQueueId.All, EventQueueType.Master, EventQueuePriority.Normal, 8192);
        }

        public void OnEvent(Event e)
        {
            this.framework.EventManager.OnEvent(e);
        }

        public void OnLog(Event e)
        {
            OnEvent(e);
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
                OnProviderConnected(provider);
            if (provider.Status == ProviderStatus.Disconnected)
                OnProviderDisconnected(provider);
            OnEvent(new OnProviderStatusChanged(provider));
        }

        public void OnProviderError(ProviderError error)
        {
            throw new NotImplementedException();
        }

        public void OnProviderConnected(Provider provider)
        {
            OnEvent(new OnProviderConnected(provider));
        }

        public void OnProviderDisconnected(Provider provider)
        {
            OnEvent(new OnProviderDisconnected(provider));
        }

        public void OnPortfolioAdded(Portfolio portfolio)
        {
            OnEvent(new OnPortfolioAdded(portfolio));
        }

        public void OnPortfolioDeleted(Portfolio portfolio)
        {
            OnEvent(new OnPortfolioDeleted(portfolio));
        }

        public void OnPositionOpened(Portfolio portfolio, Position position, bool queued = true)
        {
            var e = new OnPositionOpened(portfolio, position);
            if (queued)
                this.queue.Enqueue(e);
            else
                OnEvent(e);
        }

        internal void OnPositionClosed(Portfolio portfolio, Position position, bool queued = true)
        {
            if (queued)
                this.queue.Enqueue(new OnPositionClosed(portfolio, position));
            else
                OnEvent(new OnPositionClosed(portfolio, position));
        }

        internal void OnPositionChanged(Portfolio portfolio, Position position, bool queued = true)
        {
            if (queued)
                this.queue.Enqueue(new OnPositionChanged(portfolio, position));
            else
                OnEvent(new OnPositionChanged(portfolio, position));
        }

        public void EmitQueued()
        {
            while (!this.queue.IsEmpty())
                OnEvent(this.queue.Read());
        }

        internal void OnExecutionReport(ExecutionReport report)
        {
            OnEvent(new OnExecutionReport(report));
        }

        // FIXME:the second param name is wrong!
        internal void OnPortfolioParentChanged(Portfolio portfolio, bool queued = true)
        {
            if (queued)
                OnEvent(new OnPortfolioParentChanged(portfolio));
        }

        internal void OnTransaction(Portfolio portfolio, Transaction transaction, bool queued = true)
        {
            if (queued)
                this.queue.Enqueue(new OnTransaction(portfolio, transaction));
            else
                OnEvent(new OnTransaction(portfolio, transaction));
        }

        internal void OnFill(Portfolio portfolio, Fill fill, bool queued = true)
        {
            if (queued)
                this.queue.Enqueue(new OnFill(portfolio, fill));
            else
                OnEvent(new OnFill(portfolio, fill));
        }

        internal void OnFrameworkCleared(Framework framework)
        {
            OnEvent(new OnFrameworkCleared(framework));
        }
    }
}
