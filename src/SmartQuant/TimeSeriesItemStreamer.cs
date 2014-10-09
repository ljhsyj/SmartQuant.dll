// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class TimeSeriesItemStreamer : ObjectStreamer
    {
        public TimeSeriesItemStreamer()
        {
            this.typeId = DataObjectType.TimeSeriesItem;
            this.type = typeof(TimeSeriesItem);
        }

        public override object Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            throw new NotImplementedException();
        }
    }
}
