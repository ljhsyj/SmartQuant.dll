﻿// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class EventLogger
    {
        protected internal Framework framework;

        public string Name { get; private set; }

        public EventLogger(Framework framework, string name)
        {
            this.framework = framework;
            Name = name;
        }

        public virtual void OnEvent(Event e)
        {
        }
    }
}
