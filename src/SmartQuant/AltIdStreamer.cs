// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class AltIdStreamer : ObjectStreamer
    {
        public AltIdStreamer()
        {
            this.typeId = ObjectType.AltId;
            this.type = typeof(AltId);
        }

        public override object Read(BinaryReader reader)
        {
            var v = reader.ReadByte();
            return new AltId() { ProviderId = reader.ReadByte(), Symbol = reader.ReadString(), Exchange = reader.ReadString() };
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            writer.Write(version);
            var altId = obj as AltId;
            writer.Write(altId.ProviderId);
            writer.Write(altId.Symbol);
            writer.Write(altId.Exchange);
        }
    }
}
