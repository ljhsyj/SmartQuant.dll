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
            base.OnData(obj);
            if (this.bar != null || AddReminder(GetBarCloseDateTime(obj), this.type))
                return;
            if (obj is Tick)
                Console.WriteLine("TimeBarFactoryItem::OnData Can not add reminder. Clock = {0} local datetime = {1} exchange dateTime = {2} object = {3} object exchange datetime = {4} reminder datetime = {5}", this.type, this.factory.framework.Clock.DateTime, this.factory.framework.ExchangeClock.DateTime, obj, (obj as Tick).ExchangeDateTime, GetBarCloseDateTime(obj));
            else
                Console.WriteLine("TimeBarFactoryItem::OnData Can not add reminder. Object is not tick! Clock = {0} local datetime = {1} exchange datetime = {2} object = {3} reminder datetime = {4}", this.type, this.factory.framework.Clock.DateTime, this.factory.framework.ExchangeClock.DateTime, obj, GetBarCloseDateTime(obj));
        }

        protected override DateTime GetBarOpenDateTime(DataObject obj)
        {
            var t = GetDataObjectDateTime(obj, this.type);
            long seconds = (long) t.TimeOfDay.TotalSeconds / this.barSize * this.barSize;
            return t.Date.AddSeconds(seconds);
        }

        protected override DateTime GetBarCloseDateTime(DataObject obj)
        {
            return GetBarOpenDateTime(obj).AddSeconds(this.barSize);
        }

        protected internal override void OnReminder(DateTime datetime)
        {
            this.bar.dateTime = this.type == ClockType.Local ? datetime : this.factory.framework.Clock.DateTime;
            EmitBar();
        }
    }
}

