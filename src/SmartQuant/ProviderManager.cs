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
        }

        public void SaveSettings(IProvider provider)
        {
        }

        public void AddProvider(IProvider provider)
        {
            throw new System.NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IDataProvider GetDataProvider(int id)
        {
            throw new System.NotImplementedException();
        }

        public IExecutionProvider GetExecutionProvider(string name)
        {
            throw new NotImplementedException();
        }

        public IExecutionProvider GetExecutionProvider(int id)
        {
            throw new NotImplementedException();
        }

        public IHistoricalDataProvider GetHistoricalDataProvider(string name)
        {
            throw new NotImplementedException();
        }

        public IHistoricalDataProvider GetHistoricalDataProvider(int id)
        {
            throw new NotImplementedException();
        }

        public IInstrumentProvider GetInstrumentProvider(string name)
        {
            throw new NotImplementedException();
        }

        public IInstrumentProvider GetInstrumentProvider(int id)
        {
            throw new NotImplementedException();
        }

        public INewsProvider GetNewsProvider(string name)
        {
            throw new NotImplementedException();
        }

        public INewsProvider GetNewsProvider(int id)
        {
            throw new NotImplementedException();
        }

        public void DisconnectAll()
        {
            foreach (Provider provider in this.Providers)
                provider.Disconnect();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}
