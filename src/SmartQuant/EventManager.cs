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

        public EventLogger Logger { get; set; }

        public BarFactory BarFactory { get; set; }

        public BarSliceFactory BarSliceFactory { get; set; }

        public long EventCount { get; private set; }

        public long DataEventCount { get; private set; }

        public EventDispatcher Dispatcher { get; set; }

        public EventManager(Framework framework, EventBus bus)
        {
            this.framework = framework;
            this.bus = bus;

            Thread thread = new Thread(new ThreadStart(this.Run));
            thread.Name = "Event Manager Thread";
            thread.IsBackground = true;
            thread.Start();
        }

        public void Start()
        {
            if (Status == EventManagerStatus.Running)
                return;
            Console.WriteLine("{0} Event manager started at ", DateTime.Now, this.framework.Clock.DateTime);
            Status = EventManagerStatus.Running;
            OnEvent(new OnEventManagerStarted());
        }

        public void Stop()
        {
            if (Status == EventManagerStatus.Stopped)
                return;
            Console.WriteLine("{0} Event manager stopping at ", DateTime.Now, this.framework.Clock.DateTime);
            Status = EventManagerStatus.Stopping;
            if (this.framework.Mode == FrameworkMode.Simulation)
                OnEvent(new OnSimulatorStop());
            Status = EventManagerStatus.Stopped;
            this.framework.EventBus.Clear();
            OnEvent(new OnEventManagerStopped());
            Console.WriteLine("{0} Event manager stopped at ", DateTime.Now, this.framework.Clock.DateTime);
        }

        public void Pause()
        {
            if (Status == EventManagerStatus.Paused)
                return;
            Console.WriteLine("{0} Event manager paused at ", DateTime.Now, this.framework.Clock.DateTime);
            Status = EventManagerStatus.Paused;
            OnEvent(new OnEventManagerPaused());
        }

        public void Pause(DateTime dateTime)
        {
            this.framework.Clock.AddReminder((dt, obj) => Pause(), dateTime, null);
        }

        public void Resume()
        {
            if (Status == EventManagerStatus.Running)
                return;
            Console.WriteLine("{0} Event manager resumed at ", DateTime.Now, this.framework.Clock.DateTime);
            Status = EventManagerStatus.Running;
            OnEvent(new OnEventManagerResumed());
        }

        public void Step(byte typeId = 0)
        {
            OnEvent(new OnEventManagerStep());
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
            Status = EventManagerStatus.Running;
            while (true)
            {
                if (Status != EventManagerStatus.Running && (this.Status != EventManagerStatus.Paused || !this.stepping))
                    Thread.Sleep(1);
                else
                    this.OnEvent(this.bus.Dequeue());
            }
        }
    }
}
