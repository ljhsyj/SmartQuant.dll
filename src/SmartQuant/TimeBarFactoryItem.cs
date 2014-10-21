// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class TimeBarFactoryItem : BarFactoryItem
    {
        private ClockType type;

        public TimeBarFactoryItem(Instrument instrument, long barSize, BarInput barInput = BarInput.Trade, ClockType type = ClockType.Local)
            : base(instrument, BarType.Time, barSize, barInput)
        {
            this.type = type;
        }

        public TimeBarFactoryItem(Instrument instrument, long barSize, ClockType type = ClockType.Local)
            : base(instrument, BarType.Time, barSize)
        {
            this.type = type;
        }

        protected internal override void OnData(DataObject obj)
        {
            throw new NotImplementedException();
        }

        protected override DateTime GetBarOpenDateTime(DataObject obj)
        {
            throw new NotImplementedException();
        }

        protected override DateTime GetBarCloseDateTime(DataObject obj)
        {
            return this.GetBarOpenDateTime(obj).AddSeconds(this.barSize);
        }

        protected internal override void OnReminder(DateTime datetime)
        {
        }
    }
}

