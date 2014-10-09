// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class Level2Update : DataObject
    {
        internal byte ProviderId { get; set; }

        internal int InstrumentId { get; set; }

        public Level2[] Entries { get; private set; }

        public override byte TypeId
        {
            get
            {
                return DataObjectType.Level2Update;
            }
        }

        public Level2Update(DateTime dateTime, byte providerId, int instrumentId, Level2[] entries)
            : base(dateTime)
        {
            ProviderId = providerId;
            InstrumentId = instrumentId;
            Entries = entries;
        }
    }
}
