// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class BidStreamer : ObjectStreamer
    {
        public BidStreamer()
        {
            this.typeId = DataObjectType.Bid;
            this.type = typeof(Bid);
        }

        public override object Read(BinaryReader reader)
        {
            byte version = reader.ReadByte();
            if (version == 0)
                return new Bid(new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
            else
                return new Bid(new DateTime(reader.ReadInt64()), new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            var bid = obj as Bid;
            byte version = 0;
            if (bid.ExchangeDateTime.Ticks != 0)
                version = 1;
            writer.Write(version);
            writer.Write(bid.DateTime.Ticks);
            if ((int)version == 1)
                writer.Write(bid.ExchangeDateTime.Ticks);
            writer.Write(bid.ProviderId);
            writer.Write(bid.InstrumentId);
            writer.Write(bid.Price);
            writer.Write(bid.Size); 
        }
    }
}
