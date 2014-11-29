// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;
using System.Diagnostics;

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
            this.gates[EventType.OnSimulatorStart] = this.method_4;
            this.gates[EventType.OnSimulatorStop] = this.method_5;
            this.gates[EventType.OnSimulatorProgress] = this.method_6;
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
                {
                    var e = this.bus.Dequeue();
                   // Console.WriteLine(e);
                    OnEvent(e);
                }
            }
            Console.WriteLine("{0} Event manager thread stopped: Framework = {1} Clock = {2}", DateTime.Now, this.framework.Name, this.framework.Clock.GetModeAsString());
        }

        private void method_4(Event e)
        {
            var onSimulatorStart = (OnSimulatorStart) e;
            if (this.framework.Clock.Mode == ClockMode.Simulation)
                this.framework.Clock.DateTime = onSimulatorStart.DateTime;
            if (this.bus != null)
                this.bus.ResetCounts();
            EventCount = DataEventCount = 0;
//            this.stopwatch.Reset();
//            this.stopwatch_0.Start();
        }

        private void method_5(Event event_0)
        {
            this.framework.StrategyManager.Stop();
          //  this.stopwatch_0.Stop();
       //     long elapsedMilliseconds = this.stopwatch_0.ElapsedMilliseconds;
//            if (elapsedMilliseconds != 0L)
//                Console.WriteLine((string) (object) DateTime.Now + (object) " Data run done, count = " + (string) (object) this.long_1 + " ms = " + (string) (object) this.stopwatch_0.ElapsedMilliseconds + " event/sec = " + (string) (object) (this.long_1 / elapsedMilliseconds * 1000L));
//            else
                Console.WriteLine( "{0} Data run done, count = {1} ms = 0",DateTime.Now, DataEventCount);
        }

        private void method_6(Event e)
        {
        }
    }


