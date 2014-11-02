// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SmartQuant
{
    public class ProviderManager
    {
        private Framework framework;
        //        private XmlProviderManagerSettings settings;

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
            var path = this.framework.Configuration.ProviderManagerFileName;
            if (!File.Exists(path))
                return;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                var settings = (XmlProviderManagerSettings)new XmlSerializer(typeof(XmlProviderManagerSettings)).Deserialize(fs);
                foreach (var p in settings.Providers)
                {
                    if (p.ProviderId == provider.Id)
                    {
                        ((Provider)provider).SetProperties(new ProviderPropertyList(p.Properties));
                        break;
                    }
                }
            }
        }

        public void SaveSettings(IProvider provider)
        {
//            var path = this.framework.Configuration.ProviderManagerFileName;
//            if (!File.Exists(path))
//                return;
//
//            var props = ((Provider)provider).GetProperties();
//            XmlProvider xp = new XmlProvider();
//            xp.ProviderId = provider.Id;
//            xp.InstanceId = new Random().Next();
//            xp.Properties = (List<XmlProviderProperty>)props;
//            XmlProviderManagerSettings settings;
//            using (FileStream fs = new FileStream(path, FileMode.Open))
//            {
//                settings = (XmlProviderManagerSettings)new XmlSerializer(typeof(XmlProviderManagerSettings)).Deserialize(fs);
//                foreach (var p in settings.Providers)
//                {
//                    if (p.ProviderId == provider.Id)
//                        p = xp;
////                    {
////                        ((Provider)provider).SetProperties(new ProviderPropertyList(p.Properties));
////                        break;
////                    }
//                }
//            }
//
//
//            using (FileStream fs = new FileStream(path, FileMode.Create))
//                new XmlSerializer(typeof(XmlProviderManagerSettings)).Serialize(fs, settings);

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisconnectAll();
                foreach (Provider provider in this.Providers)
                    provider.Dispose();
            }
        }
    }
}
