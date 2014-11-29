// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Linq;
using System.Threading;

namespace SmartQuant
{
//    public class EventBus
//    {
//        private Framework framework;
//        private int qIndex;
//        private EventQueue[] queues;
//        private Event event_0;
//        public EventBusMode Mode { get; set; }
//
//        public EventPipe DataPipe { get; private set; }
//
//        public EventPipe ExecutionPipe { get; private set; }
//
//        public EventPipe HistoricalPipe  { get; private set; }
//
//        public EventPipe ServicePipe  { get; private set; }
//
//        internal IEventQueue LocalClockQueue { get; set; }
//
//        internal IEventQueue ExchangeClockQueue { get; set; }
//
//        public EventBus(Framework framework, EventBusMode mode = EventBusMode.Simulation)
//        {
//            this.framework = framework;
//            this.queues = new EventQueue[1024];
//            Mode = mode;
//            DataPipe = new EventPipe(framework);
//            ExecutionPipe = new EventPipe(framework);
//            HistoricalPipe = new EventPipe(framework);
//            ServicePipe = new EventPipe(framework);
//        }
//
//        public void Attach(EventBus bus)
//        {
//            var q = new EventQueue(EventQueueId.Data, EventQueueType.Master, EventQueuePriority.Normal, 32768);
//            q.IsSynched = true;
//            q.Name = "attached";
//            q.Enqueue(new OnQueueOpened(q));
//            bus.DataPipe.Add(q);
//            this.queues[this.qIndex++] = q;
//        }
//
////        public Event Dequeue()
////        {
////            Event e = null;
////            if (Mode == EventBusMode.Simulation)
////            {
////                while (true)
////                {
////                    while (!DataPipe.IsEmpty() && e == null)
////                    {
////                        e = DataPipe.Read();
////                        if (e!=null && e.DateTime < this.framework.Clock.DateTime)
////                        {
////                            if (e.TypeId == EventType.OnQueueOpened || e.TypeId == EventType.OnQueueClosed || e.TypeId == EventType.OnSimulatorStop || e.TypeId == EventType.OnSimulatorProgress)
////                            {
////                                e.DateTime = this.framework.Clock.DateTime;
////                                break;
////                            }
////                            else
////                                Console.WriteLine("EventBus::Dequeue Skipping: {0} {1} <> {2} ", e, e.DateTime, this.framework.Clock.DateTime);
////                        }
////                        else
////                            break;
////                    }
////                    if (!ExecutionPipe.IsEmpty())
////                        return ExecutionPipe.Read();
////                    if (e != null && !LocalClockQueue.IsEmpty() && (LocalClockQueue.PeekDateTime() <= this.framework.Clock.DateTime))
////                        return LocalClockQueue.Read();
////                    if (e != null && (e.TypeId == EventType.Bid || e.TypeId == EventType.Ask || e.TypeId == EventType.Trade) && !ExchangeClockQueue.IsEmpty() && ExchangeClockQueue.PeekDateTime() <= ((Tick)e).ExchangeDateTime)
////                        return ExchangeClockQueue.Read();
////                    if (!ServicePipe.IsEmpty())
////                        return ServicePipe.Read();
////                    if (e != null)
////                    {
////                        for (int i = 0; i < qIndex; ++i)
////                            this.queues[i].Enqueue(e);
////                        return e;
////                    }
////                    Thread.Sleep(1);
////                }
////            }
////            else
////            {
////                while (true)
////                {
////                    if (!DataPipe.IsEmpty())
////                        e = DataPipe.Read();
////                    if (!LocalClockQueue.IsEmpty() && (LocalClockQueue.PeekDateTime() <= this.framework.Clock.DateTime))
////                        return LocalClockQueue.Read();
////                    if (e != null && (e.TypeId == EventType.Bid || e.TypeId == EventType.Ask || e.TypeId == EventType.Trade) && !ExchangeClockQueue.IsEmpty() && ExchangeClockQueue.PeekDateTime() <= ((Tick)e).ExchangeDateTime)
////                        return ExchangeClockQueue.Read();
////                    if (!ExecutionPipe.IsEmpty())
////                        return ExecutionPipe.Read();
////                    if (!ServicePipe.IsEmpty())
////                        return ServicePipe.Read();
////                    if (e != null)
////                        return e;
////                    Thread.Sleep(1);
////                }
////            }
////        }
////
////
//
//        public Event Dequeue()
//        {
//            if (Mode == EventBusMode.Simulation)
//            {
//                while (true)
//                {
//                    while (!DataPipe.IsEmpty() && this.event_0 == null)
//                    {
//                        Event @event = DataPipe.Read();
//                        if (@event.DateTime < this.framework.Clock.DateTime)
//                        {
//                            if ((int) @event.TypeId != 205 && (int) @event.TypeId != 206 && ((int) @event.TypeId != 108 && (int) @event.TypeId != 109))
//                            {
//                                Console.WriteLine("EventBus::Dequeue Skipping: " + (object) @event + " " + (string) (object) @event.DateTime + " <> " + (string) (object) this.framework.Clock.DateTime);
//                            }
//                            else
//                            {
//                                @event.DateTime = this.framework.Clock.DateTime;
//                                this.event_0 = @event;
//                                break;
//                            }
//                        }
//                        else
//                        {
//                            this.event_0 = @event;
//                            break;
//                        }
//                    }
//                    if (ExecutionPipe.IsEmpty())
//                    {
//                        if (LocalClockQueue.IsEmpty() || this.event_0 == null || !(LocalClockQueue.PeekDateTime() <= this.event_0.DateTime))
//                        {
//                            if (ExchangeClockQueue.IsEmpty() || this.event_0 == null || (int) this.event_0.TypeId != 2 && (int) this.event_0.TypeId != 3 && (int) this.event_0.TypeId != 4 || !(this.ExchangeClockQueue.PeekDateTime() <= ((Tick) this.event_0).ExchangeDateTime))
//                            {
//                                if (ServicePipe.IsEmpty())
//                                {
//                                    if (this.event_0 == null)
//                                        Thread.Sleep(1);
//                                    else
//                                        goto label_17;
//                                }
//                                else
//                                    goto label_16;
//                            }
//                            else
//                                goto label_15;
//                        }
//                        else
//                            goto label_14;
//                    }
//                    else
//                        break;
//                }
//                return ExecutionPipe.Read();
//                label_14:
//                return LocalClockQueue.Read();
//                label_15:
//                return ExchangeClockQueue.Read();
//                label_16:
//                return ServicePipe.Read();
//                label_17:
//                Event event1 = this.event_0;
//                this.event_0 = (Event) null;
//                for (int index = 0; index < this.qIndex; ++index)
//                    this.queues[index].Enqueue(event1);
//                return event1;
//            }
//            else
//            return null;
//        }
//
//        public void ResetCounts()
//        {
//            // no-op
//        }
//
//        public void Clear()
//        {
//            LocalClockQueue.Clear();
//            DataPipe.Clear();
//            ExecutionPipe.Clear();
//            ExecutionPipe.Clear();
//            ServicePipe.Clear();
//            for (int i = 0; i < qIndex; ++i)
//                this.queues[i] = null;
//            qIndex = 0;
//        }
//    }

