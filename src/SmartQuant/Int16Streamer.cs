// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class Int16Streamer : ObjectStreamer
    {
        public Int16Streamer()
        {
            this.typeId = DataObjectType.Int16;
            this.type = typeof(short);
        }

        public override object Read(BinaryReader reader)
        {
            return reader.ReadInt16();
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((short)obj);
        }
    }
}