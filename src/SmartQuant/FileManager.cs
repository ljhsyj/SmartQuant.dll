// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Net.Sockets;
using System.IO;

namespace SmartQuant
{
	public class FileManager
	{
        public FileManager(string path)
        {
        }

        public FileStream GetFile(string name, FileMode mode = FileMode.OpenOrCreate)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
        }
	}
}