    public class EventBus
    {
        internal Framework framework_0;
        internal EventPipe eventPipe_0;
        internal EventPipe eventPipe_1;
        internal EventPipe eventPipe_2;
        internal EventPipe eventPipe_3;
        internal IEventQueue ieventQueue_0;
        internal IEventQueue ieventQueue_1;
        internal EventBusMode eventBusMode_0;
        internal int int_0;
        internal EventQueue[] eventQueue_0;
        internal bool bool_0;
        private Event event_0;

         internal IEventQueue LocalClockQueue
        {
            get
            {
                return ieventQueue_0;
            }
             set
            {
                ieventQueue_0= value;
            }
        }
        internal IEventQueue ExchangeClockQueue
        {
        get
        {
            return ieventQueue_1;
        }
        set
        {
            ieventQueue_1= value;
        }
    }
        public EventBusMode Mode
        {
            get
            {
                return this.eventBusMode_0;
            }
            set
            {
                if (this.eventBusMode_0 == value)
                    return;
                this.eventBusMode_0 = value;
            }
        }

        public EventPipe DataPipe
        {
            get
            {
                return this.eventPipe_0;
            }
        }

        public EventPipe ExecutionPipe
        {
            get
            {
                return this.eventPipe_1;
            }
        }

        public EventPipe ServicePipe
        {
            get
            {
                return this.eventPipe_2;
            }
        }

        public EventPipe HistoricalPipe
        {
            get
            {
                return this.eventPipe_3;
            }
        }
            

        public EventBus(Framework framework, EventBusMode mode = EventBusMode.Simulation)
        {
            this.eventBusMode_0 = EventBusMode.Simulation;
            this.eventQueue_0 = new EventQueue[1024];
            this.bool_0 = true;

            this.framework_0 = framework;
            this.eventBusMode_0 = mode;
            this.eventPipe_0 = new EventPipe(framework);
            this.eventPipe_1 = new EventPipe(framework);
            this.eventPipe_2 = new EventPipe(framework);
            this.eventPipe_3 = new EventPipe(framework);
        }

