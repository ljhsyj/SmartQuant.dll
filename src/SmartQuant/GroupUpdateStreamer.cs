// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class GroupUpdateStreamer : ObjectStreamer
    {
        public GroupUpdateStreamer()
        {
            this.typeId = DataObjectType.GroupUpdate;
            this.type = typeof(GroupUpdate);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            reader.ReadInt32();
            this.streamerManager.Deserialize(reader);
            return null;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            writer.Write(version);
            var ge = obj as GroupEvent;
            this.streamerManager.Serialize(writer, ge.Obj);
        }
    }
}
