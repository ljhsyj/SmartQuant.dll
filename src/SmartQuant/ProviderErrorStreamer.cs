// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class ProviderErrorStreamer : ObjectStreamer
    {
        public ProviderErrorStreamer()
        {
            this.typeId = DataObjectType.ProviderError;
            this.type = typeof(ProviderError);
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
