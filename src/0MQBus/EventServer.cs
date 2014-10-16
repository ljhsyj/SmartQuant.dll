// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class EventServer
    {
        private Framework framework;
        private EventBus bus;

        public EventServer(Framework framework, EventBus bus)
        {
            this.framework = framework;
            this.bus = bus;
        }

        public void OnFrameworkCleared(Framework framework)
        {
            throw new NotImplementedException();
        }

        public void OnLog(Event e)
        {
            throw new NotImplementedException();
        }

        public void OnEvent(Event e)
        {
            throw new NotImplementedException();
        }

        public void OnData(DataObject data)
        {
            throw new NotImplementedException();
        }

        public void EmitQueued()
        {
            throw new NotImplementedException();

        }
    }
}
