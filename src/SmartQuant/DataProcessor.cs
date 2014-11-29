// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class DataProcessor
    {
        public bool EmitBar { get; set; }

        public bool EmitBarOpen { get; set; }

        public bool EmitBarOpenTrade { get; set; }

        public bool EmitBarHighTrade { get; set; }

        public bool EmitBarLowTrade { get; set; }

        public bool EmitBarCloseTrade { get; set; }

        private Class25 class25_0;

        public DataProcessor()
        {
            EmitBar = EmitBarOpen = true;
            EmitBarOpenTrade = EmitBarHighTrade = EmitBarLowTrade = EmitBarCloseTrade = false;
        }

        internal DataObject Process(Class25 class25_1)
        {
            this.class25_0 = class25_1;
            return OnData(class25_1.dataObject_0);
        }

        protected virtual DataObject OnData(DataObject obj)
        {
            if (obj.TypeId == DataObjectType.Bar)
            {
                var bar = obj as Bar;
                if (EmitBarOpen)
                    Emit(new Bar(bar.OpenDateTime, bar.OpenDateTime, bar.InstrumentId, bar.Type, bar.Size, bar.Open, 0, 0, 0, 0, 0));
                if (EmitBarOpenTrade)
                    Emit(new Trade(bar.OpenDateTime, (byte)0, bar.InstrumentId, bar.Open, (int)(bar.Volume / 4)));
                if (EmitBarHighTrade && EmitBarLowTrade)
                {
                    if (bar.Close > bar.Open)
                    {
                        Emit(new Trade(new DateTime(bar.OpenDateTime.Ticks + (bar.CloseDateTime.Ticks - bar.OpenDateTime.Ticks) / 3), (byte)0, bar.InstrumentId, bar.Low, (int)(bar.Volume / 4)));
                        Emit(new Trade(new DateTime(bar.OpenDateTime.Ticks + (bar.CloseDateTime.Ticks - bar.OpenDateTime.Ticks) * 2 / 3), (byte)0, bar.InstrumentId, bar.High, (int)(bar.Volume / 4)));
                    }
                    else
                    {
                        Emit(new Trade(new DateTime(bar.OpenDateTime.Ticks + (bar.CloseDateTime.Ticks - bar.OpenDateTime.Ticks) / 3), (byte)0, bar.InstrumentId, bar.High, (int)(bar.Volume / 4)));
                        Emit(new Trade(new DateTime(bar.OpenDateTime.Ticks + (bar.CloseDateTime.Ticks - bar.OpenDateTime.Ticks) * 2 / 3), (byte)0, bar.InstrumentId, bar.Low, (int)(bar.Volume / 4)));
                    }
                }
                else
                {
                    if (EmitBarHighTrade)
                        Emit(new Trade(new DateTime(bar.OpenDateTime.Ticks + (bar.CloseDateTime.Ticks - bar.OpenDateTime.Ticks) / 2), (byte)0, bar.InstrumentId, bar.High, (int)(bar.Volume / 4)));
                    if (EmitBarLowTrade)
                        Emit(new Trade(new DateTime(bar.OpenDateTime.Ticks + (bar.CloseDateTime.Ticks - bar.OpenDateTime.Ticks) / 2), (byte)0, bar.InstrumentId, bar.Low, (int)(bar.Volume / 4)));
                }
                if (EmitBarCloseTrade)
                    Emit(new Trade(bar.CloseDateTime, (byte)0, bar.InstrumentId, bar.Close, (int)(bar.Volume / 4)));
                if (!EmitBar)
                    return null;
            }
            return obj;
        }

        protected void Emit(DataObject obj)
        {
            if (!this.class25_0.eventQueue_1.IsFull())
                this.class25_0.eventQueue_1.Write(obj);
            else
                Console.WriteLine("DataProcessor::Emit Can not write data object. Data queue is full. Data queue size = " + (object)this.class25_0.eventQueue_1.Size);
        }
    }
}

