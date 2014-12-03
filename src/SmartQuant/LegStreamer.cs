// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class LegStreamer : ObjectStreamer
    {
        public LegStreamer()
        {
            this.typeId = ObjectType.Leg;
            this.type = typeof(Leg);
        }

        public override object Read(BinaryReader reader)
        {
            return new Leg(reader);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            (obj as Leg).Write(writer);
        }
    }
}
