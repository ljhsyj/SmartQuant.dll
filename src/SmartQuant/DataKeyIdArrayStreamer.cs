// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class DataKeyIdArrayStreamer : ObjectStreamer
    {
        public DataKeyIdArrayStreamer()
        {
            this.typeId = ObjectType.DataKeyIdArray;
            this.type = typeof(DataKeyIdArray);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            (obj as DataKeyIdArray).Write(writer);
        }

        public override object Read(BinaryReader reader)
        {
            return new DataKeyIdArray(reader);
        }
    }
}

