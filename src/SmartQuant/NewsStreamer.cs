// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class NewsStreamer : ObjectStreamer
    {
        public NewsStreamer()
        {
            this.typeId = DataObjectType.News;
            this.type = typeof(News);
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
