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
            return unsyncedQueues.All(q => q.IsEmpty()) && syncedQueues.Any(q => q.IsEmpty());
        }

        public Event Read()
        {
            IEventQueue queue = null;

            queue = unsyncedQueues.FirstOrDefault(q => !q.IsEmpty());
            if (queue != null)
            {
                var e = queue.Read();
                if (e.TypeId == EventType.OnQueueClosed)
                {
                    unsyncedQueues.Remove(queue);
                }
                return e;
            }
            DateTime minDateTime = DateTime.MaxValue;
            IEventQueue removedQueue = null;
            IEventQueue minDateTimeQueue = null;
            Event evt;
            foreach (var q in syncedQueues)
            {
                evt = q.Peek();
                if (evt.TypeId == EventType.OnQueueClosed && (evt as OnQueueClosed).Queue == q)
                {
                    removedQueue = q;
                    break;
                }
                else
                {
                    minDateTime = minDateTime > evt.DateTime ? evt.DateTime : minDateTime;
                    minDateTimeQueue = q;
                }
            }

            if (removedQueue != null)
            {
                syncedQueues.Remove(removedQueue);
                if (syncedQueues.Count == 0 && this.framework.Mode == FrameworkMode.Simulation && removedQueue.Name != "Simulator stop queue")
                {
                    var newQueue = new EventQueue(EventQueueId.Data, EventQueueType.Master, EventQueuePriority.Normal, 16);
                    newQueue.IsSynched = true;
                    newQueue.Name = "Simulator stop queue";
                    newQueue.Enqueue(new Event[] { new OnQueueOpened(newQueue), new OnSimulatorStop(), new OnQueueClosed(newQueue) });
                    Add(newQueue);
                }
                return removedQueue.Read();
            }
            return minDateTimeQueue == null ? null : minDateTimeQueue.Read();
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
