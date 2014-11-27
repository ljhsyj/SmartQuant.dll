// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class StrategyStatusInfo : DataObject
    {
        public StrategyStatusType Type { get; private set; }

        public string Solution { get; set; }

        public string Mode { get; set; }

        public override byte TypeId
        {
            get
            {
                return DataObjectType.StrategyStatus;
            }
        }

        public StrategyStatusInfo(DateTime dateTime, StrategyStatusType type)
        {
            Type = type;
            DateTime = dateTime;
        }
    }
}