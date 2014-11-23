//
// Author: Alex Lee <lu.lee05@gmail.com>
//

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SmartQuant
{
    // This class is for size calculation only.
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    class FreeKeyHead
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string Label = "FKey";

        [FieldOffset(5)]
        public long Position;

        [FieldOffset(13)]
        public int Length;
    }

    // This class is for size calculation only.
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    class FreeKeyHeader
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string Label = "FKey";
        public long Position;
        public int Length;
    }

    class FreeKey : IComparable<FreeKey>
    {
        internal DataFile dataFile;
        internal string label = "FKey";
        internal long position = -1;
        internal int length = -1;

        public FreeKey()
        {
        }

        public FreeKey(DataFile dataFile, long position = -1, int length = -1)
        {
            this.dataFile = dataFile;
            this.position = position;
            this.length = length;
        }

        public FreeKey(ObjectKey objectKey)
        {
            this.dataFile = objectKey.dataFile;
            this.position = objectKey.position;
            this.length = objectKey.totalLength;
        }

        internal void Init(DataFile dataFile)
        {
        }

        internal void WriteKey(BinaryWriter writer)
        {
            writer.Write(this.label);
            writer.Write(this.position);
            writer.Write(this.length);
        }

        internal void ReadKey(BinaryReader reader, bool readLabel = true)
        {
            if (readLabel)
            {
                this.label = reader.ReadString();
                if (this.label != "FKey")
                    Console.WriteLine("FreeKey::ReadKey This is not FreeKey! label = " + this.label);
            }
            this.position = reader.ReadInt64();
            this.length = reader.ReadInt32();
        }

        public int CompareTo(FreeKey that)
        {
            return that != null ? this.length.CompareTo(that.length) : -1;
        }
    }
}

