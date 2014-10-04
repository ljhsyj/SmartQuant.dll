// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    class ObjectKeyList
    {
    }

    public class ObjectKeyListStreamer : ObjectStreamer
    {
        public ObjectKeyListStreamer()
        {
            this.typeId = ObjectType.ObjectKeyList;
            this.type = typeof(ObjectKeyList);
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

