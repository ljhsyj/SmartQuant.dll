// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;

namespace SmartQuant
{
    public class DataFile
    {
        private Dictionary<string, ObjectKey> keys = new Dictionary<string, ObjectKey>();
        private StreamerManager streamerManager;

        private string name;

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
        }

        public virtual void Open(FileMode mode = FileMode.OpenOrCreate)
        {
        }

        protected virtual bool OpenFileStream(string name, FileMode mode)
        {
            return false;
        }

        protected virtual void CloseFileStream()
        {
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
        }
    }
}

