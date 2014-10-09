// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class BooleanStreamer : ObjectStreamer
    {
        public BooleanStreamer()
        {
            this.typeId = DataObjectType.Boolean;
            this.type = typeof(bool);
        }

        public override object Read(BinaryReader reader)
        {
            return reader.ReadBoolean() ? 1 : 0;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((bool)obj);
        }
    }
}