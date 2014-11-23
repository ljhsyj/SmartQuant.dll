// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public partial class DataSeries : IDataSeries
    {
        public string Name { get; internal set; }

        public string Description { get; private set; }

        public long Count
        {
            get
            {
                return this.count;
            }
        }

        public DateTime DateTime1 { get; private set; }

        public DateTime DateTime2 { get; private set; }

        public bool CacheObjects { get; set; }

        public DataObject this [long index]
        {
            get
            {
                return Get(index);
            }
        }

        public DataObject this [DateTime dateTime]
        {
            get
            {
                return Get(dateTime);
            }
        }

        public DataSeries()
            : this((string)null)
        {
        }

        public DataSeries(string name)
        {
            Name = name;
        }

        public virtual void Update(long index, DataObject obj)
        {
            if (Get(index).DateTime != obj.DateTime)
                Console.WriteLine("DataSeries::Update Can not update object with different datetime");
            else
            {
                bool flag = this.lastUpdateDKey.changed;
                this.lastUpdateDKey.Update((int)(index - this.lastUpdateDKey.index1), obj);
                if (!flag)
                    SaveWriteDKey(this.lastUpdateDKey);
                this.dataFile.changed = true;
            }
        }

        public virtual void Add(DataObject dataObject)
        {
            if (dataObject.DateTime.Ticks == 0)
                Console.WriteLine("DataSeries::Add Error: can not add object with DateTime = 0");
            else
            {
                if (!this.writeOpened)
                    OpenWrite();
                ++this.count;
                if (this.count == 1)
                {
                    DateTime1 = dataObject.DateTime;
                    DateTime2 = dataObject.DateTime;
                }
                else if (dataObject.DateTime < DateTime2)
                {
                    SaveDataObject(dataObject);
                    return;
                }
                else
                    DateTime2 = dataObject.DateTime;
                this.lastReadDKey.Add(dataObject);
                if (this.lastReadDKey.count == this.lastReadDKey.capacity)
                {
                    SaveWriteDKey(this.lastReadDKey);
                    if (!CacheObjects && this.lastReadDKey != this.lastUpdateDKey && this.lastReadDKey != this.lastWriteDKey && this.lastReadDKey != this.lastDeleteDKey)
                        this.lastReadDKey.dataObjects = null;
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
        }

        public virtual bool Contains(DateTime dateTime)
        {
            return GetIndex(dateTime, SearchOption.ExactFirst) != -1;
        }

        public virtual DataObject Get(long index)
        {
            if (!this.readOpened)
                OpenRead();
            var dKey = GetKey(index, this.lastUpdateDKey);

            if (dKey == null)
                return null;
           // dKey.Dump();
            if (dKey != this.lastUpdateDKey)
            {
                if (!CacheObjects && this.lastUpdateDKey != null && this.lastUpdateDKey != this.lastReadDKey && this.lastUpdateDKey != this.lastWriteDKey && this.lastUpdateDKey != this.lastDeleteDKey)
                    this.lastUpdateDKey.dataObjects = null;
                this.lastUpdateDKey = dKey;
            }
            return dKey.GetObjects()[index - dKey.index1];
        }

        public virtual void Remove(long index)
        {
            if (!this.writeOpened)
                OpenWrite();
            var dKey1 = GetKey(index, this.lastDeleteDKey);
            if (dKey1 == null)
                return;
            if (this.lastDeleteDKey == null)
                this.lastDeleteDKey = dKey1;
            else if (this.lastDeleteDKey != dKey1)
            {
                if (this.lastDeleteDKey.changed)
                    this.SaveWriteDKey(this.lastDeleteDKey);
                if (!this.CacheObjects && this.lastDeleteDKey != this.lastUpdateDKey && (this.lastDeleteDKey != this.lastReadDKey && this.lastDeleteDKey != this.lastWriteDKey))
                    this.lastDeleteDKey.dataObjects = null;
                this.lastDeleteDKey = dKey1;
            }
            dKey1.Remove(index - dKey1.index1);
            --dKey1.index2;
            if (this.lastUpdateDKey != null && this.lastUpdateDKey.number > dKey1.number)
            {
                --this.lastUpdateDKey.index1;
                --this.lastUpdateDKey.index2;
            }
            if (this.lastReadDKey != null && this.lastReadDKey.number > dKey1.number)
            {
                --this.lastReadDKey.index1;
                --this.lastReadDKey.index2;
            }
            if (this.lastWriteDKey != null && this.lastWriteDKey.number > dKey1.number)
            {
                --this.lastWriteDKey.index1;
                --this.lastWriteDKey.index2;
            }
            if (dKey1.count == 0)
            {
                Delete(dKey1);
                this.lastDeleteDKey = null;
            }
            --this.count;
            this.changed = true;
            this.dataFile.changed = true;
        }

        public virtual long GetIndex(DateTime dateTime, SearchOption option = SearchOption.Prev)
        {
            if (!this.readOpened)
                OpenRead();
            if (this.count == 0 || dateTime > DateTime2)
            {
                Console.WriteLine("DataSeries::GetIndex dateTime is out of range : " + dateTime);
                return -1;
            }
            else
            {
                if (dateTime <= DateTime1)
                    return 0;
                var key = GetKey(dateTime, this.lastUpdateDKey);
                if (key == null)
                    return -1;
                if (key != this.lastUpdateDKey)
                {
                    if (!CacheObjects && this.lastUpdateDKey != null && this.lastUpdateDKey != this.lastReadDKey && this.lastUpdateDKey != this.lastWriteDKey && this.lastUpdateDKey != this.lastDeleteDKey)
                        this.lastUpdateDKey.dataObjects = null;
                    this.lastUpdateDKey = key;
                }
                return this.lastUpdateDKey.index1 + (long)this.lastUpdateDKey.GetIndex(dateTime, option);
            }
        }

        public virtual DataObject Get(DateTime dateTime)
        {
            if (!this.readOpened)
                OpenRead();
            if (this.count == 0 || dateTime > DateTime2)
            {
                Console.WriteLine("dateTime out of range: {0}", dateTime);
                return null;
            }

            if (dateTime <= DateTime1)
                return Get(0);

            var dKey = GetKey(dateTime, this.lastUpdateDKey);
            if (dKey == null)
                return null;
            if (dKey != this.lastUpdateDKey)
            {
                if (!CacheObjects && this.lastUpdateDKey != null && this.lastUpdateDKey != this.lastReadDKey && this.lastUpdateDKey != this.lastWriteDKey && this.lastUpdateDKey != this.lastDeleteDKey)
                    this.lastUpdateDKey.dataObjects = null;
                this.lastUpdateDKey = dKey;
            }
            return this.lastUpdateDKey.Get(dateTime);

        }

        public virtual void Refresh()
        {
        }
        // reviewed!
        public virtual void Clear()
        {
            if (this.dKeys == null)
                InitDataKeys();

            long position = this.position1;
            while (position != -1)
            {
                var dKey = GetDataKey(position);
                this.dataFile.DeleteObjectKey(dKey, false);
                position = dKey.next;
            }

            //            if (this.position1 != -1)
            //            {
            //                DataKey dKey = this.GetDataKey(this.position1);
            //                while (true)
            //                {
            //                    this.dataFile.DeleteObjectKey((ObjectKey)dKey, false);
            //                    if (dKey.next != -1)
            //                        dKey = this.GetDataKey(dKey.next);
            //                    else
            //                        break;
            //                }
            //            }
            this.count = 0;
            this.bufferCount = 0;
            DateTime1 = new DateTime(0);
            DateTime2 = new DateTime(0);
            this.position1 = -1;
            this.position2 = -1;
            this.readOpened = false;
            this.writeOpened = false;
            this.dKeys = new IdArray<DataKey>(4096);
            this.dKeysKey.obj = new DataKeyIdArray(this.dKeys);
            this.lastUpdateDKey = null;
            this.lastReadDKey = null;
            this.lastDeleteDKey = null;
            this.lastWriteDKey = null;
            this.changed = true;
            Flush();
        }
        // reviewed
        public void Dump()
        {
            Console.WriteLine("Data series: {0}", Name);
            Console.WriteLine("Count = {0}",this.count);
            Console.WriteLine("Position1 = {0}", position1);
            Console.WriteLine("Position2 = {0}", this.position2);
            Console.WriteLine("DateTime1 = {0}", new DateTime(DateTime1.Ticks));
            Console.WriteLine("DateTime2 = {0}", new DateTime(DateTime2.Ticks));
            Console.WriteLine("Buffer count = {0}", this.bufferCount);
            Console.WriteLine("dkeysKeyPostion = {0}", this.dKeysKeyPosition);
            Console.WriteLine("\nKeys in cache:\n");

            if (dKeys != null)
            {
                Console.WriteLine("KeyCount:{0}", dKeys.Size);
                for (int i = 0; i < this.bufferCount; ++i)
                {
                    if (this.dKeys[i] != null)
                        Console.WriteLine(this.dKeys[i]);
                }
            }
            Console.WriteLine();
            Console.WriteLine("Keys on disk:");
            Console.WriteLine();

            long position = this.position1;
            while (position != -1)
            {
                var dKey = GetDataKey(position);
                Console.WriteLine(dKey);
                position = dKey.next;
            }

            if (this.lastReadDKey != null)
                Console.WriteLine("Write Key : " + this.changed);
            else
                Console.WriteLine("Write Key : null");
            Console.WriteLine("\nEnd dump\n");
        }
    }
}
