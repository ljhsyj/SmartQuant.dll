// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;

namespace SmartQuant
{
    public class StreamerManager
    {
        internal IdArray<ObjectStreamer> streamersById = new IdArray<ObjectStreamer>();
        internal Dictionary<Type, ObjectStreamer> streamersByType = new Dictionary<Type, ObjectStreamer>();

        public StreamerManager()
        {
            Add(new FreeKeyListStreamer());
            Add(new ObjectKeyListStreamer());
            Add(new DataSeriesStreamer());
            Add(new DataKeyIdArrayStreamer());
        }

        public void Add(ObjectStreamer streamer)
        {     
            streamer.streamerManager = this;
            streamersByType.Add(streamer.GetType(), streamer);
            streamersById.Add(streamer.TypeId, streamer);
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var streamer = streamersByType[obj.GetType()];
            writer.Write(streamer.TypeId);
            streamer.Write(writer, obj);
        }

        public void Serialize(BinaryWriter writer, DataObject obj)
        {
            var streamer = streamersById[obj.TypeId];
            writer.Write(streamer.TypeId);
            streamer.Write(writer, obj);
        }

        public object Deserialize(BinaryReader reader)
        {
            var id = reader.ReadByte();
            return streamersById[id].Read(reader);
        }

        #region Extra Helper Methods
        internal void Dump()
        {
            foreach (var streamer in streamersByType.Values)
            {
                Console.WriteLine("TypeId:{0}", streamer.TypeId);
            }
        }
        #endregion
    }
}
