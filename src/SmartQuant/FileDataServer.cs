// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;

namespace SmartQuant
{
    public class FileDataServer : DataServer
    {
        private DataFile dataFile;
        private string host;
        private bool opened;

        private IdArray<DataSeries>[] idArray_0;
        private IdArray<IdArray<DataSeries>> idArray_1;
        private IdArray<IdArray<Dictionary<long, DataSeries>>> idArray_2;

        public FileDataServer(Framework framework, string fileName, string host = null)
            : base(framework)
        {
            this.host = host;
            if (host == null)
                this.dataFile = new DataFile(fileName, framework.StreamerManager);
            else
                ;
//                this.dataFile =  new NetDataFile_(fileName, host, framework.streamerManager_0);
        }

        public override void Open()
        {
            if (this.opened)
                return;
            this.dataFile.Open(FileMode.OpenOrCreate);
            this.idArray_0 = new IdArray<DataSeries>[128];
            for (int index = 0; index < this.idArray_0.Length; ++index)
                this.idArray_0[index] = new IdArray<DataSeries>(1000);
            this.opened = true;
        }

        public override void Close()
        {
            if (!this.opened)
                return;
            this.dataFile.Close();
            this.opened = false;
        }

        public override void Flush()
        {
            this.dataFile.Flush();
        }

        private DataSeries method_0(Instrument instrument_0, BarType barType_0, long long_0, bool bool_1)
        {
            DataSeries dataSeries;
            if (barType_0 == BarType.Time && long_0 <= 86400L)
            {
                if (this.idArray_1[instrument_0.Id] == null)
                    this.idArray_1[instrument_0.Id] = new IdArray<DataSeries>(1000);
                dataSeries = this.idArray_1[instrument_0.Id][(int)long_0];
            }
            else
            {
                if (this.idArray_2[(int)barType_0] == null)
                    this.idArray_2[(int)barType_0] = new IdArray<Dictionary<long, DataSeries>>(1000);
                if (this.idArray_2[(int)barType_0][instrument_0.Id] == null)
                    this.idArray_2[(int)barType_0][instrument_0.Id] = new Dictionary<long, DataSeries>();
                dataSeries = this.idArray_2[(int)barType_0][instrument_0.Id][long_0];
            }
            if (dataSeries == null)
            {
                string name = DataSeriesNameHelper.GetName(instrument_0, barType_0, long_0);
                dataSeries = (DataSeries)this.dataFile.Get(name);
                if (dataSeries == null & bool_1)
                {
                    dataSeries = this.host != null ? (DataSeries)new NetDataSeries(name) : new DataSeries(name);
                    this.dataFile.Write(name, (object)dataSeries);
                    this.dataFile.Flush();
                }
                if (dataSeries != null)
                {
                    if (barType_0 == BarType.Time && long_0 <= 86400)
                        this.idArray_1[instrument_0.Id][(int)long_0] = dataSeries;
                    else
                        this.idArray_2[(int)barType_0][instrument_0.Id][long_0] = dataSeries;
                }
            }
            return dataSeries;
        }

        private void method_1(Instrument instrument_0, Bar bar_0, SaveMode saveMode_0)
        {
            DataSeries dataSeries = this.method_0(instrument_0, bar_0.Type, bar_0.Size, true);
            switch (saveMode_0)
            {
                case SaveMode.Add:
                    dataSeries.Add(bar_0);
                    break;
                case SaveMode.Append:
                    if (!(bar_0.DateTime > dataSeries.DateTime2))
                        break;
                    dataSeries.Add((DataObject)bar_0);
                    break;
            }
        }

        public override void Save(Instrument instrument, DataObject obj, SaveMode option = SaveMode.Add)
        {
            byte type = TapeMode ? DataObjectType.Tick : obj.TypeId;
            if (type == DataObjectType.Bar)
            {
                this.method_1(instrument, (Bar) obj, option);
            }
            else
            {
                DataSeries dataSeries = this.idArray_0[(int) type][instrument.Id];
                if (dataSeries == null)
                {
                    string name = DataSeriesNameHelper.GetName(instrument, type);
                    dataSeries = (DataSeries) this.dataFile.Get(name);
                    if (dataSeries == null)
                    {
                        dataSeries = this.host != null ? (DataSeries) new NetDataSeries(name) : new DataSeries(name);
                        this.dataFile.Write(name, (object) dataSeries);
                        this.dataFile.Flush();
                    }
                    this.idArray_0[(int) type][instrument.Id] = dataSeries;
                }
                switch (option)
                {
                    case SaveMode.Add:
                        dataSeries.Add(obj);
                        break;
                    case SaveMode.Append:
                        if (!(obj.DateTime > dataSeries.DateTime2))
                            break;
                        dataSeries.Add(obj);
                        break;
                }
            }
        }

