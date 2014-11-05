// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;

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
            this.Keys = new Dictionary<string, ObjectKey>();
            this.CompressionLevel = 1;
            this.CompressionMethod = 1;
        }

        ~DataFile()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        public virtual object Get(string name)
        {
            throw new NotImplementedException();
        }

        public virtual void Write(string name, object obj)
        {
        }

        public virtual void Delete(string name)
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
            
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;
        }
    }
}

