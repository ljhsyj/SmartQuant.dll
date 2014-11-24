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
        private bool stepping;
        private byte stepEvent;
        private volatile bool running;
        private Thread thread;

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
            if (bus == null)
                throw new ArgumentNullException();

            this.framework = framework;
            this.bus = bus;
            BarFactory = new BarFactory(this.framework);
            BarSliceFactory = new BarSliceFactory(this.framework);
            Dispatcher = new EventDispatcher(this.framework);
            EventCount = 0;
            DataEventCount = 0;
            Status = EventManagerStatus.Stopped;
            stepping = false;
            stepEvent = EventType.Bar;
            running = true;

            this.thread = new Thread(new ThreadStart(this.Run));
            this.thread.Name = "Event Manager Thread";
            this.thread.IsBackground = true;
            this.thread.Start();
            while (!thread.IsAlive)
                Thread.Sleep(1);
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

        public void Pause(DateTime dateTime)
        {
            this.framework.Clock.AddReminder((dt, obj) => Pause(), dateTime, null);
        }

        public void Pause()
        {
            if (Status == EventManagerStatus.Paused)
                return;
            Console.WriteLine("{0} Event manager paused at ", DateTime.Now, this.framework.Clock.DateTime);
            Status = EventManagerStatus.Paused;
            OnEvent(new OnEventManagerPaused());
        }

        // very much like Start() method
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
            this.stepping = true;
            this.stepEvent = typeId;
            OnEvent(new OnEventManagerStep());
        }

        public void OnEvent(Event e)
        {
            if (Status == EventManagerStatus.Paused && this.stepping && (this.stepEvent == EventType.Event || this.stepEvent == e.TypeId))
            {
                Console.WriteLine(string.Format("{0} Event: {1}", DateTime.Now, e));
                this.stepping = false;
            }
            ++EventCount;

         //   throw new NotImplementedException();
        }

        public void Clear()
        {
            EventCount = DataEventCount = 0;
            BarFactory.Clear();
            BarSliceFactory.Clear();
        }

        // Called before it is disposed.
        internal void Close()
        {
            running = false;
            this.thread.Join();
        }

        private void Run()
        {
            Status = EventManagerStatus.Running;
            while (running)
            {
                if (Status == EventManagerStatus.Running || (Status == EventManagerStatus.Paused && stepping))
                    OnEvent(this.bus.Dequeue());
                else 
                    Thread.Sleep(1);
            }
        }
    }
}
