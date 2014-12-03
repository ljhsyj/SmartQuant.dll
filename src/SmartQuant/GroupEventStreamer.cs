// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class GroupEventStreamer : ObjectStreamer
    {
        public GroupEventStreamer()
        {
            this.typeId = DataObjectType.GroupEvent;
            this.type = typeof(GroupEvent);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            int groupId = reader.ReadInt32();
            return new GroupEvent(this.streamerManager.Deserialize(reader) as Event, groupId);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            var ge = obj as GroupEvent;
            writer.Write(version);
            writer.Write(ge.Group == null ? ge.GroupId : ge.Group.Id);
            this.streamerManager.Serialize(writer, ge.Obj);
        }
    }
}
