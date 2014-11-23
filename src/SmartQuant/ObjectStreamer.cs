// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class ObjectStreamer
    {
        protected internal byte typeId;
        protected internal Type type;
        protected internal StreamerManager streamerManager;

        public StreamerManager StreamerManager
        { 
            get
            {
                return this.streamerManager; 
            }
        }

        internal byte TypeId
        {
            get
            {
                return this.typeId;
            }
        }

        public ObjectStreamer()
        {
            this.typeId = DataObjectType.DataObject;
            this.type = typeof(object);
        }

        public virtual object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            return new object();
        }

        public virtual void Write(BinaryWriter writer, object obj)
        {
            writer.Write((byte)0);
        }
    }
}