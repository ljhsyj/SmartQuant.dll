// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class Level2Streamer : ObjectStreamer
    {
        public Level2Streamer()
        {
            this.typeId = DataObjectType.Level2;
            this.type = typeof(Level2);
        }

        public override object Read(BinaryReader reader)
        {
            var l2 = new Level2();
            var version = reader.ReadByte();
            l2.DateTime = DateTime.FromBinary(reader.ReadInt64());
            l2.ProviderId = reader.ReadByte();
            l2.InstrumentId = reader.ReadInt32();
            l2.Price = reader.ReadDouble();
            l2.Size = reader.ReadInt32();
            l2.Side = (Level2Side)reader.ReadByte();
            l2.Action = (Level2UpdateAction)reader.ReadByte();
            l2.Position = reader.ReadInt32();
            return l2;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            var l2 = obj as Level2;
            writer.Write(version);
            writer.Write(l2.DateTime.ToBinary());
            writer.Write(l2.ProviderId);
            writer.Write(l2.InstrumentId);
            writer.Write(l2.Price);
            writer.Write(l2.Size);
            writer.Write((byte)l2.Side);
            writer.Write((byte)l2.Action);
            writer.Write(l2.Position);
        }
    }
}
