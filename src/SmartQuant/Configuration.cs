﻿// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SmartQuant
{
    [XmlRoot("Configuration")]
    public class Configuration
    {
        [XmlElement("IsInstrumentFileLocal")]
        public bool IsInstrumentFileLocal;

        [XmlElement("InstrumentFileHost")]
        public string InstrumentFileHost;

        [XmlElement("InstrumentFileName")]
        public string InstrumentFileName;

        [XmlElement("IsDataFileLocal")]
        public bool IsDataFileLocal;

        [XmlElement("DataFileHost")]
        public string DataFileHost;

        [XmlElement("DataFileName")]
        public string DataFileName;

        [XmlElement("DefaultCurrency")]
        public string DefaultCurrency;

        [XmlElement("DefaultExchange")]
        public string DefaultExchange;

        [XmlElement("DefaultDataProvider")]
        public string DefaultDataProvider;

        [XmlElement("DefaultExecutionProvider")]
        public string DefaultExecutionProvider;

        [XmlElement("ProviderManagerFileName")]
        public string ProviderManagerFileName;

        [XmlArrayItem("Streamer")]
        [XmlArray("Streamers")]
        public List<StreamerPlugin> Streamers;

        [XmlArrayItem("Provider")]
        [XmlArray("Providers")]
        public List<ProviderPlugin> Providers;

        public ServerConfiguration OrderServer;

        public void AddDefaultStreamers()
        {
            var types = new string[]
            {
                "SmartQuant.DataObjectStreamer",
                "SmartQuant.InstrumentStreamer",
                "SmartQuant.AltIdStreamer",
                "SmartQuant.LegStreamer", 
                "SmartQuant.BidStreamer",
                "SmartQuant.AskStreamer", 
                "SmartQuant.QuoteStreamer", 
                "SmartQuant.TradeStreamer",
                "SmartQuant.BarStreamer", 
                "SmartQuant.Level2SnapshotStreamer",
                "SmartQuant.NewsStreamer",
                "SmartQuant.FundamentalStreamer",
                "SmartQuant.DataSeriesStreamer",
                "SmartQuant.ExecutionCommandStreamer",
                "SmartQuant.ExecutionReportStreamer",
                "SmartQuant.PositionStreamer",
                "SmartQuant.PortfolioStreamer",
                "SmartQuant.ObjectTableStreamer",
                "SmartQuant.DoubleStreamer",
                "SmartQuant.Int16Streamer",
                "SmartQuant.Int32Streamer",
                "SmartQuant.Int64Streamer",
                "SmartQuant.StringStreamer",
                "SmartQuant.TimeSeriesItemStreamer"
            };

            foreach (var name in types)
            {
                Type t = Type.GetType(name);
                if (t != null)
                    Streamers.Add(new StreamerPlugin(t.AssemblyQualifiedName));
            }
        }

        public void AddDefaultProviders()
        {
            var types = new Dictionary<string, bool>();
            types.Add("SmartQuant.QR.QuantRouter", true);
            types.Add("SmartQuant.QB.QuantBase", true);
            types.Add("SmartQuant.IB.IBTWS", true);
            types.Add("SmartQuant.TT.TTFIX", true);
            types.Add("SmartQuant.OEC.OpenECry", true);

            foreach (var pair in types)
            {
                Type t = Type.GetType(pair.Key);
                if (t != null)
                    Providers.Add(new ProviderPlugin(t.AssemblyQualifiedName, pair.Value));
            }
        }

        public static Configuration DefaultConfiguaration()
        {
            Configuration configuration = new Configuration();

            configuration.IsInstrumentFileLocal = true;
            configuration.InstrumentFileHost = "127.0.0.1";
            configuration.InstrumentFileName = Path.Combine(Installation.DataDir.FullName, "instruments.quant");
            configuration.IsDataFileLocal = true;
            configuration.DataFileHost = "127.0.0.1";
            configuration.DataFileName = Path.Combine(Installation.DataDir.FullName, "data.quant");

            configuration.OrderServer = new ServerConfiguration();
            configuration.OrderServer.TypeName = "SmartQuant.FileOrderServer, SmartQuant";
            configuration.OrderServer.ConnectionString = string.Format("PATH={0}", Path.Combine(Installation.DataDir.FullName, "orders.quant"));

            configuration.DefaultCurrency = "USD";
            configuration.DefaultExchange = "SMART";
            configuration.DefaultDataProvider = "QuantRouter";
            configuration.DefaultExecutionProvider = "QuantRouter";
            configuration.ProviderManagerFileName = Path.Combine(Installation.ConfigDir.FullName, "providermanager.xml");

            configuration.Streamers = new List<StreamerPlugin>();
            configuration.AddDefaultStreamers();
            configuration.Providers = new List<ProviderPlugin>();
            configuration.AddDefaultProviders();

            return configuration;
        }
    }
}
