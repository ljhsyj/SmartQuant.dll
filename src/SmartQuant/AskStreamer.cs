// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class AskStreamer : ObjectStreamer
    {
        public AskStreamer()
        {
            this.typeId = DataObjectType.Ask;
            this.type = typeof(Ask);
        }

//        public override object Read(BinaryReader reader)
//        {
//            return new Ask(reader);
//        }
//
//        public override void Write(BinaryWriter writer, object obj)
//        {
//            (obj as Ask).Write(writer);
//        }

        public override object Read(BinaryReader reader)
        {
            if ((int) reader.ReadByte() == 0)
                return (object) new Ask(new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
            else
                return (object) new Ask(new DateTime(reader.ReadInt64()), new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            Ask ask = (Ask) obj;
            byte num = (byte) 0;
            if (ask.ExchangeDateTime.Ticks != 0L)
                num = (byte) 1;
            writer.Write(num);
            writer.Write(ask.DateTime.Ticks);
            if ((int) num == 1)
                writer.Write(ask.ExchangeDateTime.Ticks);
            writer.Write(ask.ProviderId);
            writer.Write(ask.InstrumentId);
            writer.Write(ask.Price);
            writer.Write(ask.Size);
        }
    }
}
