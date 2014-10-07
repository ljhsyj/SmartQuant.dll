// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;
using System.Collections.Generic;

namespace SmartQuant
{
    public class DataManager
    {
        private Framework framework;

        public DataServer Server { get; private set; }

        public DataManager(Framework framework, DataServer dataServer)
        {
            this.framework = framework;
            this.Server = dataServer;
            this.Server.Open();
            Thread thread = new Thread(new ThreadStart(this.Run));
            thread.Name = "Data Manager Thread";
            thread.IsBackground = true;
            thread.Start();
        }

        private void Run()
        {
        }

        public void Dump()
        {
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public DataSeries AddDataSeries(Instrument instrument, byte type)
        {
            return this.Server.AddDataSeries(instrument, type);
        }

        public DataSeries AddDataSeries(string name)
        {
            return this.Server.AddDataSeries(name);
        }

        public DataSeries GetDataSeries(string symbol, byte type, BarType barType = BarType.Time, long barSize = 60)
        {
            return null;
            //  return this.Server.GetDataSeries(this.framework.InstrumentManager.Get(symbol), type, barType, barSize);
        }

        public DataSeries GetDataSeries(Instrument instrument, byte type, BarType barType = BarType.Time, long barSize = 60)
        {
            return null;
            //     return this.Server.GetDataSeries(instrument, type, barType, barSize);
        }

        public DataSeries GetDataSeries(string name)
        {
            return this.Server.GetDataSeries(name);
        }

        public List<DataSeries> GetDataSeriesList(Instrument instrument = null, string pattern = null)
        {
            return this.Server.GetDataSeriesList(instrument, pattern);
        }

        public void DeleteDataSeries(Instrument instrument, byte type, BarType barType = BarType.Time, long barSize = 60)
        {
//            return null;
//            this.Server.DeleteDataSeries(instrument, type, barType, barSize);
        }

        public void DeleteDataSeries(string symbol, byte type, BarType barType = BarType.Time, long barSize = 60)
        {
//            return null;
//            this.Server.DeleteDataSeries(this.framework.InstrumentManager.Get(symbol), type, barType, barSize);
        }

        public void DeleteDataSeries(string name)
        {
            this.Server.DeleteDataSeries(name);
        }

        public void Save(Tick tick, SaveMode option = SaveMode.Add)
        {
            this.Save(tick.InstrumentId, tick, option);
        }

        public void Save(Bar bar, SaveMode option = SaveMode.Add)
        {
            this.Save(bar.InstrumentId, bar, option);
        }

        public void Save(Level2 level2, SaveMode option = SaveMode.Add)
        {
            this.Save(level2.InstrumentId, (DataObject) level2, option);
        }

        public void Save(Level2Snapshot level2, SaveMode option = SaveMode.Add)
        {
            this.Save(level2.InstrumentId, (DataObject) level2, option);
        }

        public void Save(Level2Update level2, SaveMode option = SaveMode.Add)
        {
            this.Save(level2.InstrumentId, (DataObject) level2, option);
        }

        public void Save(Fundamental fundamental, SaveMode option = SaveMode.Add)
        {
            this.Save(fundamental.InstrumentId, fundamental, option);
        }

        public void Save(News news, SaveMode option = SaveMode.Add)
        {
            this.Save(news.InstrumentId, news, option);
        }

        public void Save(Instrument instrument, DataObject obj, SaveMode option = SaveMode.Add)
        {
//            return null;
//            this.Server.Save(instrument, obj, option);
        }

        public void Save(int instrumentId, DataObject obj, SaveMode option = SaveMode.Add)
        {
        }

        public void Save(string symbol, DataObject obj, SaveMode option = SaveMode.Add)
        {
        }

        public void Save(Instrument instrument, IDataSeries series, SaveMode option = SaveMode.Add)
        {
        }

        public Bid GetBid(Instrument instrument)
        {
            return null;

        }

        public Bid GetBid(int instrumentId)
        {
            throw new NotImplementedException();
        }

        public Ask GetAsk(Instrument instrument)
        {   
            throw new NotImplementedException();

        }

        public Ask GetAsk(int instrumentId)
        {
            throw new NotImplementedException();

        }

        public Trade GetTrade(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public Trade GetTrade(int instrumentId)
        {
            return null;

//            return this.idArray_2[instrumentId];
        }

        public Bar GetBar(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public Bar GetBar(int instrumentId)
        {
            throw new NotImplementedException();
        }

        public OrderBook GetOrderBook(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public OrderBook GetOrderBook(int instrumentId)
        {
            throw new NotImplementedException();
        }

        public TickSeries GetHistoricalTicks(TickType type, string symbol, DateTime dateTime1, DateTime dateTime2)
        {
            return null;
         
//            Instrument instrument = this.framework_0.instrumentManager_0.Instruments[symbol];
//            return this.GetHistoricalTicks(type, instrument, dateTime1, dateTime2);
        }

        public TickSeries GetHistoricalBids(string symbol, DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetHistoricalTicks(TickType.Bid, symbol, dateTime1, dateTime2);
        }

        public TickSeries GetHistoricalAsks(string symbol, DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetHistoricalTicks(TickType.Ask, symbol, dateTime1, dateTime2);
        }

        public TickSeries GetHistoricalTrades(string symbol, DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetHistoricalTicks(TickType.Trade, symbol, dateTime1, dateTime2);
        }

        public TickSeries GetHistoricalTicks(TickType type, Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
            throw new NotImplementedException();
        }

        public TickSeries GetHistoricalBids(Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetHistoricalTicks(TickType.Bid, instrument, dateTime1, dateTime2);
        }

        public TickSeries GetHistoricalAsks(Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetHistoricalTicks(TickType.Ask, instrument, dateTime1, dateTime2);
        }

        public TickSeries GetHistoricalTrades(Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetHistoricalTicks(TickType.Trade, instrument, dateTime1, dateTime2);
        }

        public TickSeries GetHistoricalTrades(string provider, Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
//            IHistoricalDataProvider historicalDataProvider = this.framework_0.providerManager_0.GetHistoricalDataProvider(provider);
//            if (historicalDataProvider != null)
//                return this.GetHistoricalTrades(historicalDataProvider, instrument, dateTime1, dateTime2);
//            Console.WriteLine("DataManager::GetHistoricalTrades Error. Provider does not exist : " + provider);
            return (TickSeries)null;
        }

        public TickSeries GetHistoricalTrades(string provider, string symbol, DateTime dateTime1, DateTime dateTime2)
        {
            throw new NotImplementedException();
        }

        public TickSeries GetHistoricalBids(string provider, string symbol, DateTime dateTime1, DateTime dateTime2)
        {
            throw new NotImplementedException();
        }

        public TickSeries GetHistoricalAsks(string provider, string symbol, DateTime dateTime1, DateTime dateTime2)
        {
            throw new NotImplementedException();
        }

        public TickSeries GetHistoricalTicks(IHistoricalDataProvider provider, TickType type, Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
            throw new NotImplementedException();
        }

        public TickSeries GetHistoricalTrades(IHistoricalDataProvider provider, Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetHistoricalTicks(provider, TickType.Trade, instrument, dateTime1, dateTime2);
        }

        public TickSeries GetHistoricalBids(IHistoricalDataProvider provider, Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetHistoricalTicks(provider, TickType.Bid, instrument, dateTime1, dateTime2);
        }

        public TickSeries GetHistoricalAsks(IHistoricalDataProvider provider, Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
            return this.GetHistoricalTicks(provider, TickType.Ask, instrument, dateTime1, dateTime2);
        }

        public BarSeries GetHistoricalBars(string provider, Instrument instrument, DateTime dateTime1, DateTime dateTime2, BarType barType, long barSize)
        {
            return null;

        }

        public BarSeries GetHistoricalBars(string provider, string symbol, DateTime dateTime1, DateTime dateTime2, BarType barType, long barSize)
        {
            return null;

        }

        public BarSeries GetHistoricalBars(IHistoricalDataProvider provider, Instrument instrument, DateTime dateTime1, DateTime dateTime2, BarType barType, long barSize)
        {
            return null;
        }
    }
}
