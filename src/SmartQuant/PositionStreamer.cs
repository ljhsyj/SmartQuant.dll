// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class PositionStreamer : ObjectStreamer
    {
        public PositionStreamer()
        {
            this.typeId = DataObjectType.Position;
            this.type = typeof(Position);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            return new Position();
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            writer.Write(version);
        }
    }
}
