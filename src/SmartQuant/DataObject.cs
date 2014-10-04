// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class DataObject : Event
    {
        public override byte TypeId
        {
            get
            {
                return DataObjectType.DataObject;
            }
        }

        public DataObject()
        {
        }

        public DataObject(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DataObject(DataObject obj)
        {
            DateTime = obj.DateTime;
        }
    }
}