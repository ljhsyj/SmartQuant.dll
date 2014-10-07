// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;
using System.Collections.Generic;

namespace SmartQuant
{
    public class OrderBook
    {
        public IList<Tick> Bids { get; private set; }

        public IList<Tick> Asks { get; private set; }

        public OrderBook()
        {
            Bids = new List<Tick>();
            Asks = new List<Tick>();
        }

        public Quote GetQuote(int level)
        {
            throw new NotImplementedException();
        }

        public int GetBidVolume()
        {
            throw new NotImplementedException();
        }

        public int GetAskVolume()
        {
            throw new NotImplementedException();
        }

        public double GetAvgBidPrice()
        {
            throw new NotImplementedException();
        }

        public double GetAvgAskPrice()
        {
            throw new NotImplementedException();
        }
    }
}
