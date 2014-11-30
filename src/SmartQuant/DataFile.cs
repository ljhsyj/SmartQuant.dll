using System;
using System.Collections.Generic;
using System.IO;

namespace SmartQuant
{
    public partial class DataFile
    {
        internal string fileMarker = "SmartQuant";
        internal byte version = 1;
        internal long headerLength;
        internal long newKeyPosition;
        internal long oKeysKeyPosition;
        internal long fKeysKeyPosition;
        internal int oKeysKeyLength;
        internal int fKeysKeyLength;
        internal int oKeysCount;
        internal int fKeysCount;

        internal string name;
        internal Stream stream;
        internal FileMode mode;
        internal bool opened;
        internal bool changed;

        internal List<FreeKey> fKeys = new List<FreeKey>();
        internal ObjectKey oKeysKey;
        internal ObjectKey fKeysKey;
        internal StreamerManager streamerManager;
        private bool disposed;
        private MemoryStream memoryStream;
        private BinaryWriter writer;

        public Dictionary<string, ObjectKey> Keys { get; private set; }

        public byte CompressionMethod { get; set; }

        public byte CompressionLevel { get; set; }

        public DataFile(string name, StreamerManager streamerManager)
        {
            CompressionLevel = 1;
            CompressionMethod = 1;
            this.name = name;
            this.streamerManager = streamerManager;
            Keys = new Dictionary<string, ObjectKey>();
            this.memoryStream = new MemoryStream();
            this.writer = new BinaryWriter(this.memoryStream);
        }

        ~DataFile()
        {
            Dispose(false);
        }

        protected virtual bool OpenFileStream(string name, FileMode mode)
        {
            this.stream = new FileStream(name, mode);
            return this.stream.Length != 0;
        }

        protected virtual void CloseFileStream()
        {
            this.stream.Close();
        }

        public virtual void Open(FileMode mode = FileMode.OpenOrCreate)
        {
            if (mode != FileMode.OpenOrCreate && mode != FileMode.Create)
            {
                Console.WriteLine("DataFile::Open Can not open file in {0} mode. DataFile suppports FileMode.OpenOrCreate and FileMode.Create modes.", mode);
                return;
            }

            if (this.opened)
            {
                Console.WriteLine("DataFile::Open File is already open: {0}", this.name);
                return;
            }

            this.mode = mode;
            if (!OpenFileStream(this.name, mode))
            {
                // File is empty
                this.headerLength = HEADER_LENGTH;
                this.newKeyPosition = HEADER_LENGTH;
                this.oKeysKeyPosition = HEADER_LENGTH;
                this.fKeysKeyPosition = HEADER_LENGTH;
                this.oKeysKeyLength = 0;
                this.fKeysKeyLength = 0;
                this.oKeysCount = 0;
                this.fKeysCount = 0;
                this.changed = true;
                WriteHeader();

                this.opened = true;
                return;
            }

            if (!ReadHeader())
            {
                Console.WriteLine("DataFile::Open Error opening file {0}", this.name);
                return;
            }

            ReadKeys();
            ReadFree();
            this.opened = true;
        }

        protected internal virtual void ReadBuffer(byte[] buffer, long position, int length)
        {
            lock (this)
            {
                this.stream.Seek(position, SeekOrigin.Begin);
                this.stream.Read(buffer, 0, length);
            }
        }

        protected internal virtual void WriteBuffer(byte[] buffer, long position, int length)
        {
            lock (this)
            {
                this.stream.Seek(position, SeekOrigin.Begin);
                this.stream.Write(buffer, 0, length);
            }
        }

        public virtual void Write(string name, object obj)
        {
            ObjectKey oKey;
            Keys.TryGetValue(name, out oKey);
            if (oKey != null)
            {
                oKey.obj = obj;
                oKey.Init(this);
            }
            else
            {
                oKey = new ObjectKey(this, name, obj);
                Keys.Add(name, oKey);
                ++this.oKeysCount;
            }
            oKey.dateTime = DateTime.Now;

            // For the instance of each ObjectType, some extra work might be needed.
            if (oKey.TypeId == ObjectType.DataSeries)
                ((DataSeries)obj).InitDataKeys(this, oKey);

            // Finally, write it to disk.
            WriteObjectKey(oKey);
        }

        public virtual object Get(string name)
        {
            ObjectKey key;
            Keys.TryGetValue(name, out key);
            return key != null ? key.GetObject() : null;
        }

        public virtual void Delete(string name)
        {
            ObjectKey key;
            Keys.TryGetValue(name, out key);
            if (key != null)
                DeleteObjectKey(key, true);
        }

        protected void ReadKeys()
        {
            ReadOKeys();
        }

        protected void ReadFree()
        {
            ReadFKeys();
        }

        public void Dump()
        {
            if (!this.opened)
            {
                Console.WriteLine("ObjectFile {0} is closed", this.name);
                return;
            }

            Console.WriteLine(string.Format("DataFile {0} in {1} mode and contains {2} objects:", this.name, this.mode, this.Keys.Values.Count));
            foreach (var oKey in Keys.Values)
                oKey.Dump();
            Console.WriteLine("Free objects = {0}",this.fKeysCount);
            foreach (var fKey in this.fKeys)
                Console.WriteLine("{0} {1}", fKey.position, fKey.length);
        }

        public virtual void Flush()
        {
            if (this.changed)
            {
                foreach (var oKey in Keys.Values)
                {
                    if (oKey.TypeId == ObjectType.DataSeries && oKey.obj != null)
                    {
                        var ds = (DataSeries)oKey.obj;
                        if (ds.changed)
                            ds.Flush();
                    }
                }
                SaveOKeys();
                SaveFKeys();
                WriteHeader();
            }
            this.changed = false;
        }

        public virtual void Refresh()
        {
        }

        public virtual void Close()
        {
            if (!this.opened)
            {
                Console.WriteLine("DataFile::Close File is not open: {0}" + this.name);
                return;
            }

            Flush();
            CloseFileStream();
            this.opened = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
                Close();
            this.disposed = true;
        }
    }
}
