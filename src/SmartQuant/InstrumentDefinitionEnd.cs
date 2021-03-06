﻿// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class InstrumentDefinitionEnd
    {
        public string RequestId { get; set; }

        public RequestResult Result { get; set; }

        public string Text { get; set; }

        public InstrumentDefinitionEnd()
        {
        }

        internal InstrumentDefinitionEnd(string requestId, RequestResult result, string text)
        {
            RequestId = requestId;
            Result = result;
            Text = text;
        }
    }
}
