// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class Level2Snapshot : DataObject
    {
        internal byte ProviderId { get; set; }

        internal int InstrumentId { get; set; }

        public override byte TypeId
        {
            get
            {
                return DataObjectType.Level2Snapshot;
            }
        }

        public Bid[] Bids { get; private set; }

        public Ask[] Asks { get; private set; }

        public Level2Snapshot(DateTime dateTime, byte providerId, int instrumentId, Bid[] bids, Ask[] asks)
            : base(dateTime)
        {
            ProviderId = providerId;
            InstrumentId = instrumentId;
            Bids = bids;
            Asks = asks;
        }

        public Level2Snapshot()
        {
        }
    }
}
