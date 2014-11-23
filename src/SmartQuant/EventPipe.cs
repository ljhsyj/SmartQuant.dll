// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Linq;
using LinkedList = System.Collections.Generic.LinkedList<global::SmartQuant.IEventQueue>;
using LinkedListNode = System.Collections.Generic.LinkedListNode<global::SmartQuant.IEventQueue>;

namespace SmartQuant
{
	public class EventPipe
	{
        private Framework framework;
        private LinkedList syncedQueues;
        private LinkedList unsyncedQueues;

        public int Count
        {
            get
            {
                return unsyncedQueues.Count;
            }
        }

        public EventPipe(Framework framework)
        {
            this.framework = framework;
            this.syncedQueues = new LinkedList();
            this.unsyncedQueues = new LinkedList();
        }

        public void Add(IEventQueue queue)
        {
            if (queue.IsSynched)
                syncedQueues.AddLast(queue);
            else
                unsyncedQueues.AddLast(queue);
        }

        public bool IsEmpty()
        {
            return unsyncedQueues.All(q => q.IsEmpty()) && syncedQueues.All(q => q.IsEmpty());;
        }

        public Event Read()
        {
            IEventQueue queue = null;
            Event e = null;
            if (unsyncedQueues.Count != 0)
            {
                foreach (var q in unsyncedQueues)
                {
                    if (!q.IsEmpty())
                    {
                        e = q.Dequeue();
                        queue = q;
                        break;
                    }
                }

                if (e != null)
                {
                    if (e.TypeId == EventType.OnQueueClosed)
                        unsyncedQueues.Remove(queue);
                    return e;
                }
            }

            if (syncedQueues.Count == 0)
                return null;
                
            DateTime dt1 = DateTime.MaxValue;
            IEventQueue q1 = null;
            IEventQueue qFrom = null;
            foreach (var q in syncedQueues)
            {
                e = q.Peek();

                if (e.TypeId != EventType.OnQueueClosed)
                {
                    DateTime dt2 = e.DateTime;
                    if (e.DateTime <= dt1)
                    {
                        q1 = q;
                        dt1 = e.DateTime;
                    }
                    qFrom = q;
                }
                else
                {
                }
            }

            if (qFrom != null)
            {
                syncedQueues.Remove(qFrom);
                if (syncedQueues.Count == 0 && this.framework.Mode == FrameworkMode.Simulation && q1.Name != "Simulator stop queue")
                {
                    EventQueue newQueue = new EventQueue(EventQueueId.Data,EventQueueType.Master, EventQueuePriority.Normal, 16);
                    newQueue.IsSynched = true;
                    newQueue.Name = "Simulator stop queue";
                    newQueue.Enqueue(new Event[] { new OnQueueOpened(newQueue), new OnSimulatorStop(), new OnQueueClosed(newQueue)});
                    Add(newQueue);
                }
            }
            return q1.Read();
        }

        public Event Dequeue()
        {
            return null;
        }

        public void Clear()
        {
            syncedQueues.Clear();
            unsyncedQueues.Clear();
        }
	}
}
