// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class CharStreamer : ObjectStreamer
    {
        public CharStreamer()
        {
            this.typeId = DataObjectType.Char;
            this.type = typeof(char);
        }

        public override object Read(BinaryReader reader)
        {
            return reader.ReadChar();
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((char)obj);
        }
    }
}