// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;

namespace SmartQuant
{
    public class EventManager
    {
        private delegate void EventGate(Event e);

        private Framework framework;
        private EventBus bus;
        private bool stepping;
        private byte stepEvent;
        private volatile bool running;
        private Thread thread;
        private IdArray<EventGate> gates;

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
            this.gates = new IdArray<EventGate>(1024);
//            this.gates[EventType.OnProviderConnected] = new EventManager.Delegate0(this.method_2);
//            this.gates[EventType.OnProviderDisconnected] = new EventManager.Delegate0(this.method_3);
//            this.gates[EventType.OnSimulatorStart] = new EventManager.Delegate0(this.method_4);
//            this.gates[EventType.OnSimulatorStop] = new EventManager.Delegate0(this.method_5);
//            this.gates[EventType.OnSimulatorProgress] = new EventManager.Delegate0(this.method_6);
//            this.gates[EventType.Bid] = new EventManager.Delegate0(this.method_7);
//            this.gates[EventType.Ask] = new EventManager.Delegate0(this.method_8);
//            this.gates[EventType.Trade] = new EventManager.Delegate0(this.method_9);
//            this.gates[EventType.Bar] = new EventManager.Delegate0(this.method_10);
//            this.gates[EventType.Level2Snapshot] = new EventManager.Delegate0(this.method_12);
//            this.gates[EventType.Level2Update] = new EventManager.Delegate0(this.method_13);
//            this.gates[EventType.News] = new EventManager.Delegate0(this.method_15);
//            this.gates[EventType.Fundamental] = new EventManager.Delegate0(this.method_14);
//            this.gates[EventType.ExecutionReport] = new EventManager.Delegate0(this.method_16);
//            this.gates[EventType.Reminder] = new EventManager.Delegate0(this.method_32);
//            this.gates[EventType.Group] = new EventManager.Delegate0(this.method_33);
//            this.gates[EventType.GroupEvent] = new EventManager.Delegate0(this.method_34);
//            this.gates[EventType.OnPositionOpened] = new EventManager.Delegate0(this.method_29);
//            this.gates[EventType.OnPositionClosed] = new EventManager.Delegate0(this.method_30);
//            this.gates[EventType.OnPositionChanged] = new EventManager.Delegate0(this.method_31);
//            this.gates[EventType.OnFill] = new EventManager.Delegate0(this.method_27);
//            this.gates[EventType.OnTransaction] = new EventManager.Delegate0(this.method_28);
//            this.gates[EventType.OnSendOrder] = new EventManager.Delegate0(this.method_18);
//            this.gates[EventType.OnPendingNewOrder] = new EventManager.Delegate0(this.method_19);
//            this.gates[EventType.OnNewOrder] = new EventManager.Delegate0(this.method_20);
//            this.gates[EventType.OnOrderStatusChanged] = new EventManager.Delegate0(this.method_21);
//            this.gates[EventType.OnOrderPartiallyFilled] = new EventManager.Delegate0(this.method_22);
//            this.gates[EventType.OnOrderFilled] = new EventManager.Delegate0(this.method_23);
//            this.gates[EventType.OnOrderReplaced] = new EventManager.Delegate0(this.method_24);
//            this.gates[EventType.OnOrderCancelled] = new EventManager.Delegate0(this.method_25);
//            this.gates[EventType.OnOrderDone] = new EventManager.Delegate0(this.method_26);
//            this.gates[EventType.HistoricalData] = new EventManager.Delegate0(this.method_35);
//            this.gates[EventType.HistoricalDataEnd] = new EventManager.Delegate0(this.method_36);
//            this.gates[EventType.BarSlice] = new EventManager.Delegate0(this.method_11);
//            this.gates[EventType.OnStrategyEvent] = new EventManager.Delegate0(this.method_38);
//            this.gates[EventType.AccountData] = new EventManager.Delegate0(this.method_17);
//            this.gates[EventType.OnUserCommand] = new EventManager.Delegate0(this.method_37);

            this.thread = new Thread(new ThreadStart(Run));
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
            this.framework.Clock.AddReminder((datetime, obj) => Pause(), dateTime, null);
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

        public void Step(byte typeId = EventType.Event)
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
            if (Filter != null && Filter.Filter(e) == null)
                return;
            if (this.gates[e.TypeId] != null)
                this.gates[e.TypeId](e);
            if (Dispatcher != null)
                Dispatcher.OnEvent(e);
            if (Logger != null)
                Logger.OnEvent(e);
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
            Console.WriteLine("{0} Event manager thread started: Framework = {1} Clock = {2}", DateTime.Now, this.framework.Name, this.framework.Clock.GetModeAsString());
            Status = EventManagerStatus.Running;
            while (running)
            {
                if (Status != EventManagerStatus.Running && (Status != EventManagerStatus.Paused || !this.stepping))
                    Thread.Sleep(1);
                else
                    OnEvent(this.bus.Dequeue());
            }
            Console.WriteLine("{0} Event manager thread stopped: Framework = {1} Clock = {2}", DateTime.Now, this.framework.Name, this.framework.Clock.GetModeAsString());
        }
    }
}
