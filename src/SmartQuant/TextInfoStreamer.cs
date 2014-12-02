// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class TextInfoStreamer : ObjectStreamer
    {
        public TextInfoStreamer()
        {
            this.typeId = DataObjectType.Text;
            this.type = typeof(TextInfo);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            return new TextInfo(new DateTime(reader.ReadInt64()), reader.ReadString());
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            writer.Write(version);
            var info = obj as TextInfo;
            writer.Write(info.DateTime.Ticks);
            writer.Write(info.Content);
        }
    }
}
