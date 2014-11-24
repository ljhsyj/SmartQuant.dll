// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.IO;
using System;

namespace SmartQuant
{
    public class DataObjectStreamer : ObjectStreamer
    {
        public DataObjectStreamer()
        {
            this.typeId = DataObjectType.DataObject;
            this.type = typeof(DataObject);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            return  new DataObject(new DateTime(reader.ReadInt64()));
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((byte)0);
            writer.Write((obj as DataObject).DateTime.Ticks);
        }
    }
}
