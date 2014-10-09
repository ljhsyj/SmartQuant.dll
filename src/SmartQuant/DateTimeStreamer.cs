// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class DateTimeStreamer : ObjectStreamer
    {
        public DateTimeStreamer()
        {
            this.typeId = DataObjectType.DateTime;
            this.type = typeof(DateTime);
        }

        public override object Read(BinaryReader reader)
        {
            return new DateTime(reader.ReadInt64());
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write(((DateTime)obj).Ticks);
        }
    }
}