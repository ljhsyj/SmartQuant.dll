// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class FieldListStreamer : ObjectStreamer
    {
        public FieldListStreamer()
        {
            this.typeId = DataObjectType.FieldList;
            this.type = typeof(FieldList);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            var flist = new FieldList();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                int index = reader.ReadInt32();
                double value = reader.ReadDouble();
                flist.Fields[index] = value;
            }
            return flist;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            var flist = obj as FieldList;
            writer.Write(version);  
            int count = 0;
            for (int i = 0; i < flist.Fields.Size; ++i)
                if (flist.Fields[i] != 0)
                    ++count;

            writer.Write(count);
            for (int i = 0; i < flist.Fields.Size; ++i)
            {
                if (flist.Fields[i] != 0)
                {
                    writer.Write(i);
                    writer.Write(flist.Fields[i]);
                }
            }
        }
    }
}
