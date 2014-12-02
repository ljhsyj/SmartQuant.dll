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
            var version = reader.ReadByte();
            var error = new ProviderError();
            error.dateTime = DateTime.FromBinary(reader.ReadInt64());
            error.Type = (ProviderErrorType)reader.ReadByte();
            error.ProviderId = reader.ReadByte();
            error.Id = reader.ReadInt32();
            error.Code = reader.ReadInt32();
            error.Text = reader.ReadString();
            return error;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            var version = 0;
            var error = obj as ProviderError;
            writer.Write(version);
            writer.Write(error.dateTime.ToBinary());
            writer.Write((byte)error.Type);
            writer.Write(error.ProviderId);
            writer.Write(error.Id);
            writer.Write(error.Code);
            writer.Write(error.Text);
        }
    }
}
