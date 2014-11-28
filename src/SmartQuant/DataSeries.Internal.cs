using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SmartQuant
{
    class DataKeyIdArray
    {
        internal IdArray<DataKey> keys;

        public DataKeyIdArray(IdArray<DataKey> keys)
        {
            this.keys = keys;
        }

        #region Extra Helper Methods

        public DataKeyIdArray(BinaryReader reader)
        {
            var version = reader.ReadByte();
            var count = reader.ReadInt32();
            this.keys = new IdArray<DataKey>(count);
            int id = reader.ReadInt32();
            while (id != -1)
            {
                var key = new DataKey(null, null, -1, -1);
                key.Read(reader, true);
                keys.Add(id, key);
                id = reader.ReadInt32();
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((byte)0);
            writer.Write(keys.Size);
            for (int i = 0; i < keys.Size; ++i)
            {
                if (keys[i] != null)
                {
                    writer.Write(i);
                    keys[i].WriteKey(writer);
                }
            }
            // -1 means we reach the end.
            writer.Write(-1);
        }

        #endregion
    }

    public partial class DataSeries
    {
        private DataFile dataFile;
        private ObjectKey oKey;
        internal long count;
        internal int bufferCount;
        //        private DateTime minDateTime;
        //        private DateTime maxDateTime;

        // first DataKey postion
        internal long position1 = -1;
        // last DataKey Position
        internal long position2 = -1;

        // the position of the objectkey refenceing it's datakey list
        internal long dKeysKeyPosition = -1;

        private bool readOpened;
        private bool writeOpened;
        private DataKey lastUpdateDKey;
        private DataKey lastReadDKey;
        private DataKey lastDeleteDKey;
        private DataKey lastWriteDKey;
        private IdArray<DataKey> dKeys;
        private ObjectKey dKeysKey;

        internal bool changed;

        internal void InitDataKeys(DataFile dataFile, ObjectKey oKey)
        {
            this.dataFile = dataFile;
            this.oKey = oKey;
            oKey.compressionLevel = 0;
            oKey.compressionMethod = 0;
            if (this.dKeysKeyPosition != -1)
                return;
            this.dKeys = this.bufferCount >= 4096 ? new IdArray<DataKey>(this.bufferCount) : new IdArray<DataKey>(4096);
            this.dKeysKey = new ObjectKey(dataFile, null, new DataKeyIdArray(this.dKeys));
        }

        // reviewed
        private void OpenRead()
        {
            if (this.readOpened)
                Console.WriteLine("DataSeries::OpenRead already read open");
            else
            {
                if (this.dKeys == null)
                    InitDataKeys();
                this.readOpened = true;
            }
        }
        // reviewed
        private void OpenWrite()
        {
            if (this.writeOpened)
                Console.WriteLine("DataSeries::OpenWrite already write open");
            else
            {
                if (this.dKeys == null)
                    InitDataKeys();
                if (this.bufferCount != 0 && this.dKeys[this.bufferCount - 1] != null)
                {
                    this.lastReadDKey = this.dKeys[this.bufferCount - 1];
                    this.lastReadDKey.GetObjects();
                }
                else
                {
                    if (this.position2 != -1)
                    {
                        this.lastReadDKey = GetDataKey(this.position2);
                        this.lastReadDKey.number = this.bufferCount - 1;
                        this.lastReadDKey.GetObjects();
                    }
                    else
                    {
                        this.lastReadDKey = new DataKey(this.dataFile, null, -1, -1);
                        this.lastReadDKey.number = 0;
                        this.lastReadDKey.changed = true;
                        this.bufferCount = 1;
                    }
                    this.dKeys[this.lastReadDKey.number] = this.lastReadDKey;
                }
                this.writeOpened = true;
            }
        }

        private void InitDataKeys()
        {
            // The length of DataKey's name is 0, and take 1 byte in datafile.
            var length = ObjectKey.HEAD_LENGTH + 1;
            this.dKeysKey = this.dataFile.ReadObjectKey(this.dKeysKeyPosition, length);
            this.dKeys = ((DataKeyIdArray)this.dKeysKey.GetObject()).keys;
            for (int i = 0; i < this.dKeys.Size; ++i)
            {
               
                var dkey = this.dKeys[i];
                if (dkey != null)
                {
                    dkey.dataFile = this.dataFile;
                    dkey.number = i;
                //    dkey.Dump();
                }
            }
       }
       
        // reviewed
        private void SaveDataKeysKey()
        {
            if (this.dKeysKey == null)
                this.dKeysKey = new ObjectKey(this.dataFile, "DataKeys", new DataKeyIdArray(this.dKeys));
            this.dataFile.WriteObjectKey(this.dKeysKey);
            this.dKeysKeyPosition = this.dKeysKey.position;
        }


        private void SaveDataObject(DataObject dataObject)
        {
            if (dataObject.DateTime >= this.lastReadDKey.minDateTime && dataObject.DateTime <= this.lastReadDKey.maxDateTime)
            {
                this.lastReadDKey.Add(dataObject);
                if (this.lastReadDKey.count == this.lastReadDKey.capacity)
                {
                    this.SaveWriteDKey(this.lastReadDKey);
                    this.lastReadDKey = new DataKey(this.dataFile, null, this.lastReadDKey.position, -1);
                    this.lastReadDKey.number = this.bufferCount;
                    this.lastReadDKey.index1 = this.count;
                    this.lastReadDKey.index2 = this.count;
                    this.lastReadDKey.changed = true;
                    ++this.bufferCount;
                    this.dKeys[this.lastReadDKey.number] = this.lastReadDKey;
                }
                else
                    this.changed = true;
                this.dataFile.changed = true;
            }
            else
            {
                var dKey = GetKey(dataObject.DateTime, this.lastWriteDKey);
                if (this.lastWriteDKey == null)
                    this.lastWriteDKey = dKey;
                else if (this.lastWriteDKey != dKey)
                {
                    if (this.lastWriteDKey.changed)
                        this.SaveWriteDKey(this.lastWriteDKey);
                    if (!this.CacheObjects && this.lastWriteDKey != this.lastUpdateDKey && (this.lastWriteDKey != this.lastReadDKey && this.lastWriteDKey != this.lastDeleteDKey))
                        this.lastWriteDKey.dataObjects = null;
                    this.lastWriteDKey = dKey;
                }
                this.lastWriteDKey.GetObjects();
                if (this.lastWriteDKey.count < this.lastWriteDKey.capacity)
                {
                    this.lastWriteDKey.Add(dataObject);
                    if (this.lastWriteDKey.count == this.lastWriteDKey.capacity)
                        this.SaveWriteDKey(this.lastWriteDKey);
                }
                else
                {
                    var newDKey = new DataKey(this.dataFile, null, -1, -1);
                    int index = this.lastWriteDKey.GetIndex(dataObject.DateTime, SearchOption.Next);
                    for (int i = index; i < this.lastWriteDKey.count; ++i)
                    {
                        newDKey.Add(this.lastWriteDKey.dataObjects[i]);
                        this.lastWriteDKey.dataObjects[i] = null;
                    }
                    this.lastWriteDKey.count = index;
                    this.lastWriteDKey.index2 = this.lastWriteDKey.index1 + (long)this.lastWriteDKey.count - 1;
                    if (this.lastWriteDKey.count != 0)
                        this.lastWriteDKey.maxDateTime = this.lastWriteDKey.dataObjects[this.lastWriteDKey.count - 1].DateTime;
                    this.lastWriteDKey.Add(dataObject);
                    this.Insert(newDKey, this.lastWriteDKey);
                }
                if (this.lastUpdateDKey != null && this.lastUpdateDKey.number > this.lastWriteDKey.number)
                {
                    ++this.lastUpdateDKey.index1;
                    ++this.lastUpdateDKey.index2;
                }
                if (this.lastReadDKey != null && this.lastReadDKey.number > this.lastWriteDKey.number)
                {
                    ++this.lastReadDKey.index1;
                    ++this.lastReadDKey.index2;
                }
                if (this.lastDeleteDKey != null && this.lastDeleteDKey.number > this.lastWriteDKey.number)
                {
                    ++this.lastDeleteDKey.index1;
                    ++this.lastDeleteDKey.index2;
                }
                this.lastWriteDKey.changed = true;
                this.changed = true;
                this.dataFile.changed = true;
            }
        }

        // reviewed
        private void Insert(DataKey dKey, DataKey dKeyAt)
        {
            for (int i = this.bufferCount; i > dKeyAt.number + 1; --i)
            {
                this.dKeys[i] = this.dKeys[i - 1];
                if (this.dKeys[i] != null)
                    this.dKeys[i].number = i;
            }
            ++this.bufferCount;
            dKey.number = dKeyAt.number + 1;
            this.dKeys[dKey.number] = dKey;
            dKey.prev = dKeyAt.position;
            dKey.next = dKeyAt.next;
            this.SaveWriteDKey(dKey);
            this.dataFile.WriteObjectKey(this.oKey);
        }

        // reviewed
        private void SaveWriteDKey(DataKey dKey)
        {
            long position = dKey.position;
            this.dataFile.WriteObjectKey(dKey);
            if (dKey.position != position)
            {
                DataKey prevDKey = null;
                if (dKey.number != 0)
                    prevDKey = this.dKeys[dKey.number - 1];
                if (prevDKey != null)
                {
                    prevDKey.next = dKey.position;
                    if (!prevDKey.changed)
                        this.WriteDKeyNext(dKey.prev, dKey.position);
                }
                else if (dKey.prev != -1)
                    this.WriteDKeyNext(dKey.prev, dKey.position);

                DataKey nextDKey = null;
                if (dKey.number != this.bufferCount - 1)
                    nextDKey = this.dKeys[dKey.number + 1];
                if (nextDKey != null)
                {
                    nextDKey.prev = dKey.position;
                    if (!nextDKey.changed)
                        this.WriteDKeyPrev(dKey.next, dKey.position);
                }
                else if (dKey.next != -1)
                    this.WriteDKeyPrev(dKey.next, dKey.position);
            }
            if (dKey == this.lastReadDKey)
            {
                if (this.position1 == -1)
                    this.position1 = this.lastReadDKey.position;
                this.position2 = this.lastReadDKey.position;
            }
            this.dataFile.WriteObjectKey(this.oKey);
        }

        // reviewed
        private void UpdateDKeyPrev(DataKey key, long prevValue)
        {
            key.prev = prevValue;
            this.WriteDKeyPrev(key.position, prevValue);
        }
        // reviewed
        private void UpdateDKeyNext(DataKey key, long nextValue)
        {
            key.next = nextValue;
            this.WriteDKeyNext(key.position, nextValue);
        }

        // reviewed
        private void WriteDKeyPrev(long keyPosition, long value)
        {
            int offset = 61; // prev offset
            var mstream = new MemoryStream();
            new BinaryWriter(mstream).Write(value);
            this.dataFile.WriteBuffer(mstream.GetBuffer(), keyPosition + offset, sizeof(long));
        }

        // reviewed
        private void WriteDKeyNext(long keyPosition, long value)
        {
            int offset = 69; // next offset
            var mstream = new MemoryStream();
            new BinaryWriter(mstream).Write(value);
            this.dataFile.WriteBuffer(mstream.GetBuffer(), keyPosition + offset, sizeof(long));
        }

        // reviewed
        private void Delete(DataKey key)
        {
            if (key.position == this.position2)
            {
                if (key.prev != -1)
                {
                    this.position2 = key.prev;
                }
                else
                {
                    this.position1 = -1;
                    this.position2 = -1;
                }
            }
            this.dataFile.DeleteObjectKey(key, false);
            if (key.prev != -1)
            {
                DataKey prevKey = this.dKeys[key.number - 1];
                if (prevKey != null)
                {
                    prevKey.next = key.next;
                    if (!prevKey.changed)
                        this.WriteDKeyNext(key.prev, key.next);
                }
                else
                    this.WriteDKeyNext(key.prev, key.next);
            }
            if (key.next != -1)
            {
                DataKey nextKey = this.dKeys[key.number + 1];
                if (nextKey != null)
                {
                    nextKey.prev = key.prev;
                    if (!nextKey.changed)
                        this.WriteDKeyPrev(key.next, key.prev);
                }
                else
                    this.WriteDKeyPrev(key.next, key.prev);
            }
            for (int i = key.number; i < this.bufferCount - 1; ++i)
            {
                this.dKeys[i] = this.dKeys[i + 1];
                if (this.dKeys[i] != null)
                    this.dKeys[i].number = i;
            }
            --this.bufferCount;
            this.dataFile.WriteObjectKey(this.oKey);
        }
            
        private DataKey GetDataKey(long position)
        {
            var buffer = new byte[DataKey.HEADER_LENGTH];
            var reader = new BinaryReader(new MemoryStream(buffer));
            this.dataFile.ReadBuffer(buffer, position, DataKey.HEADER_LENGTH);
            var key = new DataKey(this.dataFile, null, -1, -1);
            key.Read(reader, true);
            key.position = position;
            return key;
        }

        // reviewed
        private DataKey GetFirstKey()
        {
            var key = this.dKeys[0];
            if (key == null)
            {
                key = GetDataKey(this.position1);
                this.dKeys[0] = key;
            }
            key.number = 0;
            key.index1 = 0;
            key.index2 = (long)(key.count - 1);
            return key;
        }

        private DataKey GetNextKey(DataKey curKey)
        {
            if (curKey.number == -1)
                Console.WriteLine("DataSeries::GetNextKey Error: key.number is not set");
            var nextKey = this.dKeys[curKey.number + 1];
            if (nextKey == null)
            {
                if (curKey.next == -1)
                    Console.WriteLine("DataSeries::GetNextKey Error: key.next is not set");
                nextKey = GetDataKey(curKey.next);
                nextKey.number = curKey.number + 1;
                this.dKeys[nextKey.number] = nextKey;
            }
            nextKey.index1 = curKey.index2 + 1;
            nextKey.index2 = nextKey.index1 + (long)nextKey.count - 1;
            return nextKey;
        }

        private DataKey GetKey(long index, DataKey dKey)
        {
            if (index < 0 || index > this.count - 1)
            {
                Console.WriteLine("DataSeries::GetKey dateTime is out of range : {0}", index);
                return null;
            }

            if (dKey == null)
            {
                dKey = GetFirstKey();
            }
            var dataKey = dKey;
            do
            {
                if (dataKey.index1 <= index && index <= dataKey.index2)
                    return dataKey;
                dataKey = GetNextKey(dataKey);
            } while(dataKey != null);

//            if (dKey != null && dKey.index1 <= index && index <= dKey.index2)
//                return dKey;
//
//            var dataKey = dKey != null && index > dKey.index2 ? GetNextKey(dKey) : GetFirstKey();
//            while (index < dataKey.index1 || index > dataKey.index2)
//                dataKey = GetNextKey(dataKey);
            return dataKey;
        }

        // reviewed
        private DataKey GetKey(DateTime dateTime, DataKey dKey)
        {
            if (this.count == 0 || dateTime > DateTime2)
            {
                Console.WriteLine("DataSeries::GetKey dateTime is out of range : " + dateTime);
                return null;
            }
            if (dateTime <= DateTime1)
                return this.GetFirstKey();
            if (dKey != null && dKey.minDateTime <= dateTime && dateTime <= dKey.maxDateTime)
                return dKey;
            var dataKey = (dKey != null && dateTime > dKey.maxDateTime) ? this.GetNextKey(dKey) : this.GetFirstKey();
            while ((dateTime <= dataKey.minDateTime) && (dateTime >= dataKey.maxDateTime))
                dataKey = GetNextKey(dataKey);
            return dataKey;
        }

        internal void Flush()
        {
            if (!this.changed)
                return;

            if (this.lastWriteDKey != null && this.lastWriteDKey.changed)
                this.SaveWriteDKey(this.lastWriteDKey);
            if (this.lastReadDKey != null && this.lastReadDKey.changed)
                this.SaveWriteDKey(this.lastReadDKey);
            if (this.lastDeleteDKey != null && this.lastDeleteDKey.changed)
                SaveWriteDKey(this.lastDeleteDKey);

            SaveDataKeysKey();
            this.dataFile.WriteObjectKey(this.oKey);
            this.changed = false;
        }

        #region Extra Helper Methods

        internal DataSeries(BinaryReader reader)
            : this()
        {
            var version = reader.ReadByte();
            count = reader.ReadInt64();
            bufferCount = reader.ReadInt32();
            dKeysKeyPosition = reader.ReadInt64();
            DateTime1 = new DateTime(reader.ReadInt64());
            DateTime2 = new DateTime(reader.ReadInt64());
            position1 = reader.ReadInt64();
            position2 = reader.ReadInt64();
            Name = reader.ReadString();
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write((byte)0);
            writer.Write(this.count);
            writer.Write(this.bufferCount);
            writer.Write(this.dKeysKeyPosition);
            writer.Write(this.DateTime1.Ticks);
            writer.Write(this.DateTime2.Ticks);
            writer.Write(this.position1);
            writer.Write(this.position2);
            writer.Write(this.Name);
        }

        #endregion
    }
}
