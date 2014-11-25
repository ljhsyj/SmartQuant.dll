// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class VolumeBarFactoryItem : BarFactoryItem
    {
        public VolumeBarFactoryItem(Instrument instrument, long barSize, BarInput barInput = BarInput.Trade)
            : base(instrument, BarType.Volume, barSize, barInput)
        {
        }

        protected internal override void OnData(DataObject obj)
        {
            base.OnData(obj);
            if (this.bar.Volume >= this.barSize)
                EmitBar();
        }
    }
}
