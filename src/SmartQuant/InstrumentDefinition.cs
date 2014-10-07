// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class InstrumentDefinition
    {
        public string RequestId { get; set; }
        public byte ProviderId { get; set; }
        public int TotalNum { get; set; }
        public Instrument[] Instruments { get; set; }
    }
}
