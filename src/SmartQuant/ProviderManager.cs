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

//        public void LoadSettings(IProvider provider)
//        {
//        }
//
//        public void SaveSettings(IProvider provider)
//        {
//        }
//
        public void AddProvider(IProvider provider)
        {
            throw new System.NotImplementedException();
        }
//
//        public IProvider GetProvider(string name)
//        {
//            throw new NotImplementedException();
//        }
//
//        public IProvider GetProvider(int id)
//        {
//            throw new System.NotImplementedException();
//        }
//
//        public void Clear()
//        {
//            throw new System.NotImplementedException();
//        }
    }

}
