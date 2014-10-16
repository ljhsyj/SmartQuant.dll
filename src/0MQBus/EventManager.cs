// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;

namespace SmartQuant
{
    public class EventManager
    {
        private Framework framework;
        private EventBus bus;
        private bool stepping = false;

        public EventManagerStatus Status { get; private set; }

        public EventFilter Filter { get; set; }

        public long EventCount { get; private set; }

        public long DataEventCount { get; private set; }

        public EventDispatcher Dispatcher { get; set; }

        public EventManager(Framework framework, EventBus bus)
        {
            this.framework = framework;
            this.bus = bus;
            Thread thread  = new Thread(new ThreadStart(this.Run));
            thread.Name = "Event Manager Thread";
            thread.IsBackground = true;
            thread.Start();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Pause(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Step(byte typeId = 0)
        {
            throw new NotImplementedException();
        }
            
        public void OnEvent(Event e)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
        }

        private void Run()
        {
            this.Status = EventManagerStatus.Running;
            while (true)
            {
                if (this.Status != EventManagerStatus.Running && (this.Status != EventManagerStatus.Paused || !this.stepping))
                    Thread.Sleep(1);
                else
                    this.OnEvent(this.bus.Dequeue());
            }
        }
    }
}
