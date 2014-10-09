// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class OnFrameworkCleared : Event
    {
        internal Framework Framework { get; private set; }

        public override byte TypeId
        {
            get
            {
                return EventType.OnFrameworkCleared;
            }
        }

        public OnFrameworkCleared(Framework framework)
        {
            Framework = framework;
        }
    }
}
