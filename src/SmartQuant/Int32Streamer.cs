// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class Int32Streamer : ObjectStreamer
    {
        public Int32Streamer()
        {
            this.typeId = DataObjectType.Int32;
            this.type = typeof(int);
        }

        public override object Read(BinaryReader reader)
        {
            return reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((int)obj);
        }
    }
}

