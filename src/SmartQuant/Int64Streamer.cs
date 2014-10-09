// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class Int64Streamer : ObjectStreamer
    {
        public Int64Streamer()
        {
            this.typeId = DataObjectType.Int64;
            this.type = typeof(long);
        }

        public override object Read(BinaryReader reader)
        {
            return reader.ReadInt64();
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((long)obj);
        }
    }
}