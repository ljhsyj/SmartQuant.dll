// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class DoubleStreamer : ObjectStreamer
    {
        public DoubleStreamer()
        {
            this.typeId = DataObjectType.Double;
            this.type = typeof(double);
        }

        public override object Read(BinaryReader reader)
        {
            return reader.ReadDouble();
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((double)obj);
        }
    }
}