        public void Attach(EventBus bus)
        {
            EventQueue queue = new EventQueue((byte) 1, (byte) 0, (byte) 2, 25000);
            queue.IsSynched = true;
            queue.Name = "attached";
            queue.Enqueue((Event) new OnQueueOpened(queue));
            bus.eventPipe_0.Add((IEventQueue) queue);
            this.eventQueue_0[this.int_0++] = queue;
        }

        public Event Dequeue()
        {
            if (this.eventBusMode_0 == EventBusMode.Simulation)
            {
                while (true)
                {
                    while (!this.eventPipe_0.IsEmpty() && this.event_0 == null)
                    {
                        Event @event = this.eventPipe_0.Read();
                        if (@event.DateTime < this.framework_0.Clock.DateTime)
                        {
                            if ((int) @event.TypeId != 205 && (int) @event.TypeId != 206 && ((int) @event.TypeId != 108 && (int) @event.TypeId != 109))
                            {
                                Console.WriteLine("EventBus::Dequeue Skipping: " + (object) @event + " " + (string) (object) @event.DateTime + " <> " + (string) (object) this.framework_0.Clock.DateTime);
                            }
                            else
                            {
                                @event.DateTime = this.framework_0.Clock.DateTime;
                                this.event_0 = @event;
                                break;
                            }
                        }
                        else
                        {
                            this.event_0 = @event;
                            break;
                        }
                    }
                    if (this.eventPipe_1.IsEmpty())
                    {
                        if (this.ieventQueue_0.IsEmpty() || this.event_0 == null || !(this.ieventQueue_0.PeekDateTime() <= this.event_0.DateTime))
                        {
                            if (this.ieventQueue_1.IsEmpty() || this.event_0 == null || (int) this.event_0.TypeId != 2 && (int) this.event_0.TypeId != 3 && (int) this.event_0.TypeId != 4 || !(this.ieventQueue_1.PeekDateTime() <= ((Tick) this.event_0).ExchangeDateTime))
                            {
                                if (this.eventPipe_2.IsEmpty())
                                {
                                    if (this.event_0 == null)
                                        Thread.Sleep(1);
                                    else
                                        goto label_17;
                                }
                                else
                                    goto label_16;
                            }
                            else
                                goto label_15;
                        }
                        else
                            goto label_14;
                    }
                    else
                        break;
                }
                return this.eventPipe_1.Read();
                label_14:
                return this.ieventQueue_0.Read();
                label_15:
                return this.ieventQueue_1.Read();
                label_16:
                return this.eventPipe_2.Read();
                label_17:
                Event event1 = this.event_0;
                this.event_0 = (Event) null;
                for (int index = 0; index < this.int_0; ++index)
                    this.eventQueue_0[index].Enqueue(event1);
                return event1;
            }
            else
            {
                while (true)
                {
                    do
                    {
                        if (!this.eventPipe_0.IsEmpty() && this.event_0 == null)
                            goto label_29;
                        label_22:
                        if (this.ieventQueue_0.IsEmpty() || !(this.ieventQueue_0.PeekDateTime() <= this.framework_0.Clock.DateTime))
                        {
                            if (this.ieventQueue_1.IsEmpty() || this.event_0 == null || (int) this.event_0.TypeId != 2 && (int) this.event_0.TypeId != 3 && (int) this.event_0.TypeId != 4 || !(this.ieventQueue_1.PeekDateTime() <= ((Tick) this.event_0).ExchangeDateTime))
                            {
                                if (this.eventPipe_1.IsEmpty())
                                {
                                    if (this.eventPipe_2.IsEmpty())
                                    {
                                        if (this.event_0 == null)
                                            continue;
                                        else
                                            goto label_35;
                                    }
                                    else
                                        goto label_34;
                                }
                                else
                                    goto label_33;
                            }
                            else
                                goto label_32;
                        }
                        else
                            goto label_31;
                        label_29:
                        this.event_0 = this.eventPipe_0.Read();
                        goto label_22;
                    }
                    while (!this.bool_0);
                    Thread.Sleep(1);
                }
                label_31:
                return this.ieventQueue_0.Read();
                label_32:
                return this.ieventQueue_1.Read();
                label_33:
                return this.eventPipe_1.Read();
                label_34:
                return this.eventPipe_2.Read();
                label_35:
                Event @event = this.event_0;
                this.event_0 = (Event) null;
                return @event;
            }
        }

        public void ResetCounts()
        {
        }

        public void Clear()
        {
            this.event_0 = (Event) null;
            this.ieventQueue_0.Clear();
            this.eventPipe_2.Clear();
            this.eventPipe_0.Clear();
            this.eventPipe_1.Clear();
            this.eventPipe_3.Clear();
            for (int index = 0; index < this.int_0; ++index)
                this.eventQueue_0[index] = (EventQueue) null;
            this.int_0 = 0;
        }
    }
}
