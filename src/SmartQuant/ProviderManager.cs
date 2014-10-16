// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class ProviderManager
    {
        private Framework framework;

        public ProviderList Providers { get; private set; }

        public IDataSimulator DataSimulator { get; set; }

        public IExecutionSimulator ExecutionSimulator { get; set; }

        public ProviderManager(Framework framework, IDataSimulator dataSimulator = null, IExecutionSimulator executionSimulator = null)
        {
            this.framework = framework;
            this.Providers = new ProviderList();
            this.DataSimulator = dataSimulator != null ? dataSimulator : new DataSimulator(framework);
            this.AddProvider(this.DataSimulator);
            this.ExecutionSimulator = executionSimulator != null ? executionSimulator : new ExecutionSimulator(framework);
            this.AddProvider(this.ExecutionSimulator);
        }

        public void SetDataSimulator(string name)
        {
            this.DataSimulator = this.GetProvider(name) as IDataSimulator;
        }

        public void SetExecutionSimulator(string name)
        {
            this.ExecutionSimulator = this.GetProvider(name) as IExecutionSimulator;
        }

        public void SetDataSimulator(int id)
        {
            this.DataSimulator = this.GetProvider(id) as IDataSimulator;
        }

        public void SetExecutionSimulator(int id)
        {
            this.ExecutionSimulator = this.GetProvider(id) as IExecutionSimulator;
        }

        public void LoadSettings(IProvider provider)
        {
            throw new NotImplementedException();
        }

        public void SaveSettings(IProvider provider)
        {
            throw new NotImplementedException();
        }

        public void AddProvider(IProvider provider)
        {
            if (provider.Id > 100)
            {
                Console.WriteLine("ProviderManager::AddProvider Error. Provider Id must be smaller than 100. You are trying to add provider with Id = {0}", provider.Id);
                return;
            }
            Providers.Add(provider);
            this.LoadSettings(provider);
            this.framework.EventServer.OnProviderAdded(provider);
        }

        public IProvider GetProvider(string name)
        {
            return this.Providers.GetByName(name);
        }

        public IProvider GetProvider(int id)
        {
            return this.Providers.GetById(id);
        }

        public IDataProvider GetDataProvider(string name)
        {
            return Providers.GetByName(name) as IDataProvider;
        }

        public IDataProvider GetDataProvider(int id)
        {
            return Providers.GetById(id) as IDataProvider;
        }

        public IExecutionProvider GetExecutionProvider(string name)
        {
            return Providers.GetByName(name) as IExecutionProvider;
        }

        public IExecutionProvider GetExecutionProvider(int id)
        {
            return Providers.GetById(id) as IExecutionProvider;
        }

        public IHistoricalDataProvider GetHistoricalDataProvider(string name)
        {
            return Providers.GetByName(name) as IHistoricalDataProvider;
        }

        public IHistoricalDataProvider GetHistoricalDataProvider(int id)
        {
            return Providers.GetById(id) as IHistoricalDataProvider;
        }

        public IInstrumentProvider GetInstrumentProvider(string name)
        {
            return Providers.GetByName(name) as IInstrumentProvider;
        }

        public IInstrumentProvider GetInstrumentProvider(int id)
        {
            return Providers.GetById(id) as IInstrumentProvider;
        }

        public INewsProvider GetNewsProvider(string name)
        {
            return Providers.GetByName(name) as INewsProvider;
        }

        public INewsProvider GetNewsProvider(int id)
        {
            return Providers.GetById(id) as INewsProvider;
        }

        public void DisconnectAll()
        {
            foreach (Provider provider in this.Providers)
                provider.Disconnect();
        }

        public void Clear()
        {
            Providers.Clear();
        }
    }
}
