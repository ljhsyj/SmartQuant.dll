// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;
using System.Collections.Generic;

namespace SmartQuant
{
    public class ObjectKeyListStreamer : ObjectStreamer
    {
        public ObjectKeyListStreamer()
        {
            this.typeId = ObjectType.ObjectKeyList;
            this.type = typeof(ObjectKeyList);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            (obj as ObjectKeyList).Write(writer);
        }

        public override object Read(BinaryReader reader)
        {
            return new ObjectKeyList(reader);
        }
    }
}

