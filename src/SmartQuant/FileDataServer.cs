// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class FileDataServer : DataServer
	{
        public FileDataServer(Framework framework, string fileName, string host = null) : base(framework)
        {
        }

        public override DataSeries AddDataSeries(Instrument instrument, byte type, BarType barType = BarType.Time, long barSize = 60)
        {
            throw new NotImplementedException();
        }
	}
}
