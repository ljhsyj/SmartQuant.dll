// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Linq;
using System.Threading;

namespace SmartQuant
{
    public class EventBus
    {
        private Framework framework;
        private int qIndex;
        private EventQueue[] queues;
        private Event event_0;
        public EventBusMode Mode { get; set; }

        public EventPipe DataPipe { get; private set; }

        public EventPipe ExecutionPipe { get; private set; }

        public EventPipe HistoricalPipe  { get; private set; }

        public EventPipe ServicePipe  { get; private set; }

        internal IEventQueue LocalClockQueue { get; set; }

        internal IEventQueue ExchangeClockQueue { get; set; }

        public EventBus(Framework framework, EventBusMode mode = EventBusMode.Simulation)
        {
            this.framework = framework;
            this.queues = new EventQueue[1024];
            Mode = mode;
            DataPipe = new EventPipe(framework);
            ExecutionPipe = new EventPipe(framework);
            HistoricalPipe = new EventPipe(framework);
            ServicePipe = new EventPipe(framework);
        }

        public void Attach(EventBus bus)
        {
            var q = new EventQueue(EventQueueId.Data, EventQueueType.Master, EventQueuePriority.Normal, 32768);
            q.IsSynched = true;
            q.Name = "attached";
            q.Enqueue(new OnQueueOpened(q));
            bus.DataPipe.Add(q);
            this.queues[this.qIndex++] = q;
        }

//        public Event Dequeue()
//        {
//            Event e = null;
//            if (Mode == EventBusMode.Simulation)
//            {
//                while (true)
//                {
//                    while (!DataPipe.IsEmpty() && e == null)
//                    {
//                        e = DataPipe.Read();
//                        if (e!=null && e.DateTime < this.framework.Clock.DateTime)
//                        {
//                            if (e.TypeId == EventType.OnQueueOpened || e.TypeId == EventType.OnQueueClosed || e.TypeId == EventType.OnSimulatorStop || e.TypeId == EventType.OnSimulatorProgress)
//                            {
//                                e.DateTime = this.framework.Clock.DateTime;
//                                break;
//                            }
//                            else
//                                Console.WriteLine("EventBus::Dequeue Skipping: {0} {1} <> {2} ", e, e.DateTime, this.framework.Clock.DateTime);
//                        }
//                        else
//                            break;
//                    }
//                    if (!ExecutionPipe.IsEmpty())
//                        return ExecutionPipe.Read();
//                    if (e != null && !LocalClockQueue.IsEmpty() && (LocalClockQueue.PeekDateTime() <= this.framework.Clock.DateTime))
//                        return LocalClockQueue.Read();
//                    if (e != null && (e.TypeId == EventType.Bid || e.TypeId == EventType.Ask || e.TypeId == EventType.Trade) && !ExchangeClockQueue.IsEmpty() && ExchangeClockQueue.PeekDateTime() <= ((Tick)e).ExchangeDateTime)
//                        return ExchangeClockQueue.Read();
//                    if (!ServicePipe.IsEmpty())
//                        return ServicePipe.Read();
//                    if (e != null)
//                    {
//                        for (int i = 0; i < qIndex; ++i)
//                            this.queues[i].Enqueue(e);
//                        return e;
//                    }
//                    Thread.Sleep(1);
//                }
//            }
//            else
//            {
//                while (true)
//                {
//                    if (!DataPipe.IsEmpty())
//                        e = DataPipe.Read();
//                    if (!LocalClockQueue.IsEmpty() && (LocalClockQueue.PeekDateTime() <= this.framework.Clock.DateTime))
//                        return LocalClockQueue.Read();
//                    if (e != null && (e.TypeId == EventType.Bid || e.TypeId == EventType.Ask || e.TypeId == EventType.Trade) && !ExchangeClockQueue.IsEmpty() && ExchangeClockQueue.PeekDateTime() <= ((Tick)e).ExchangeDateTime)
//                        return ExchangeClockQueue.Read();
//                    if (!ExecutionPipe.IsEmpty())
//                        return ExecutionPipe.Read();
//                    if (!ServicePipe.IsEmpty())
//                        return ServicePipe.Read();
//                    if (e != null)
//                        return e;
//                    Thread.Sleep(1);
//                }
//            }
//        }
//
//

        public Event Dequeue()
        {
            if (Mode == EventBusMode.Simulation)
            {
                while (true)
                {
                    while (!DataPipe.IsEmpty() && this.event_0 == null)
                    {
                        Event @event = DataPipe.Read();
                        if (@event.DateTime < this.framework.Clock.DateTime)
                        {
                            if ((int) @event.TypeId != 205 && (int) @event.TypeId != 206 && ((int) @event.TypeId != 108 && (int) @event.TypeId != 109))
                            {
                                Console.WriteLine("EventBus::Dequeue Skipping: " + (object) @event + " " + (string) (object) @event.DateTime + " <> " + (string) (object) this.framework.Clock.DateTime);
                            }
                            else
                            {
                                @event.DateTime = this.framework.Clock.DateTime;
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
                    if (ExecutionPipe.IsEmpty())
                    {
                        if (LocalClockQueue.IsEmpty() || this.event_0 == null || !(LocalClockQueue.PeekDateTime() <= this.event_0.DateTime))
                        {
                            if (ExchangeClockQueue.IsEmpty() || this.event_0 == null || (int) this.event_0.TypeId != 2 && (int) this.event_0.TypeId != 3 && (int) this.event_0.TypeId != 4 || !(this.ExchangeClockQueue.PeekDateTime() <= ((Tick) this.event_0).ExchangeDateTime))
                            {
                                if (ServicePipe.IsEmpty())
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
                return ExecutionPipe.Read();
                label_14:
                return LocalClockQueue.Read();
                label_15:
                return ExchangeClockQueue.Read();
                label_16:
                return ServicePipe.Read();
                label_17:
                Event event1 = this.event_0;
                this.event_0 = (Event) null;
                for (int index = 0; index < this.qIndex; ++index)
                    this.queues[index].Enqueue(event1);
                return event1;
            }
            else
            return null;
        }

        public void ResetCounts()
        {
            // no-op
        }

        public void Clear()
        {
            LocalClockQueue.Clear();
            DataPipe.Clear();
            ExecutionPipe.Clear();
            ExecutionPipe.Clear();
            ServicePipe.Clear();
            for (int i = 0; i < qIndex; ++i)
                this.queues[i] = null;
            qIndex = 0;
        }
    }
}
