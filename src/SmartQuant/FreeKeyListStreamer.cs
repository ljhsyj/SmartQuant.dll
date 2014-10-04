// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    class FreeKeyList
    {
    }

    public class FreeKeyListStreamer : ObjectStreamer
    {
        public FreeKeyListStreamer()
        {
            this.typeId = ObjectType.FreeKeyList;
            this.type = typeof(FreeKeyList);
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

