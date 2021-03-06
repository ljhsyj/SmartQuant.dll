﻿// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class OnQueueOpened : Event
    {
        internal EventQueue Queue { get; private set; }

        public override byte TypeId
        {
            get
            {
                return EventType.OnQueueOpened;
            }
        }

        public OnQueueOpened(EventQueue queue)
        {
            DateTime = DateTime.MinValue;
            Queue = queue;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", GetType().Name, Queue.Name);
        }
    }
}
