using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace SmartQuant
{
    // This class is only for documentation and size calculation
//    [StructLayout(LayoutKind.Explicit)]
//    class FileHead
//    {
//        [FieldOffset(0)]
//        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
//        public string Marker;
//
//        [FieldOffset(11)]
//        public byte Version;
//
//        [FieldOffset(12)]
//        public long HeaderLength;
//
//        [FieldOffset(20)]
//        public long NewKeyPosition;
//
//        [FieldOffset(28)]
//        public long OKeysKeyPosition;
//
//        [FieldOffset(36)]
//        public long FKeysKeyPosition;
//
//        [FieldOffset(44)]
//        public int OKeyLength;
//
//        [FieldOffset(48)]
//        public int FKeyLength;
//
//        [FieldOffset(52)]
//        public int OKeyCount;
//
//        [FieldOffset(56)]
//        public int FKeyCount;
//
//        [FieldOffset(60)]
//        public byte CompressionMethod;
//
//        [FieldOffset(61)]
//        public byte CompressionLevel;
//    }

    // This class is for size calculation only.
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    class FileHeader
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string Marker;
        public byte Version;
        public long HeaderLength;
        public long NewKeyPosition;
        public long OKeysKeyPosition;
        public long FKeysKeyPosition;
        public int OKeyLength;
        public int FKeyLength;
        public int OKeyCount;
        public int FKeyCount;
        public byte CompressionMethod;
        public byte CompressionLevel;

        public FileHeader()
        {
            Marker = "SmartQuant";
            Version = 1;
            HeaderLength = NewKeyPosition = OKeysKeyPosition = FKeysKeyPosition = 62;//Marshal.SizeOf(typeof(FileHeader));
            OKeyLength = FKeyLength = OKeyCount = FKeyCount = 0;
            CompressionMethod = 1;
            CompressionLevel = 1;
        }

        public void Read(BinaryReader reader)
        {
        }

        public void Write(BinaryWriter writer)
        {
        }
    }

    public partial class DataFile
    {
        private readonly int HEADER_LENGTH = 62;//Marshal.SizeOf(typeof(FileHead));

        internal bool ReadHeader()
        {
            byte[] buffer = new byte[HEADER_LENGTH];
            ReadBuffer(buffer, 0, HEADER_LENGTH);
            var reader = new BinaryReader(new MemoryStream(buffer));
            this.fileMarker = reader.ReadString();
            if (this.fileMarker != "SmartQuant")
            {
                Console.WriteLine("DataFile::ReadHeader This is not SmartQuant data file!");
                return false;
            }

            this.version = reader.ReadByte();
            this.headerLength = reader.ReadInt64();
            this.newKeyPosition = reader.ReadInt64();
            this.oKeysKeyPosition = reader.ReadInt64();
            this.fKeysKeyPosition = reader.ReadInt64();
            this.oKeysKeyLength = reader.ReadInt32();
            this.fKeysKeyLength = reader.ReadInt32();
            this.oKeysCount = reader.ReadInt32();
            this.fKeysCount = reader.ReadInt32();
            CompressionMethod = reader.ReadByte();
            CompressionLevel = reader.ReadByte();
            return true;
        }

        internal void WriteHeader()
        {

            using (var mstream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(mstream))
                {
                    writer.Write(this.fileMarker);
                    writer.Write(this.version);
                    writer.Write(this.headerLength);
                    writer.Write(this.newKeyPosition);
                    writer.Write(this.oKeysKeyPosition);
                    writer.Write(this.fKeysKeyPosition);
                    writer.Write(this.oKeysKeyLength);
                    writer.Write(this.fKeysKeyLength);
                    writer.Write(this.oKeysCount);
                    writer.Write(this.fKeysCount);
                    writer.Write(CompressionMethod);
                    writer.Write(CompressionLevel);
                    WriteBuffer(mstream.GetBuffer(), 0, (int)mstream.Length);
                }
            }
        }

        private FreeKey GetFreeKeyWithEnoughLength(int length)
        {
            foreach (var freeKey in this.fKeys)
            {
                if (freeKey.length >= length)
                    return freeKey;
            }
            return null;
        }

        internal void DeleteObjectKey(ObjectKey objectKey, bool remove = true)
        {
            objectKey.freed = true;
            //TODO:FIXME
            WriteBuffer(new byte[] { 1 }, objectKey.position + 5, 1);
            if (remove)
            {
                Keys.Remove(objectKey.name);
                --this.oKeysCount;
            }
            this.fKeys.Add(new FreeKey(objectKey));
            this.fKeys.Sort();
            ++this.fKeysCount;
            this.changed = true;
        }

        internal ObjectKey ReadObjectKey(long position, int length)
        {
            var buffer = new byte[length];
            ReadBuffer(buffer, position, length);
            var reader = new BinaryReader(new MemoryStream(buffer));
            var key = new ObjectKey(this, null, null);
            key.Read(reader, true);
            key.position = position;
            //key.Dump();
            return key;
        }

        internal void WriteObjectKey(ObjectKey oKey)
        {
            this.memoryStream.SetLength(0);
            oKey.Write(this.writer);
            if (oKey.position != -1)
            {
                if (this.memoryStream.Length > (long)oKey.totalLength)
                {
                    DeleteObjectKey(oKey, false);
                    oKey.totalLength = (int)this.memoryStream.Length;
                    var freeKey = oKey != this.fKeysKey ? this.GetFreeKeyWithEnoughLength(oKey.headerLength + oKey.contentLength) : GetFreeKeyWithEnoughLength(oKey.headerLength + oKey.contentLength - 17);
                    if (freeKey != null)
                    {
                        oKey.position = freeKey.position;
                        oKey.totalLength = freeKey.length;
                        this.fKeys.Remove(freeKey);
                        --this.fKeysCount;
                        if (oKey == this.fKeysKey)
                        {
                            this.memoryStream.SetLength(0);
                            oKey.WriteObjectData(true);
                        }
                    }
                    else
                    {
                        oKey.position = this.newKeyPosition;
                        this.newKeyPosition += (long)oKey.totalLength;
                    }
                }
            }
            else
            {
                oKey.position = this.newKeyPosition;
                this.newKeyPosition += (long)oKey.totalLength;
            }
            WriteBuffer(this.memoryStream.GetBuffer(), oKey.position, (int)this.memoryStream.Length);
            oKey.changed = false;
            this.changed = true;
        }

        private void ReadOKeys()
        {
            if (this.oKeysKeyLength == 0)
                return;
            this.oKeysKey = ReadObjectKey(this.oKeysKeyPosition, this.oKeysKeyLength);
            Keys = ((ObjectKeyList)this.oKeysKey.GetObject()).keys;
            foreach (var oKey in Keys.Values)
                oKey.Init(this);
        }

        private void ReadFKeys()
        {
            if (this.fKeysKeyLength == 0)
                return;
            this.fKeysKey = ReadObjectKey(this.fKeysKeyPosition, this.fKeysKeyLength);
            this.fKeys = ((FreeKeyList)this.fKeysKey.GetObject()).keys;
            foreach (var fKey in this.fKeys)
                fKey.Init(this);
        }

        private void SaveOKeys()
        {
            if (this.oKeysKey != null)
                DeleteObjectKey(this.oKeysKey, false);
            this.oKeysKey = new ObjectKey(this, "ObjectKeys", new ObjectKeyList(Keys));
            this.oKeysKey.compressionLevel = 0;
            WriteObjectKey(this.oKeysKey);
            this.oKeysKeyPosition = this.oKeysKey.position;
            this.oKeysKeyLength = this.oKeysKey.headerLength + this.oKeysKey.contentLength;
        }

        private void SaveFKeys()
        {
            if (this.fKeysKey != null)
                DeleteObjectKey(this.fKeysKey, false);
            this.fKeysKey = new ObjectKey(this, "FreeKeys", new FreeKeyList(this.fKeys));
            this.fKeysKey.compressionLevel = 0;
            WriteObjectKey(this.fKeysKey);
            this.fKeysKeyPosition = this.fKeysKey.position;
            this.fKeysKeyLength = this.fKeysKey.headerLength + this.fKeysKey.contentLength;
        }

        #region Helper Methods

        internal void DumpHeader()
        {
            Console.Write("Marker:{0}\nVersion:{1}\nHeaderLength:{2}\n", this.fileMarker, this.version, this.headerLength);
            Console.Write("NewKeyPostion:{0}\n", this.newKeyPosition);
            Console.Write("OkeyList(Postion:{0} Length:{1} Count:{2})\n", this.oKeysKeyPosition, this.oKeysKeyLength, this.oKeysCount);
            Console.Write("FkeyList(Postion:{0} Length:{1} Count:{2})\n", this.fKeysKeyPosition, this.fKeysKeyLength, this.fKeysCount);
            Console.Write("Compression(Method:{0} Level:{1})\n", this.CompressionMethod, this.CompressionLevel);
        }
        #endregion
    }
}