// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class AltId
    {
        public byte ProviderId { get; set; }

        public string Symbol { get; set; }

        public string Exchange { get; set; }

        public AltId()
        {
        }

        public AltId(byte providerId, string symbol, string exchange)
        {
            ProviderId = providerId;
            Symbol = symbol;
            Exchange = exchange;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}@{2}", ProviderId, Symbol, Exchange);
        }
    }
}
