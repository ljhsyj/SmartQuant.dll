// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class OnSimulatorProgress : DataObject
    {
        internal long Count { get; private set; }
        internal int Percent { get; private set; }

        public override byte TypeId
        {
            get
            {
                return DataObjectType.OnSimulatorProgress;
            }
        }

        public OnSimulatorProgress()
        {
            DateTime = DateTime.MinValue;
        }

        public OnSimulatorProgress(long count, int percent) : this()
        {
            Count = count;
            Percent = percent;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
