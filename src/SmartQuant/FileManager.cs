// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;

namespace SmartQuant
{
    public class FileManager
    {
        private string path;
        private Dictionary<string, FileStream> fsCache = new Dictionary<string, FileStream>();

        public FileManager(string path)
        {
            this.path = path;
        }

        public FileStream GetFile(string name, FileMode mode = FileMode.OpenOrCreate)
        {
            lock (this)
            {
                FileStream fs;
                this.fsCache.TryGetValue(name, out fs);
                if (fs == null)
                {
                    fs = new FileStream(Path.Combine(this.path, name), mode);
                    this.fsCache.Add(name, fs);
                }
                return fs;
            }
        }

        public void Close()
        {
            foreach (Stream stream in this.fsCache.Values)
                stream.Close();
        }
    }
}
