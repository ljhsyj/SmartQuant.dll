// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    class DataSimulator : Provider, IDataSimulator
    {
        public DateTime DateTime1 { get; set; }

        public DateTime DateTime2 { get; set; }

        public bool SubscribeBid { get; set; }

        public bool SubscribeAsk { get; set; }

        public bool SubscribeTrade { get; set; }

        public bool SubscribeBar { get; set; }

        public bool SubscribeLevelII { get; set; }

        public bool SubscribeNews { get; set; }

        public bool SubscribeFundamental { get; set; }

        public bool SubscribeAll
        {
            set
            {
                SubscribeBid = value;
                SubscribeAsk = value;
                SubscribeTrade = value;
                SubscribeBar = value;
                SubscribeLevelII = value;
                SubscribeNews = value;
                SubscribeFundamental = value;
            }
        }

        public BarFilter BarFilter { get; private set; }

        public DataProcessor Processor { get; set; }

        public List<IDataSeries> Series { get; set; }

        public DataSimulator(Framework framework)
            : base(framework)
        {
            this.id = ProviderId.DataSimulator;
            this.name = "DataSimulator";
            this.description = "Default data simulator";
            this.url = "www.smartquant.com";
            DateTime1 = DateTime.MinValue;
            DateTime2 = DateTime.MaxValue;
            SubscribeAll = true;
            Processor = new DataProcessor();
            BarFilter = new BarFilter();
            Series = new List<IDataSeries>();
        }

        public override void Connect()
        {
            if (IsDisconnected)
                Status = ProviderStatus.Connected;
        }

        public override void Disconnect()
        {
            throw new NotImplementedException();
        }

        protected override void OnConnected()
        {
            throw new NotImplementedException();
        }

        protected override void OnDisconnected()
        {
        }

        public void Clear()
        {
            DateTime1 = DateTime.MinValue;
            DateTime2 = DateTime.MaxValue;
            Series.Clear();
            BarFilter.Clear();
        }
    }
}
