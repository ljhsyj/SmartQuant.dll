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

        public DataProcessor Processor { get; set; }

        public List<IDataSeries> Series { get; set; }

        public DataSimulator(Framework framework) : base(framework)
        {
            this.id = ProviderId.DataSimulator;
            this.name = "DataSimulator";
            this.description = "Default data simulator";
            this.url = "www.smartquant.com";
            this.DateTime1 = DateTime.MinValue;
            this.DateTime2 = DateTime.MaxValue;
            this.Series = new List<IDataSeries>();
        }

        public void Clear()
        {
            this.Series.Clear();
        }
	}
}
