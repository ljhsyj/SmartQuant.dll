// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class QuoteStreamer : ObjectStreamer
    {
        public QuoteStreamer()
        {
            this.typeId = DataObjectType.Quote;
            this.type = typeof(Quote);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            return new Quote(new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            var quote = obj as Quote;
            writer.Write(version);
            writer.Write(quote.DateTime.Ticks);
            writer.Write(quote.Bid.ProviderId);
            writer.Write(quote.Bid.InstrumentId);
            writer.Write(quote.Bid.Price);
            writer.Write(quote.Bid.Size);
            writer.Write(quote.Ask.Price);
            writer.Write(quote.Ask.Size);
        }
    }
}
