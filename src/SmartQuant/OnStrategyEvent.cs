// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class OnStrategyEvent : Event
	{
        private object data;

        public override byte TypeId
        {
            get
            {
                return EventType.OnStrategyEvent;
            }
        }

        public OnStrategyEvent(object data)
        {
            this.data = data;
        }
	}
}
