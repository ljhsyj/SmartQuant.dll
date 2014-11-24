// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class AskStreamer : ObjectStreamer
    {
        public AskStreamer()
        {
            this.typeId = DataObjectType.Ask;
            this.type = typeof(Ask);
        }

        public override object Read(BinaryReader reader)
        {
            return new Ask(reader);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            (obj as Ask).Write(writer);
        }
    }
}