        public override DataSeries GetDataSeries(Instrument instrument, byte type, BarType barType = BarType.Time, long barSize = 60)
        {
            if (type == DataObjectType.Bar)
                return this.method_0(instrument, barType, barSize, false);
            DataSeries dataSeries = this.idArray_0[type][instrument.Id];
            if (dataSeries == null)
            {
                dataSeries = this.dataFile.Get(DataSeriesNameHelper.GetName(instrument, type)) as DataSeries;
                this.idArray_0[type][instrument.Id] = dataSeries;
            }
            return dataSeries;
        }

        public override List<DataSeries> GetDataSeriesList(Instrument instrument = null, string pattern = null)
        {
            var list = new List<DataSeries>();
            foreach (var objectKey in this.dataFile.Keys.Values)
            {
                if (objectKey.TypeId == ObjectType.DataSeries && (instrument == null || !(DataSeriesNameHelper.GetSymbol(objectKey.name) != instrument.Symbol)) && (pattern == null || objectKey.name.Contains(pattern)))
                    list.Add(this.dataFile.Get(objectKey.name) as DataSeries);
            }
            return list;
        }

        public override DataSeries GetDataSeries(string name)
        {
            return this.dataFile.Get(name) as DataSeries;
        }

        public override void DeleteDataSeries(Instrument instrument, byte type, BarType barType = BarType.Time, long barSize = 60)
        {
            if (type == DataObjectType.Bar)
            {
                DataSeries dataSeries = this.method_0(instrument, barType, barSize, false);
                if (barType == BarType.Time && barSize <= 86400)
                {
                    dataSeries = this.idArray_1[instrument.Id][(int)barSize] = null;
                }
                else
                {
                    if (this.idArray_2[type] == null)
                        this.idArray_2[type] = new IdArray<Dictionary<long, DataSeries>>(1000);
                    if (this.idArray_2[type][instrument.Id] == null)
                        this.idArray_2[type][instrument.Id] = new Dictionary<long, DataSeries>();
                    this.idArray_2[type][instrument.Id].Remove(barSize);
                }
                this.dataFile.Delete(DataSeriesNameHelper.GetName(instrument, barType, barSize));
            }
            else
            {
                if (this.idArray_0[type][instrument.Id] != null)
                    this.idArray_0[type].Remove(instrument.Id);
                this.dataFile.Delete(DataSeriesNameHelper.GetName(instrument, type));
            }
        }

        public override void DeleteDataSeries(string name)
        {
            var dataSeries = this.dataFile.Get(name) as DataSeries;
            if (dataSeries == null)
                return;
            for (int i = 0; i < this.idArray_0.Length; ++i)
            {
                for (int id = 0; id < this.idArray_0[i].Size; ++id)
                {
                    if (this.idArray_0[i][id] == dataSeries)
                        this.idArray_0[i].Remove(id);
                }
            }
            this.dataFile.Delete(name);
        }

        public override DataSeries AddDataSeries(Instrument instrument, byte type, BarType barType = BarType.Time, long barSize = 60L)
        {
            if (type == DataObjectType.Bar)
                return this.method_0(instrument, barType, barSize, true);
            DataSeries dataSeries = this.idArray_0[type][instrument.Id];
            if (dataSeries == null)
            {
                string name = DataSeriesNameHelper.GetName(instrument, type);
                dataSeries = this.dataFile.Get(name) as DataSeries;
                if (dataSeries == null)
                {
                    dataSeries = this.host != null ? new NetDataSeries(name) : new DataSeries(name);
                    this.dataFile.Write(name, dataSeries);
                }
                this.idArray_0[type][instrument.Id] = dataSeries;
            }
            return dataSeries;
        }

        public override DataSeries AddDataSeries(string name)
        {
            var dataSeries = this.dataFile.Get(name) as DataSeries;
            if (dataSeries == null)
            {
                dataSeries = this.host != null ? new NetDataSeries(name) : new DataSeries(name);
                this.dataFile.Write(name, dataSeries);
            }
            return dataSeries;
        }
    }
}
