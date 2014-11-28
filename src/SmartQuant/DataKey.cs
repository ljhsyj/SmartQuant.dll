using System;
using System.Runtime.InteropServices;
using System.IO;

namespace SmartQuant
{
    // This class is for size calculation only.
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    class DataKeyHead
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string Label = "DKey";

        [FieldOffset(5)]
        public bool Freed;

        [FieldOffset(6)]
        public DateTime DateTime;

        [FieldOffset(14)]
        public long Position;

        [FieldOffset(22)]
        private int HeaderLength;

        [FieldOffset(26)]
        private int ContentLength;

        [FieldOffset(30)]
        private int TotalLength;

        [FieldOffset(34)]
        private byte CompressionMethod;

        [FieldOffset(35)]
        private byte CompressionLevel;

        [FieldOffset(36)]
        private byte TypeId;

        [FieldOffset(37)]
        public int Capacity;

        [FieldOffset(41)]
        public int Count;

        [FieldOffset(45)]
        public long MinDateTimeTicks;

        [FieldOffset(53)]
        public long MaxDateTimeTicks;

        [FieldOffset(61)]
        public long Prev;

        [FieldOffset(69)]
        public long Next;
    }

//    // This class is for size calculation only.
//    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
//    class DataKeyHeader : ObjectKeyHeader
//    {
//        public int Capacity;
//        public int Count;
//        public long MinDateTimeTicks;
//        public long MaxDateTimeTicks;
//        public long Prev;
//        public long Next;
//
//        public DataKeyHeader()
//        {
//            Label = "DKey";
//        }
//    }

    // DataKey is a special type of ObjectKey used with DataSeries.
    class DataKey : ObjectKey
    {
        public static readonly int HEADER_LENGTH = Marshal.SizeOf(typeof(DataKeyHead));
        internal DataObject[] dataObjects;
        internal int capacity = 10000;
        internal int count;
        internal DateTime minDateTime;
        internal DateTime maxDateTime;
        internal int number = -1;
        internal long index1;
        internal long index2;
        internal long prev = -1;
        internal long next = -1;

        public DataKey(DataFile dataFile, object obj = null, long prev = -1, long next = -1)
            : base(dataFile, "", obj)
        {
            this.label = "DKey";
            this.headerLength = HEADER_LENGTH;
            this.prev = prev;
            this.next = next;
        }

        // reviewed!
        public void Add(DataObject dataObject)
        {
            if (this.count == this.capacity)
                Console.WriteLine("DataKey::Add Can not add object. Buffer is full.");
            else
            {
                if (this.dataObjects == null)
                    this.dataObjects = new DataObject[this.capacity];
                if (this.count == 0)
                {
                    this.dataObjects[this.count++] = dataObject;
                    this.minDateTime = dataObject.DateTime;
                    this.maxDateTime = dataObject.DateTime;
                }
                else if (dataObject.DateTime >= this.dataObjects[this.count - 1].DateTime)
                {
                    this.dataObjects[this.count++] = dataObject;
                    this.maxDateTime = dataObject.DateTime;
                }
                else
                {
                    int index = this.count;
                    while (true)
                    {
                        this.dataObjects[index] = this.dataObjects[index - 1];
                        if (!(dataObject.DateTime >= this.dataObjects[index].DateTime) && index != 1)
                            --index;
                        else
                            break;
                    }
                    this.dataObjects[index - 1] = dataObject;
                    if (index == 1)
                        this.minDateTime = dataObject.DateTime;
                    ++this.count;
                }
                ++this.index2;
                this.changed = true;
            }
        }

        public void Update(int index, DataObject dataObject)
        {
            this.dataObjects[index] = dataObject;
            this.changed = true;
        }

        public void Remove(long index)
        {
            if (this.dataObjects == null)
                GetObjects();
            for (long i = index; i < (long)(this.count - 1); ++i)
                this.dataObjects[i] = this.dataObjects[i + 1];
            --this.count;
            this.changed = true;
            if (this.count == 0)
                return;
            if (index == 0)
                this.minDateTime = this.dataObjects[0].DateTime;
            if (index == this.count)
                this.maxDateTime = this.dataObjects[this.count - 1].DateTime;
        }

        public DataObject[] GetObjects()
        {
            if (this.dataObjects != null)
                return this.dataObjects;
            this.dataObjects = new DataObject[this.capacity];
            if (this.contentLength == -1)
                return this.dataObjects;
            var reader = new BinaryReader(new MemoryStream(GetUncompressedBytes(true)));
            for (int i = 0; i < this.count; ++i)
            {
            
                this.dataObjects[i] = (DataObject)this.dataFile.streamerManager.Deserialize(reader);
//                Console.WriteLine(i);
//                Console.WriteLine("{0}", this.dataObjects[i].GetType());
//                if (this.dataObjects[i] is Ask)
//                {
//                    var ask = this.dataObjects[i] as Ask;
//                    Console.WriteLine("dt:{0}", ask.DateTime);
//                    Console.WriteLine("price:{0}", ask.Price);
//                    Console.WriteLine("size:{0}", ask.Size);
//
//                }
            }
            return this.dataObjects;
        }

        public DataObject Get(int index)
        {
            return GetObjects()[index];
        }

        public DataObject Get(DateTime dateTime)
        {
            if (this.dataObjects == null)
                GetObjects();
            for (int i = 0; i < this.count; ++i)
                if (this.dataObjects[i].DateTime >= dateTime)
                    return this.dataObjects[i];
            return null;
        }

        public int GetIndex(DateTime dateTime, SearchOption option = SearchOption.Next)
        {
            if (this.dataObjects == null)
                GetObjects();
            for (int i = 0; i < this.count; ++i)
            {
                if (this.dataObjects[i].DateTime >= dateTime)
                {
                    switch (option)
                    {
                        case SearchOption.Next:
                            return i;
                        case SearchOption.Prev:
                            return this.dataObjects[i].DateTime == dateTime ? i : i - 1;
                        case SearchOption.ExactFirst:
                            return this.dataObjects[i].DateTime == dateTime ? i : -1;
                        default:
                            Console.WriteLine("DataKey::GetIndex Unknown search option: " + option);
                            continue;
                    }
                }
            }
            return -1;
        }

        internal override byte[] WriteObjectData(bool compress = true)
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            var streamerManager = this.dataFile.streamerManager;
            byte typeId = this.dataObjects[0].TypeId;
            var objectStreamer = streamerManager.streamersById[typeId];
            for (int i = 0; i < this.count; ++i)
            {
                if (this.dataObjects[i].TypeId != typeId)
                {
                    typeId = this.dataObjects[i].TypeId;
                    objectStreamer = streamerManager.streamersById[typeId];
                }
                writer.Write(objectStreamer.typeId);
                objectStreamer.Write(writer, this.dataObjects[i]);
            }
            var buf = memoryStream.ToArray();
            return compress && this.compressionLevel != 0 ? new QuickLZ().Compress(buf) : buf;
        }

        internal override void Write(BinaryWriter writer)
        {
            var buffer = WriteObjectData(true);
            this.headerLength = HEADER_LENGTH;
            this.contentLength = buffer.Length;
            if (this.totalLength == -1)
                this.totalLength = this.headerLength + this.contentLength;
            writer.Write(this.label);
            writer.Write(this.freed);
            writer.Write(this.dateTime.Ticks);
            writer.Write(this.position);
            writer.Write(this.headerLength);
            writer.Write(this.contentLength);
            writer.Write(this.totalLength);
            writer.Write(this.compressionMethod);
            writer.Write(this.compressionLevel);
            writer.Write(this.TypeId);
            writer.Write(this.capacity);
            writer.Write(this.count);
            writer.Write(this.minDateTime.Ticks);
            writer.Write(this.maxDateTime.Ticks);
            writer.Write(this.prev);
            writer.Write(this.next);
            writer.Write(buffer, 0, buffer.Length);
        }

        // reviewed!
        internal override void WriteKey(BinaryWriter writer)
        {
            writer.Write(this.label);
            writer.Write(this.freed);
            writer.Write(this.dateTime.Ticks);
            writer.Write(this.position);
            writer.Write(this.headerLength);
            writer.Write(this.contentLength);
            writer.Write(this.totalLength);
            writer.Write(this.compressionMethod);
            writer.Write(this.compressionLevel);
            writer.Write(this.TypeId);
            writer.Write(this.capacity);
            writer.Write(this.count);
            writer.Write(this.minDateTime.Ticks);
            writer.Write(this.maxDateTime.Ticks);
            writer.Write(this.prev);
            writer.Write(this.next);
        }

        internal override void Read(BinaryReader reader, bool readLabel = true)
        {
            if (readLabel)
            {
                this.label = reader.ReadString();
                if (this.label != "DKey")
                    Console.WriteLine("DataKey::Read This is not DataKey! label = " + this.label);
            }
            this.freed = reader.ReadBoolean();
            this.dateTime = new DateTime(reader.ReadInt64());
            this.position = reader.ReadInt64();
            this.headerLength = reader.ReadInt32();
            this.contentLength = reader.ReadInt32();
            this.totalLength = reader.ReadInt32();
            this.compressionMethod = reader.ReadByte();
            this.compressionLevel = reader.ReadByte();
            this.typeId = reader.ReadByte();
            this.capacity = reader.ReadInt32();
            this.count = reader.ReadInt32();
            this.minDateTime = new DateTime(reader.ReadInt64());
            this.maxDateTime = new DateTime(reader.ReadInt64());
            this.prev = reader.ReadInt64();
            this.next = reader.ReadInt64();
        }

        public override string ToString()
        {
            return string.Format("DataKey: position = {0} prev = {1} next = {2} number {3} size = {4} count = {5} index1 = {6} index2 = {7}", this.position, this.prev, this.next, this.number, this.capacity, this.count, this.index1, this.index2);
        }

        public override void Dump()
        {
            Console.WriteLine(this);
        }
    }

}
