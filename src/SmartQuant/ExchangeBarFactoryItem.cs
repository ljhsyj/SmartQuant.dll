// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class ExchangeBarFactoryItem : BarFactoryItem
    {

        public ExchangeBarFactoryItem(Instrument instrument, long barSize, BarInput barInput = BarInput.Trade)
            : base(instrument, BarType.Exchange, barSize, barInput)
        {
        }

        protected internal override void OnData(DataObject obj)
        {
            DateTime dateTime = this.GetExchangeDateTime(obj);
            if (this.bar != null && this.bar.OpenDateTime.AddSeconds(this.barSize) <= dateTime)
                this.EmitBar();
            base.OnData(obj);
            this.bar.DateTime = dateTime;
        }

        protected override DateTime GetBarOpenDateTime(DataObject obj)
        {
            return this.GetExchangeDateTime(obj);
        }

        private DateTime GetExchangeDateTime(DataObject obj)
        {
            return ((Tick)obj).ExchangeDateTime;
        }
    }
}
