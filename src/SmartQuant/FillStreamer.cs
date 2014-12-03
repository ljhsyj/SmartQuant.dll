// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class FillStreamer : ObjectStreamer
    {
        public FillStreamer()
        {
            this.typeId = DataObjectType.Fill;
            this.type = typeof(Fill);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            var fill = new Fill();
            fill.DateTime = new DateTime(reader.ReadInt64());
            reader.ReadInt32();
            fill.CurrencyId = reader.ReadByte();
            fill.Side = (OrderSide)Enum.Parse(typeof(OrderSide), reader.ReadString());
            fill.Qty = reader.ReadDouble();
            fill.Price = reader.ReadDouble();
            fill.Text = reader.ReadString();
            return fill;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            int foo = 0;
            writer.Write(version);
            Fill fill = obj as Fill;
            writer.Write(fill.DateTime.Ticks);
            writer.Write(foo);
            writer.Write(fill.CurrencyId);
            writer.Write(fill.Side.ToString());
            writer.Write(fill.Qty);
            writer.Write(fill.Price);
            writer.Write(fill.Text);
        }
    }
}
