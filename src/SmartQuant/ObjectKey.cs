using System;
using System.IO;
using System.Collections.Generic;

namespace SmartQuant
{
    public partial class ObjectKey : IComparable<ObjectKey>
    {
        public byte TypeId
        {
            get
            {
                return this.typeId;
            }
        }

        public ObjectKey()
        {
        }

        public ObjectKey(DataFile file, string name = null, object obj = null)
        {
            this.name = name;
            this.obj = obj;
            if (file == null)
                return;
            this.compressionLevel = file.CompressionLevel;
            this.compressionMethod = file.CompressionMethod;
            Init(file);
        }

        public virtual object GetObject()
        {
            if (this.obj != null)
                return this.obj;
            if (this.contentLength == -1)
                return null;
             Console.WriteLine(TypeId);
             this.dataFile.streamerManager.Dump();
            this.obj = this.dataFile.streamerManager.streamersById[TypeId].Read(new BinaryReader(new MemoryStream(GetUncompressedBytes(true))));
            if (TypeId == ObjectType.DataSeries)
                ((DataSeries)this.obj).InitDataKeys(this.dataFile, this);
            return this.obj;
        }

        public virtual void Dump()
        {
            var streamer = this.dataFile.streamerManager.streamersById[this.typeId];
            if (streamer != null)
                Console.WriteLine("{0} of typeId {1} ({2}) position = {3}", this.name, this.typeId, streamer.type, this.position);
            else
                Console.WriteLine("{0} of typeId {1} (Unknown streamer, typeId = {2}) position = {3}", this.name, this.typeId, streamer.type, this.position);
        }

        public int CompareTo(ObjectKey other)
        {
            return other == null ? 1 : this.totalLength.CompareTo(other.totalLength);
        }
    }

    class ObjectKeyList
    {
        internal Dictionary<string, ObjectKey> keys;

        internal ObjectKeyList(Dictionary<string, ObjectKey> keys)
        {
            this.keys = keys;
        }

        #region Extra Hepler Methods

        internal ObjectKeyList(BinaryReader reader)
        {
            var keys = new Dictionary<string, ObjectKey>();
            var version = reader.ReadByte();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                var key = new ObjectKey(reader);
                keys.Add(key.name, key);
            }
            this.keys = keys;
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write((byte)0);
            writer.Write(keys.Count);
            foreach (var key in keys.Values)
                key.WriteKey(writer);
        }
        #endregion
    }
}
