// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
	public class BarFactoryItem
	{
        protected internal BarFactory factory;
        protected internal Instrument instrument;
        protected BarType barType;
        protected internal long barSize;
        protected internal BarInput barInput;
        protected Bar bar;

        protected BarFactoryItem(Instrument instrument, BarType barType, long barSize, BarInput barInput = 0)
        {
        }

        protected internal virtual void OnData(DataObject obj)
        {
            throw new NotImplementedException();
        }

        protected internal virtual void OnReminder(DateTime datetime)
        {
        }

        protected virtual DateTime GetBarOpenDateTime(DataObject obj)
        {
            throw new NotImplementedException();
        }

        protected virtual DateTime GetBarCloseDateTime(DataObject obj)
        {
            throw new NotImplementedException();
        }

        protected void AddReminder(DateTime datetime)
        {
            throw new NotImplementedException();
        }

        protected void EmitBar()
        {
            throw new NotImplementedException();
        }
	}
}
