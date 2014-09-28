// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.IO;
using System;
using System.Collections.Generic;

namespace SmartQuant
{
	public class DataFileManager
	{
        private string path;
        private Dictionary<string, DataFile> dataFiles;

        public DataFileManager(string path)
        {
            this.path = path;
            dataFiles = new Dictionary<string, DataFile>();
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
            this.GetFile(fileName, FileMode.OpenOrCreate).Delete(objectName);
        }

        public void Close(string name)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            foreach (DataFile file in this.dataFiles.Values)
                file.Close();
        }
	}
}
