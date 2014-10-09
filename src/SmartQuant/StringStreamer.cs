// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class StringStreamer : ObjectStreamer
    {
        public StringStreamer()
        {
            this.typeId = DataObjectType.String;
            this.type = typeof(string);
        }

        public override object Read(BinaryReader reader)
        {
            return reader.ReadString();
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((string)obj);
        }
    }
}

