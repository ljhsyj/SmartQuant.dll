// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class Level2SnapshotStreamer : ObjectStreamer
    {
        public Level2SnapshotStreamer()
        {
            this.typeId = DataObjectType.Level2Snapshot;
            this.type = typeof(Level2Snapshot);
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
