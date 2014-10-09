// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.IO;
using System.Drawing;

namespace SmartQuant
{
    public class ColorStreamer : ObjectStreamer
    {
        public ColorStreamer()
        {
            this.typeId = DataObjectType.Color;
            this.type = typeof(Color);
        }

        public override object Read(BinaryReader reader)
        {
            return Color.FromArgb(reader.ReadInt32());
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write(((Color)obj).ToArgb());
        }
    }
}