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
            var item = new TimeSeriesItem();
            var version = reader.ReadByte();
            item.DateTime = DateTime.FromBinary(reader.ReadInt64());
            item.Value = reader.ReadDouble();
            return item;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            writer.Write(version);
            var item = obj as TimeSeriesItem;
            writer.Write(item.DateTime.ToBinary());
            writer.Write(item.Value);
        }
    }
}
