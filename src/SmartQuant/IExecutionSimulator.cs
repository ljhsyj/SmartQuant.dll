// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public interface IExecutionSimulator : IExecutionProvider
    {
        bool FillOnQuote { get; set; }

        bool FillOnTrade { get; set; }

        bool FillOnBar { get; set; }

        bool FillOnBarOpen { get; set; }

        bool FillOnLevel2 { get; set; }

        bool FillMarketOnNext { get; set; }

        bool FillLimitOnNext { get; set; }

        bool FillStopOnNext { get; set; }

        bool FillStopLimitOnNext { get; set; }

        bool FillAtLimitPrice { get; set; }

        bool PartialFills { get; set; }

        bool Queued { get; set; }

        BarFilter BarFilter { get; }

        ICommissionProvider CommissionProvider { get; set; }

        ISlippageProvider SlippageProvider { get; set; }

        void OnBid(Bid bid);

        void OnAsk(Ask ask);

        void OnLevel2(Level2Snapshot snapshot);

        void OnLevel2(Level2Update update);

        void OnTrade(Trade trade);

        void OnBar(Bar bar);

        void Clear();
    }
}
