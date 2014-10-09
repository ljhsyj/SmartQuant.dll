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
            throw new NotImplementedException();
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            throw new NotImplementedException();
        }
    }
}
