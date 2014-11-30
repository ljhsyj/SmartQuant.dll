using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SmartQuant
{
    // This class is for size calculation only.
//    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
//    class ObjectKeyHead
//    {
//        [FieldOffset(0)]
//        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
//        public string Label = "OKey";
//
//        [FieldOffset(5)]
//        public bool Freed;
//
//        [FieldOffset(6)]
//        public DateTime DateTime;
//
//        [FieldOffset(14)]
//        public long Position = -1;
//
//        [FieldOffset(22)]
//        private int HeaderLength = -1;
//
//        [FieldOffset(26)]
//        private int ContentLength = -1;
//
//        [FieldOffset(30)]
//        private int TotalLength = -1;
//
//        [FieldOffset(34)]
//        private byte CompressionMethod = 1;
//
//        [FieldOffset(35)]
//        private byte CompressionLevel = 1;
//
//        [FieldOffset(36)]
//        private byte TypeId = 1;
//    }

    // This class is for size calculation only.
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    class ObjectKeyHeader
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string Label = "OKey";
        public bool Freed;
        public DateTime DateTime;
        public long Position = -1;
        private int HeaderLength = -1;
        private int ContentLength = -1;
        private int TotalLength = -1;
        private byte CompressionMethod = 1;
        private byte CompressionLevel = 1;
        private byte TypeId = 1;
    }

    public partial class ObjectKey
    {
//        internal static readonly int HEAD_LENGTH = Marshal.SizeOf(typeof(ObjectKeyHead));
        internal static readonly int HEAD_LENGTH = 37;

        internal byte typeId;
        internal byte compressionMethod = 1;
        internal byte compressionLevel = 1;
        internal string name;
        internal DataFile dataFile;

        internal object obj;

        internal string label = "OKey";
        internal bool freed;
        internal DateTime dateTime;
        internal long position = -1;
        internal int headerLength = -1;
        internal int contentLength = -1;
        internal int totalLength = -1;
        protected internal bool changed;

        internal void Init(DataFile dataFile)
        {
            this.dataFile = dataFile;
            if (this.obj == null)
                return;
            ObjectStreamer streamer;
            dataFile.streamerManager.streamersByType.TryGetValue(this.obj.GetType(), out streamer);
            if (streamer != null)
                this.typeId = streamer.typeId;
            else
                Console.WriteLine("ObjectKey::Init Can not find streamer for object of type " + this.obj.GetType());
        }

        internal byte[] GetUncompressedBytes(bool compress)
        {
            var buffer = new byte[this.contentLength];
            this.dataFile.ReadBuffer(buffer, this.position + this.headerLength, this.contentLength);
            return compress && this.compressionLevel != 0 ? new QuickLZ().Decompress(buffer) : buffer;
        }

        internal virtual byte[] WriteObjectData(bool compress = true)
        {
            var mstream = new MemoryStream();
            this.dataFile.streamerManager.streamersByType[this.obj.GetType()].Write(new BinaryWriter(mstream), this.obj);
            var buffer = mstream.ToArray();
            return compress && this.compressionLevel != 0 ? new QuickLZ().Compress(buffer) : buffer;
        }

        internal virtual void Write(BinaryWriter writer)
        {
            byte[] contentBuf = WriteObjectData(true);
            this.headerLength = HEAD_LENGTH + this.name.Length + 1;
            this.contentLength = contentBuf.Length;
            if (this.totalLength == -1)
                this.totalLength = this.headerLength + this.contentLength;
            writer.Write(this.label);                 // 4b
            writer.Write(this.freed);                 // 5b
            writer.Write(this.dateTime.Ticks);        // 13b
            writer.Write(this.position);              // 21b
            writer.Write(this.headerLength);          // 25b
            writer.Write(this.contentLength);         // 29b
            writer.Write(this.totalLength);           // 33b
            writer.Write(this.compressionMethod);     // 34b
            writer.Write(this.compressionLevel);      // 35b 
            writer.Write(this.typeId);                // 36b 
            writer.Write(this.name);
            writer.Write(contentBuf, 0, contentBuf.Length);
        }

        internal virtual void WriteKey(BinaryWriter writer)
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
            writer.Write(this.typeId);
            writer.Write(this.name);
        }

        internal virtual void Read(BinaryReader reader, bool readLabel = true)
        {
            if (readLabel)
            {
                this.label = reader.ReadString();
                if (this.label != "OKey")
                    Console.WriteLine("ObjectKey::Read This is not ObjectKey! label = {0}", this.label);
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
            this.name = reader.ReadString();
        }

        #region Extra Helper Methods

        internal ObjectKey(BinaryReader reader)
            : this()
        {
            Read(reader, true);
        }

        internal static ObjectKey FromBytes(byte[] buffer)
        {
            ObjectKey key;
            using (var stream = new MemoryStream(buffer))
                using (var reader = new BinaryReader(stream))
                    key = new ObjectKey(reader);
            return key;
        }

        internal virtual void DumpDetail()
        {
            Console.WriteLine("====Key Details====");
            Console.WriteLine("Name:{0}", this.name);
            Console.WriteLine("Label:{0}", this.label);
            Console.WriteLine("Freed:{0}", this.freed);
            Console.WriteLine("DateTime:{0}", this.dateTime);
            Console.WriteLine("typeid:{0}", this.typeId);
            Console.WriteLine("position:{0}", this.position);
            Console.WriteLine("headerLength:{0}", this.headerLength);
            Console.WriteLine("contentLength:{0}", this.contentLength);
            Console.WriteLine("totalLength:{0}", this.totalLength);
            Console.WriteLine("compression(method:{0} level:{1})", this.compressionMethod, this.compressionLevel);
            Console.WriteLine("===================");
        }

        #endregion
    }
}
