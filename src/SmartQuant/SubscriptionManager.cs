// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
	public class SubscriptionManager
	{
        private Framework framework;

        public SubscriptionManager(Framework framework)
        {
            this.framework = framework;
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public void Subscribe(int providerId, Instrument instrument)
        {
            this.Subscribe(this.framework.ProviderManager.GetProvider(providerId) as IDataProvider, instrument);
        }

        public void Subscribe(int providerId, int instrumentId)
        {
            this.Subscribe(providerId, this.framework.InstrumentManager.GetById(instrumentId));
        }

        public void Subscribe(string provider, Instrument instrument)
        {
            this.Subscribe(this.framework.ProviderManager.GetProvider(provider) as IDataProvider, instrument);
        }

        public void Subscribe(string provider, string symbol)
        {
            this.Subscribe(this.framework.ProviderManager.GetProvider(provider) as IDataProvider, this.framework.InstrumentManager.Get(symbol));
        }

        public void Subscribe(IDataProvider provider, Instrument instrument)
        {
        }

        public void Unsubscribe(int providerId, Instrument instrument)
        {
            this.Unsubscribe(this.framework.ProviderManager.GetProvider(providerId) as IDataProvider, instrument);
        }

        public void Unsubscribe(int providerId, int instrumentId)
        {
            Instrument byId = this.framework.InstrumentManager.GetById(instrumentId);
            this.Unsubscribe(providerId, byId);
        }

        public void Unsubscribe(IDataProvider provider, Instrument instrument)
        {
        }

        public void Unsubscribe(string provider, Instrument instrument)
        {
            this.Unsubscribe(this.framework.ProviderManager.GetProvider(provider) as IDataProvider, instrument);
        }

        public void Unsubscribe(string provider, string symbol)
        {
            this.Unsubscribe(this.framework.ProviderManager.GetProvider(provider) as IDataProvider, this.framework.InstrumentManager.Get(symbol));
        }

        public void Subscribe(IDataProvider provider, InstrumentList instruments)
        {
        }

        public void Unsubscribe(IDataProvider provider, InstrumentList instruments)
        {
        }
	}
}
