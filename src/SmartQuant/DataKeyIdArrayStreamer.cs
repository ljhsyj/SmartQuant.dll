// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    class DataKey
    {
    }

    class DataKeyIdArray
    {
    }

    public class DataKeyIdArrayStreamer : ObjectStreamer
    {
        public DataKeyIdArrayStreamer()
        {
            this.typeId = ObjectType.DataKeyIdArray;
            this.type = typeof(DataKeyIdArray);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            throw new NotImplementedException();
        }

        public override object Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}