//    public class EventManager
//    {
//        private Framework framework_0;
//        internal EventBus eventBus_0;
//        internal EventFilter eventFilter_0;
//        internal EventLogger eventLogger_0;
//        internal EventDispatcher eventDispatcher_0;
//        internal BarFactory barFactory_0;
//        internal BarSliceFactory barSliceFactory_0;
//        internal EventManagerStatus eventManagerStatus_0;
//        private Thread cNaUxJbMd9;
//        private bool bool_0;
//        private Stopwatch stopwatch_0;
//        private long long_0;
//        private long long_1;
//        private bool bool_1;
//        private byte byte_0;
//        private IdArray<EventManager.Delegate0> idArray_0;
//
//        public EventManagerStatus Status
//        {
//            get
//            {
//                return this.eventManagerStatus_0;
//            }
//        }
//
//        public EventFilter Filter
//        {
//            get
//            {
//                return this.eventFilter_0;
//            }
//            set
//            {
//                this.eventFilter_0 = value;
//            }
//        }
//
//        public EventLogger Logger
//        {
//            get
//            {
//                return this.eventLogger_0;
//            }
//            set
//            {
//                this.eventLogger_0 = value;
//            }
//        }
//
//        public BarFactory BarFactory
//        {
//            get
//            {
//                return this.barFactory_0;
//            }
//            set
//            {
//                this.barFactory_0 = value;
//            }
//        }
//
//        public BarSliceFactory BarSliceFactory
//        {
//            get
//            {
//                return this.barSliceFactory_0;
//            }
//            set
//            {
//                this.barSliceFactory_0 = value;
//            }
//        }
//
//        public EventDispatcher Dispatcher
//        {
//            get
//            {
//                return this.eventDispatcher_0;
//            }
//            set
//            {
//                this.eventDispatcher_0 = value;
//            }
//        }
//
//        public long EventCount
//        {
//            get
//            {
//                return this.long_0;
//            }
//        }
//
//        public long DataEventCount
//        {
//            get
//            {
//                return this.long_1;
//            }
//        }
//
//        static EventManager()
//        {
//            System.ComponentModel.LicenseManager.Validate(typeof (EventManager));
//        }
//
//        public EventManager(Framework framework, EventBus bus)
//        {
//            this.eventManagerStatus_0 = EventManagerStatus.Stopped;
//            this.stopwatch_0 = new Stopwatch();
//            this.byte_0 = (byte) 6;
//            this.idArray_0 = new IdArray<EventManager.Delegate0>(1000);
//            this.framework_0 = framework;
//            this.eventBus_0 = bus;
//            this.barFactory_0 = new BarFactory(framework);
//            this.barSliceFactory_0 = new BarSliceFactory(framework);
//            this.eventDispatcher_0 = new EventDispatcher(framework);
//            this.idArray_0[104] = new EventManager.Delegate0(this.method_2);
//            this.idArray_0[105] = new EventManager.Delegate0(this.method_3);
//            this.idArray_0[107] = new EventManager.Delegate0(this.method_4);
//            this.idArray_0[108] = new EventManager.Delegate0(this.method_5);
//            this.idArray_0[109] = new EventManager.Delegate0(this.method_6);
//            this.idArray_0[2] = new EventManager.Delegate0(this.method_7);
//            this.idArray_0[3] = new EventManager.Delegate0(this.method_8);
//            this.idArray_0[4] = new EventManager.Delegate0(this.method_9);
//            this.idArray_0[6] = new EventManager.Delegate0(this.method_10);
//            this.idArray_0[136] = new EventManager.Delegate0(this.method_11);
//            this.idArray_0[8] = new EventManager.Delegate0(this.method_12);
//            this.idArray_0[9] = new EventManager.Delegate0(this.method_13);
//            this.idArray_0[23] = new EventManager.Delegate0(this.method_15);
//            this.idArray_0[22] = new EventManager.Delegate0(this.method_14);
//            this.idArray_0[13] = new EventManager.Delegate0(this.method_16);
//            this.idArray_0[117] = new EventManager.Delegate0(this.method_18);
//            this.idArray_0[118] = new EventManager.Delegate0(this.method_19);
//            this.idArray_0[119] = new EventManager.Delegate0(this.method_20);
//            this.idArray_0[120] = new EventManager.Delegate0(this.method_21);
//            this.idArray_0[121] = new EventManager.Delegate0(this.method_22);
//            this.idArray_0[122] = new EventManager.Delegate0(this.method_23);
//            this.idArray_0[123] = new EventManager.Delegate0(this.method_24);
//            this.idArray_0[124] = new EventManager.Delegate0(this.method_25);
//            this.idArray_0[125] = new EventManager.Delegate0(this.method_26);
//            this.idArray_0[113] = new EventManager.Delegate0(this.method_27);
//            this.idArray_0[114] = new EventManager.Delegate0(this.method_28);
//            this.idArray_0[110] = new EventManager.Delegate0(this.method_29);
//            this.idArray_0[111] = new EventManager.Delegate0(this.method_30);
//            this.idArray_0[112] = new EventManager.Delegate0(this.method_31);
//            this.idArray_0[15] = new EventManager.Delegate0(this.method_32);
//            this.idArray_0[50] = new EventManager.Delegate0(this.method_33);
//            this.idArray_0[52] = new EventManager.Delegate0(this.method_34);
//            this.idArray_0[134] = new EventManager.Delegate0(this.method_35);
//            this.idArray_0[135] = new EventManager.Delegate0(this.method_36);
//            this.idArray_0[140] = new EventManager.Delegate0(this.method_17);
//            this.idArray_0[212] = new EventManager.Delegate0(this.method_37);
//            this.idArray_0[137] = new EventManager.Delegate0(this.method_38);
//            if (bus == null)
//                return;
//            this.cNaUxJbMd9 = new Thread(new ThreadStart(this.method_1));
//            this.cNaUxJbMd9.Name = "Event Manager Thread";
//            this.cNaUxJbMd9.IsBackground = true;
//            this.cNaUxJbMd9.Start();
//        }
//
//        public void Start()
//        {
//            if (this.eventManagerStatus_0 == EventManagerStatus.Running)
//                return;
//            Console.WriteLine((string) (object) DateTime.Now + (object) " Event manager started at " + (string) (object) this.framework_0.Clock.DateTime);
//            this.eventManagerStatus_0 = EventManagerStatus.Running;
//            this.OnEvent((Event) new OnEventManagerStarted());
//        }
//
//        public void Pause()
//        {
//            if (this.eventManagerStatus_0 == EventManagerStatus.Paused)
//                return;
//            Console.WriteLine((string) (object) DateTime.Now + (object) " Event manager paused at " + (string) (object) this.framework_0.Clock.DateTime);
//            this.eventManagerStatus_0 = EventManagerStatus.Paused;
//            this.OnEvent((Event) new OnEventManagerPaused());
//        }
//
//        public void Pause(DateTime dateTime)
//        {
//            this.framework_0.Clock.AddReminder(new ReminderCallback(this.method_0), dateTime, (object) null);
//        }
//
//        private void method_0(DateTime dateTime_0, object object_0)
//        {
//            this.Pause();
//        }
//
//        public void Resume()
//        {
//            if (this.eventManagerStatus_0 == EventManagerStatus.Running)
//                return;
//            Console.WriteLine((string) (object) DateTime.Now + (object) " Event manager resumed at " + (string) (object) this.framework_0.Clock.DateTime);
//            this.eventManagerStatus_0 = EventManagerStatus.Running;
//            this.OnEvent((Event) new OnEventManagerResumed());
//        }
//
//        public void Stop()
//        {
//            if (this.eventManagerStatus_0 == EventManagerStatus.Stopped)
//                return;
//            Console.WriteLine((string) (object) DateTime.Now + (object) " Event manager stopping at " + (string) (object) this.framework_0.Clock.DateTime);
//            this.eventManagerStatus_0 = EventManagerStatus.Stopping;
//            if (this.framework_0.Mode == FrameworkMode.Simulation)
//                this.OnEvent((Event) new OnSimulatorStop());
//            this.eventManagerStatus_0 = EventManagerStatus.Stopped;
//            this.framework_0.EventBus.Clear();
//            this.OnEvent((Event) new OnEventManagerStopped());
//            Console.WriteLine((string) (object) DateTime.Now + (object) " Event manager stopped at " + (string) (object) this.framework_0.Clock.DateTime);
//        }
//
//        public void Step(byte typeId = (byte) 0)
//        {
//            this.bool_1 = true;
//            this.byte_0 = typeId;
//            this.OnEvent((Event) new OnEventManagerStep());
//        }
//
//        private void method_1()
//        {
//            Console.WriteLine((string) (object) DateTime.Now + (object) " Event manager thread started: Framework = " + this.framework_0.Name + " Clock = " + this.framework_0.Clock.GetModeAsString());
//            this.eventManagerStatus_0 = EventManagerStatus.Running;
//            while (!this.bool_0)
//            {
//                if (this.eventManagerStatus_0 != EventManagerStatus.Running && (this.eventManagerStatus_0 != EventManagerStatus.Paused || !this.bool_1))
//                    Thread.Sleep(1);
//                else
//                    this.OnEvent(this.eventBus_0.Dequeue());
//            }
//            Console.WriteLine((string) (object) DateTime.Now + (object) " Event manager thread stopped: Framework = " + this.framework_0.Name + " Clock = " + this.framework_0.Clock.GetModeAsString());
//        }
//
//        public void OnEvent(Event e)
//        {
//            if (this.eventManagerStatus_0 == EventManagerStatus.Paused && this.bool_1 && ((int) this.byte_0 == 0 || (int) this.byte_0 == (int) e.TypeId))
//            {
//                Console.WriteLine((string) (object) DateTime.Now + (object) " Event : " + (string) (object) e);
//                this.bool_1 = false;
//            }
//            ++this.long_0;
//            if (this.eventFilter_0 != null && this.eventFilter_0.Filter(e) == null)
//                return;
//            if (this.idArray_0[(int) e.TypeId] != null)
//                this.idArray_0[(int) e.TypeId](e);
//            if (this.eventDispatcher_0 != null)
//                this.eventDispatcher_0.OnEvent(e);
//            if (this.eventLogger_0 == null)
//                return;
//            this.eventLogger_0.OnEvent(e);
//        }
//
//        private void method_2(Event event_0)
//        {
//            if (!(((OnProviderConnected) event_0).Provider is IDataProvider))
//                return;
//            this.framework_0.SubscriptionManager.method_0((IDataProvider) ((OnProviderConnected) event_0).provider_0);
//        }
//
//        private void method_3(Event event_0)
//        {
//            if (!(((OnProviderDisconnected) event_0).HfjKxsEkn6 is IDataProvider))
//                return;
//            this.framework_0.SubscriptionManager.method_1((IDataProvider) ((OnProviderDisconnected) event_0).HfjKxsEkn6);
//        }
//
//        private void method_4(Event event_0)
//        {
//            OnSimulatorStart onSimulatorStart = (OnSimulatorStart) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Simulation)
//                this.framework_0.Clock.DateTime = onSimulatorStart.DateTime;
//            if (this.eventBus_0 != null)
//                this.eventBus_0.ResetCounts();
//            this.long_0 = 0L;
//            this.long_1 = 0L;
//            this.stopwatch_0.Reset();
//            this.stopwatch_0.Start();
//        }
//
//        private void method_5(Event event_0)
//        {
//            this.framework_0.StrategyManager.Stop();
//            this.stopwatch_0.Stop();
//            long elapsedMilliseconds = this.stopwatch_0.ElapsedMilliseconds;
//            if (elapsedMilliseconds != 0L)
//                Console.WriteLine((string) (object) DateTime.Now + (object) " Data run done, count = " + (string) (object) this.long_1 + " ms = " + (string) (object) this.stopwatch_0.ElapsedMilliseconds + " event/sec = " + (string) (object) (this.long_1 / elapsedMilliseconds * 1000L));
//            else
//                Console.WriteLine(string.Concat(new object[4]
//                    {
//                        (object) DateTime.Now,
//                        (object) " Data run done, count = ",
//                        (object) this.long_1,
//                        (object) " ms = 0"
//                    }));
//        }
//
//        private void method_6(Event event_0)
//        {
//        }
//
//        private void method_7(Event event_0)
//        {
//            ++this.long_1;
//            Bid bid = (Bid) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Simulation)
//                this.framework_0.Clock.DateTime = bid.DateTime;
//            else
//                bid.DateTime = this.framework_0.Clock.DateTime;
//            if (bid.ExchangeDateTime > this.framework_0.ExchangeClock.DateTime)
//                this.framework_0.ExchangeClock.DateTime = bid.ExchangeDateTime;
//            else if (bid.ExchangeDateTime > this.framework_0.ExchangeClock.DateTime)
//                Console.WriteLine(string.Concat(new object[4]
//                    {
//                        (object) "EventManager::OnBid Exchange datetime is out of synch : bid datetime = ",
//                        (object) bid.ExchangeDateTime,
//                        (object) " clock datetime = ",
//                        (object) this.framework_0.ExchangeClock.DateTime
//                    }));
//            this.barFactory_0.method_0((DataObject) bid);
//            this.framework_0.dataManager_0.method_1(bid);
//            this.framework_0.InstrumentManager.GetById(bid.InstrumentId).Bid = bid;
//            this.framework_0.providerManager_0.ginterface3_0.OnBid(bid);
//            this.framework_0.StrategyManager.method_4(bid);
//        }
//
//        private void method_8(Event event_0)
//        {
//            ++this.long_1;
//            Ask ask = (Ask) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Simulation)
//                this.framework_0.Clock.DateTime = ask.DateTime;
//            else
//                ask.dateTime = this.framework_0.clock_0.DateTime;
//            if (ask.exchangeDateTime > this.framework_0.clock_1.DateTime)
//                this.framework_0.clock_1.DateTime = ask.exchangeDateTime;
//            else if (ask.exchangeDateTime > this.framework_0.clock_1.DateTime)
//                Console.WriteLine(string.Concat(new object[4]
//                    {
//                        (object) "EventManager::OnAsk Exchange datetime is out of synch : ask datetime = ",
//                        (object) ask.exchangeDateTime,
//                        (object) " clock datetime = ",
//                        (object) this.framework_0.clock_1.dateTime_0
//                    }));
//            this.barFactory_0.method_0((DataObject) ask);
//            this.framework_0.dataManager_0.method_2(ask);
//            this.framework_0.instrumentManager_0.GetById(ask.instrumentId).ask_0 = ask;
//            this.framework_0.providerManager_0.ginterface3_0.OnAsk(ask);
//            this.framework_0.StrategyManager.method_5(ask);
//        }
//
//        private void method_9(Event event_0)
//        {
//            ++this.long_1;
//            Trade trade = (Trade) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Simulation)
//                this.framework_0.Clock.DateTime = trade.dateTime;
//            else
//                trade.dateTime = this.framework_0.clock_0.DateTime;
//            if (trade.exchangeDateTime > this.framework_0.clock_1.DateTime)
//                this.framework_0.clock_1.DateTime = trade.exchangeDateTime;
//            else if (trade.exchangeDateTime > this.framework_0.clock_1.DateTime)
//                Console.WriteLine(string.Concat(new object[4]
//                    {
//                        (object) "EventManager::OnTrade Exchange datetime is out of synch : trade datetime = ",
//                        (object) trade.exchangeDateTime,
//                        (object) " clock datetime = ",
//                        (object) this.framework_0.clock_1.dateTime_0
//                    }));
//            this.barFactory_0.method_0((DataObject) trade);
//            this.framework_0.dataManager_0.method_3(trade);
//            this.framework_0.instrumentManager_0.GetById(trade.instrumentId).trade_0 = trade;
//            this.framework_0.providerManager_0.ginterface3_0.OnTrade(trade);
//            this.framework_0.StrategyManager.method_6(trade);
//        }
//
//        private void method_10(Event event_0)
//        {
//            ++this.long_1;
//            Bar bar = (Bar) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Simulation)
//                this.framework_0.Clock.DateTime = bar.dateTime;
//            if (bar.barStatus_0 == BarStatus.Open)
//            {
//                if (bar.barType_0 == BarType.Time && !this.barSliceFactory_0.method_0(bar))
//                    return;
//                this.framework_0.providerManager_0.ginterface3_0.OnBarOpen(bar);
//                this.framework_0.StrategyManager.method_9(bar);
//            }
//            else
//            {
//                this.framework_0.dataManager_0.method_4(bar);
//                this.framework_0.instrumentManager_0.GetById(bar.int_0).bar_0 = bar;
//                this.framework_0.providerManager_0.ginterface3_0.OnBar(bar);
//                this.framework_0.StrategyManager.method_10(bar);
//                if (bar.barType_0 != BarType.Time)
//                    return;
//                this.barSliceFactory_0.method_1(bar);
//            }
//        }
//
//        private void method_11(Event event_0)
//        {
//            BarSlice barSlice_0 = (BarSlice) event_0;
//            barSlice_0.dateTime = this.framework_0.clock_0.DateTime;
//            this.framework_0.StrategyManager.method_11(barSlice_0);
//        }
//
//        private void method_12(Event event_0)
//        {
//            ++this.long_1;
//            Level2Snapshot level2Snapshot = (Level2Snapshot) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Simulation)
//                this.framework_0.Clock.DateTime = level2Snapshot.dateTime;
//            else
//                level2Snapshot.dateTime = this.framework_0.Clock.DateTime;
//            this.framework_0.dataManager_0.method_5(level2Snapshot);
//            this.framework_0.providerManager_0.ginterface3_0.OnLevel2(level2Snapshot);
//            this.framework_0.StrategyManager.method_7(level2Snapshot);
//        }
//
//        private void method_13(Event event_0)
//        {
//            ++this.long_1;
//            Level2Update level2Update = (Level2Update) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Simulation)
//                this.framework_0.Clock.DateTime = level2Update.dateTime;
//            else
//                level2Update.dateTime = this.framework_0.Clock.DateTime;
//            this.framework_0.dataManager_0.method_6(level2Update);
//            this.framework_0.providerManager_0.ginterface3_0.OnLevel2(level2Update);
//            this.framework_0.StrategyManager.method_8(level2Update);
//        }
//
//        private void method_14(Event event_0)
//        {
//            ++this.long_1;
//            Fundamental fundamental_0 = (Fundamental) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Simulation)
//                this.framework_0.Clock.DateTime = fundamental_0.dateTime;
//            else
//                fundamental_0.dateTime = this.framework_0.clock_0.DateTime;
//            this.framework_0.dataManager_0.method_8(fundamental_0);
//            this.framework_0.StrategyManager.method_13(fundamental_0);
//        }
//
//        private void method_15(Event event_0)
//        {
//            ++this.long_1;
//            News news_0 = (News) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Simulation)
//                this.framework_0.Clock.DateTime = news_0.dateTime;
//            else
//                news_0.dateTime = this.framework_0.clock_0.DateTime;
//            this.framework_0.dataManager_0.method_7(news_0);
//            this.framework_0.StrategyManager.method_12(news_0);
//        }
//
//        private void method_16(Event event_0)
//        {
//            ExecutionReport executionReport_0 = (ExecutionReport) event_0;
//            if (this.framework_0.Clock.Mode == ClockMode.Realtime)
//                executionReport_0.dateTime = this.framework_0.Clock.DateTime;
//            this.framework_0.orderManager_0.zvQuombvjY(executionReport_0);
//            this.framework_0.portfolioManager_0.method_1(executionReport_0);
//            this.framework_0.StrategyManager.method_23(executionReport_0);
//            this.framework_0.eventServer_0.EmitQueued();
//        }
//
//        private void method_17(Event event_0)
//        {
//            this.framework_0.accountDataManager_0.method_1((AccountData) event_0);
//        }
//
//        private void method_18(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_14(((OnSendOrder) event_0).order_0);
//        }
//
//        private void method_19(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_15(((OnPendingNewOrder) event_0).order_0);
//        }
//
//        private void method_20(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_16(((OnNewOrder) event_0).order_0);
//        }
//
//        private void method_21(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_17(((OnOrderStatusChanged) event_0).order_0);
//        }
//
//        private void method_22(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_19(((OnOrderPartiallyFilled) event_0).order_0);
//        }
//
//        private void method_23(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_18(((OnOrderFilled) event_0).order_0);
//        }
//
//        private void method_24(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_21(((OnOrderReplaced) event_0).order_0);
//        }
//
//        private void method_25(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_20(((OnOrderCancelled) event_0).order_0);
//        }
//
//        private void method_26(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_22(((OnOrderDone) event_0).order_0);
//        }
//
//        private void method_27(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_24((OnFill) event_0);
//        }
//
//        private void method_28(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_25((OnTransaction) event_0);
//        }
//
//        private void method_29(Event event_0)
//        {
//            OnPositionOpened onPositionOpened = (OnPositionOpened) event_0;
//            this.framework_0.StrategyManager.method_26(onPositionOpened.portfolio, onPositionOpened.position);
//        }
//
//        private void method_30(Event event_0)
//        {
//            OnPositionClosed onPositionClosed = (OnPositionClosed) event_0;
//            this.framework_0..method_27(onPositionClosed.portfolio, onPositionClosed.position);
//        }
//
//        private void method_31(Event event_0)
//        {
//            OnPositionChanged onPositionChanged = (OnPositionChanged) event_0;
//            this.framework_0.StrategyManager.method_28(onPositionChanged.portfolio, onPositionChanged.position);
//        }
//
//        private void method_32(Event event_0)
//        {
//            Reminder reminder = (Reminder) event_0;
//            if (reminder.clock_0.clockType_0 == ClockType.Local && reminder.clock_0.clockMode_0 == ClockMode.Simulation || reminder.clock_0.clockType_0 == ClockType.Exchange)
//                reminder.clock_0.DateTime = event_0.dateTime;
//            ((Reminder) event_0).Execute();
//        }
//
//        private void method_33(Event event_0)
//        {
//            this.framework_0.groupManager_0.method_0((Group) event_0);
//        }
//
//        private void method_34(Event event_0)
//        {
//            this.framework_0.groupManager_0.method_1((GroupEvent) event_0);
//        }
//
//        private void method_35(Event event_0)
//        {
//            this.framework_0.dataManager_0.method_9((HistoricalData) event_0);
//        }
//
//        private void method_36(Event event_0)
//        {
//            this.framework_0.dataManager_0.method_10((HistoricalDataEnd) event_0);
//        }
//
//        private void method_37(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_29(((OnUserCommand) event_0).string_0);
//        }
//
//        private void method_38(Event event_0)
//        {
//            this.framework_0.StrategyManager.method_30(((OnStrategyEvent) event_0).object_0);
//        }
//
//        public void Clear()
//        {
//            this.long_0 = 0L;
//            this.long_1 = 0L;
//            this.barFactory_0.Clear();
//            this.barSliceFactory_0.Clear();
//        }
//
//        public void Close()
//        {
//        }
//        private delegate void Delegate0(Event e);
//    }
}
