using System;
using System.IO;
using System.Collections.Generic;

namespace SmartQuant
{
    public partial class DataFile
    {
        private string name;
        private StreamerManager streamerManager;
        private Stream stream;

        public byte CompressionMethod { set; get; }

        public byte CompressionLevel { set; get; }

        public Dictionary<string, ObjectKey> Keys { get; private set; }

        public DataFile(string name, StreamerManager streamerManager)
        {
            this.name = name;
            this.streamerManager = streamerManager;
            Keys = new Dictionary<string, ObjectKey>();
            CompressionLevel = 1;
            CompressionMethod = 1;
        }

        ~DataFile()
        {
            Dispose(false);
        }

        public virtual void Open(FileMode mode = FileMode.OpenOrCreate)
        {
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

        public virtual void Close()
        {
        }

        public object Get(string name)
        {
            throw new NotImplementedException();
        }

        public void Write(string name, object obj)
        {
        }

        public void Delete(string name)
        {
        }

        protected void ReadKeys()
        {
        }

        protected void ReadFree()
        {
        }

        protected internal virtual void ReadBuffer(byte[] buffer, long position, int length)
        {
        }

        protected internal virtual void WriteBuffer(byte[] buffer, long position, int length)
        {
        }

        public virtual void Flush()
        {
        }

        public void Dump()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
        }
    }
}

