// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class TickStreamer : ObjectStreamer
    {
        public TickStreamer()
        {
            this.typeId = DataObjectType.Tick;
            this.type = typeof(Tick);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            if (version == 0)
                return new Tick(new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
            else
                return new Tick(new DateTime(reader.ReadInt64()), new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            var tick = (Tick)obj;
            byte version = tick.ExchangeDateTime.Ticks == 0 ? (byte)0 : (byte)1;
            writer.Write(version);
            writer.Write(tick.DateTime.Ticks);
            if (version == 1)
                writer.Write(tick.ExchangeDateTime.Ticks);
            writer.Write(tick.ProviderId);
            writer.Write(tick.InstrumentId);
            writer.Write(tick.Price);
            writer.Write(tick.Size);
        }
    }
}
