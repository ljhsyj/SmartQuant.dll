// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class Event
    {
        protected internal DateTime dateTime;

        public DateTime DateTime
        {
            get
            {
                return this.dateTime;
            }
            set
            {
                this.dateTime = value;
            }
        }

        public virtual byte TypeId
        {
            get
            {
                return EventType.Event;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", DateTime, GetType());
        }
    }
}
