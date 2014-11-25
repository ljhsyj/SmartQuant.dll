// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

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
            Providers = new ProviderList();
            DataSimulator = dataSimulator != null ? dataSimulator : new DataSimulator(framework);
            AddProvider(DataSimulator);
            ExecutionSimulator = executionSimulator != null ? executionSimulator : new ExecutionSimulator(framework);
            AddProvider(ExecutionSimulator);
        }

        public void SetDataSimulator(string name)
        {
            DataSimulator = GetProvider(name) as IDataSimulator;
        }

        public void SetExecutionSimulator(string name)
        {
            ExecutionSimulator = GetProvider(name) as IExecutionSimulator;
        }

        public void SetDataSimulator(int id)
        {
            DataSimulator = GetProvider(id) as IDataSimulator;
        }

        public void SetExecutionSimulator(int id)
        {
            ExecutionSimulator = GetProvider(id) as IExecutionSimulator;
        }

        public void LoadSettings(IProvider provider)
        {
            var path = this.framework.Configuration.ProviderManagerFileName;
            if (!File.Exists(path))
                return;

            using (var fs = new FileStream(path, FileMode.Open))
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
            var pSettings = new List<XmlProvider>();
            foreach (var p in Providers)
            {
                XmlProvider xml;
                xml.ProviderId = p.Id;
                xml.InstanceId = p.Id;
                xml.Properties = ((Provider)p).GetProperties().ToXmlProviderProperties();
                pSettings.Add(xml);
            }
            var settings = new XmlProviderManagerSettings { Providers = pSettings };
            using (FileStream fs = new FileStream(this.framework.Configuration.ProviderManagerFileName, FileMode.Create))
                new XmlSerializer(typeof(XmlProviderManagerSettings)).Serialize(fs, settings);

        }

        public void AddProvider(IProvider provider)
        {
            if (provider.Id > 100)
            {
                Console.WriteLine("ProviderManager::AddProvider Error. Provider Id must be smaller than 100. You are trying to add provider with Id = {0}", provider.Id);
                return;
            }
            Providers.Add(provider);
            LoadSettings(provider);
            this.framework.EventServer.OnProviderAdded(provider);
        }

        public void RemoveProvider(Provider provider)
        {
            Providers.Remove(provider);
            this.framework.EventServer.OnProviderRemoved(provider);
        }

        public IProvider GetProvider(string name)
        {
            return Providers.GetByName(name);
        }

        public IProvider GetProvider(int id)
        {
            return Providers.GetById(id);
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
            foreach (var provider in Providers)
                provider.Disconnect();
        }

        public void Clear()
        {
            if (DataSimulator != null)
                DataSimulator.Clear();
            if (ExecutionSimulator != null)
                ExecutionSimulator.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            DisconnectAll();
            foreach (Provider provider in Providers)
                provider.Dispose();
        }
    }
}
