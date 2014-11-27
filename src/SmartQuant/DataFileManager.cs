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
        private StreamerManager streamerManager;

        public DataFileManager(string path)
        {
            this.path = path;
            this.dataFiles = new Dictionary<string, DataFile>();
            this.streamerManager = new StreamerManager();
            this.streamerManager.Add(new DataObjectStreamer());
            this.streamerManager.Add(new BidStreamer());
            this.streamerManager.Add(new AskStreamer());
            this.streamerManager.Add(new QuoteStreamer());
            this.streamerManager.Add(new TradeStreamer());
            this.streamerManager.Add(new BarStreamer());
            this.streamerManager.Add(new Level2SnapshotStreamer());
            this.streamerManager.Add(new Level2UpdateStreamer());
            this.streamerManager.Add(new NewsStreamer());
            this.streamerManager.Add(new FundamentalStreamer());
            this.streamerManager.Add(new DataSeriesStreamer());
            this.streamerManager.Add(new ObjectTableStreamer());
            this.streamerManager.Add(new InstrumentStreamer());
            this.streamerManager.Add(new AltIdStreamer());
            this.streamerManager.Add(new LegStreamer());
            this.streamerManager.Add(new ExecutionCommandStreamer());
            this.streamerManager.Add(new ExecutionReportStreamer());
            this.streamerManager.Add(new PositionStreamer());
            this.streamerManager.Add(new PortfolioStreamer());
            this.streamerManager.Add(new DoubleStreamer());
            this.streamerManager.Add(new Int16Streamer());
            this.streamerManager.Add(new Int32Streamer());
            this.streamerManager.Add(new Int64Streamer());
            this.streamerManager.Add(new StringStreamer());
            this.streamerManager.Add(new TimeSeriesItemStreamer());
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
            GetFile(fileName, FileMode.OpenOrCreate).Delete(objectName);
        }

        public void Close(string name)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            foreach (var file in this.dataFiles.Values)
                file.Close();
        }
	}
}
