// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;
using System;
using System.IO;

namespace SmartQuant
{
    public class SubscriptionManager
    {
        private Framework framework;
        private Dictionary<byte, Dictionary<Instrument, int>> subscriptions;
        private SubscriptionList subs;

        public SubscriptionManager(Framework framework)
        {
            this.framework = framework;
            subscriptions = new Dictionary<byte, Dictionary<Instrument, int>>();
            subs = new SubscriptionList();
        }

        public void Clear()
        {
            subscriptions.Clear();
            subs.Clear();
        }

        public void Subscribe(int providerId, Instrument instrument)
        {
            Subscribe(this.framework.ProviderManager.GetProvider(providerId) as IDataProvider, instrument);
        }

        public void Subscribe(int providerId, int instrumentId)
        {
            Subscribe(providerId, this.framework.InstrumentManager.GetById(instrumentId));
        }

        public void Subscribe(string provider, Instrument instrument)
        {
            Subscribe(this.framework.ProviderManager.GetProvider(provider) as IDataProvider, instrument);
        }

        public void Subscribe(string provider, string symbol)
        {
            Subscribe(this.framework.ProviderManager.GetProvider(provider) as IDataProvider, this.framework.InstrumentManager.Get(symbol));
        }

        public void Subscribe(IDataProvider provider, Instrument instrument)
        {
            if (provider.Status != ProviderStatus.Connected)
                provider.Connect();
            Dictionary<Instrument, int> dictionary = null;
            if (!this.subscriptions.TryGetValue(provider.Id, out dictionary))
            {
                dictionary = new Dictionary<Instrument, int>();
                this.subscriptions[provider.Id] = dictionary;
            }

            int num1 = 0;
            bool flag = false;
            int num2;
            if (!dictionary.TryGetValue(instrument, out num1))
            {
                flag = true;
                num2 = 1;
            }
            else
            {
                if (num1 == 0)
                    flag = true;
                num2 = num1 + 1;
            }
            dictionary[instrument] = num2;
            if (!flag)
                return;
            provider.Subscribe(instrument);
        }

        public void Unsubscribe(int providerId, Instrument instrument)
        {
            Unsubscribe(this.framework.ProviderManager.GetProvider(providerId) as IDataProvider, instrument);
        }

        public void Unsubscribe(int providerId, int instrumentId)
        {
            Unsubscribe(providerId, this.framework.InstrumentManager.GetById(instrumentId));
        }

        public void Unsubscribe(IDataProvider provider, Instrument instrument)
        {
            var dictionary = this.subscriptions[provider.Id];
            dictionary[instrument] -= 1;
            if (dictionary[instrument] == 0)
                provider.Unsubscribe(instrument);
        }

        public void Unsubscribe(string provider, Instrument instrument)
        {
            Unsubscribe(this.framework.ProviderManager.GetProvider(provider) as IDataProvider, instrument);
        }

        public void Unsubscribe(string provider, string symbol)
        {
            Unsubscribe(this.framework.ProviderManager.GetProvider(provider) as IDataProvider, this.framework.InstrumentManager.Get(symbol));
        }

        public void Subscribe(IDataProvider provider, InstrumentList instruments)
        {
            if (provider.Status != ProviderStatus.Connected)
                provider.Connect();
            InstrumentList instrument = new InstrumentList();
            for (int i = 0; i < instruments.Count; ++i)
            {
                var byIndex = instruments.GetByIndex(i);
                if (!this.subscriptions.ContainsKey(provider.Id))
                    this.subscriptions[provider.Id] = new Dictionary<Instrument, int>();
                if (!this.subscriptions[provider.Id].ContainsKey(byIndex) || this.subscriptions[provider.Id][byIndex] == 0)
                {
                    this.subscriptions[provider.Id][byIndex] = 0;
                    instrument.Add(byIndex);
                }
                Dictionary<Instrument, int> dictionary = this.subscriptions[provider.Id];
                Instrument index2;
                dictionary[byIndex] += 1;
            }
            if (instrument.Count <= 0)
                return;
            provider.Subscribe(instrument);
        }

        public void Unsubscribe(IDataProvider provider, InstrumentList instruments)
        {
            var instrument = new InstrumentList();
            for (int i = 0; i < instruments.Count; ++i)
            {
                Instrument byIndex = instruments.GetByIndex(i);
                Dictionary<Instrument, int> dictionary = this.subscriptions[provider.Id];
                dictionary[byIndex] -= 1;
                if (this.subscriptions[provider.Id][byIndex] == 0)
                    instrument.Add(byIndex);
            }
            provider.Unsubscribe(instrument);
        }

        internal void OnProviderConnected(IDataProvider provider)
        {
            if (!this.subscriptions.ContainsKey(provider.Id))
                return;
            foreach (var instrument in this.subscriptions[provider.Id].Keys)
            {
                if (this.subscriptions[provider.Id][instrument] != 0)
                {
                    Console.WriteLine("SubscriptionManager::OnProviderConnected {0}  resubscribing {1}", provider.Name, instrument.Symbol);
                    provider.Subscribe(instrument);
                }
            }
        }

        internal void OnProviderDisconnected(IDataProvider provider)
        {
        }

        internal void Load(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                int providerId = reader.ReadInt32();
                int instrumentCount = reader.ReadInt32();
                for (int j = 0; j < instrumentCount; ++j)
                {
                    int instrumentId = reader.ReadInt32();
                    int times = reader.ReadInt32();
                    for (int k = 0; k < times; ++k)
                        Subscribe(providerId, instrumentId);
                }
            }
        }

        internal void Save(BinaryWriter writer)
        {
            writer.Write(this.subscriptions.Count);
            foreach (var subscription in this.subscriptions)
            {
                writer.Write(subscription.Key);
                var val = subscription.Value;
                writer.Write(val.Count);
                foreach (var kv in val)
                {
                    writer.Write(kv.Key.Id);
                    writer.Write(kv.Value);
                }
            }
        }
    }
}
