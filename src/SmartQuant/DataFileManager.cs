// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.IO;
using System;

namespace SmartQuant
{
	public class DataFileManager
	{
        private string path;

        public DataFileManager(string path)
        {
            this.path = path;
        }

        public DataFile GetFile(string name, FileMode mode = FileMode.OpenOrCreate)
        {
            throw new NotImplementedException();
        }

        public DataSeries GetSeries(string fileName, string seriesName)
        {
            throw new NotImplementedException();
        }

        public void Delete(string fileName, string objectName)
        {
            throw new NotImplementedException();
        }

        public void Close(string name)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
	}

}
