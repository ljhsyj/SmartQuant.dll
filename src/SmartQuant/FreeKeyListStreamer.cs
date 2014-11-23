// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;
using System.Collections.Generic;

namespace SmartQuant
{
    public class FreeKeyListStreamer : ObjectStreamer
    {
        public FreeKeyListStreamer()
        {
            this.typeId = ObjectType.FreeKeyList;
            this.type = typeof(FreeKeyList);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            var keys = ((FreeKeyList)obj).keys;
            writer.Write((byte)0);
            writer.Write(keys.Count);
            foreach (var key in keys)
                key.WriteKey(writer);
        }

        public override object Read(BinaryReader reader)
        {
            var keys = new List<FreeKey>();
            var version = reader.ReadByte();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                var key = new FreeKey();
                key.ReadKey(reader, true);
                keys.Add(key);
            }
            return new FreeKeyList(keys);
        }
    }
}

