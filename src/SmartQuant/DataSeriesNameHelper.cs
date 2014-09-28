// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public static class DataSeriesNameHelper
    {
        public static string GetName(Instrument instrument, byte type)
        {
            throw new NotImplementedException();
        }

        public static string GetName(Instrument instrument, BarType barType, long barSize)
        {
            throw new NotImplementedException();
        }

        public static bool TryGetBarTypeSize(DataSeries series, out BarType barType, out long barSize)
        {
            throw new NotImplementedException();
        }

        public static Instrument GetInstrument(DataSeries series, Framework framework)
        {
            throw new NotImplementedException();
        }

        public static string GetSymbol(DataSeries series)
        {
            return DataSeriesNameHelper.GetSymbol(series.Name);
        }

        public static string GetSymbol(string seriesName)
        {
            throw new NotImplementedException();
        }

        public static byte GetDataType(DataSeries series)
        {
            throw new NotImplementedException();
        }
    }
}

