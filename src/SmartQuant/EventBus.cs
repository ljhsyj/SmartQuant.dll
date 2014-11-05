// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class EventBus
    {
        private Framework framework;

        public EventBusMode Mode { get; set; }

        public EventPipe DataPipe { get; private set; }

        public EventPipe ExecutionPipe { get; private set; }

        public EventPipe HistoricalPipe  { get; private set; }

        public EventPipe ServicePipe  { get; private set; }

        public EventBus(Framework framework, EventBusMode mode = EventBusMode.Simulation)
        {
            this.framework = framework;
            Mode = mode;
            DataPipe = new EventPipe(framework);
            ExecutionPipe = new EventPipe(framework);
            HistoricalPipe = new EventPipe(framework);
            ServicePipe = new EventPipe(framework);
        }

        public void Attach(EventBus bus)
        {
            throw new NotImplementedException();
        }

        public Event Dequeue()
        {
            throw new NotImplementedException();
        }

        public void ResetCounts()
        {
        }

        public void Clear()
        {
            DataPipe.Clear();
            ExecutionPipe.Clear();
            ExecutionPipe.Clear();
            ServicePipe.Clear();
        }
    }
}
