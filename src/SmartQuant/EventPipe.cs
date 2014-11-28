// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Linq;

namespace SmartQuant
{
    public class EventPipe
    {
        private Framework framework;
        private System.Collections.Generic.LinkedList<IEventQueue> syncedQueues = new System.Collections.Generic.LinkedList<IEventQueue>();
        private System.Collections.Generic.LinkedList<IEventQueue> unsyncedQueues = new System.Collections.Generic.LinkedList<IEventQueue>();

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
            // Check the unsynced queues first.
            var queue = unsyncedQueues.FirstOrDefault(q => !q.IsEmpty());
            if (queue != null)
            {
                var e = queue.Read();
                if (e.TypeId == EventType.OnQueueClosed)
                    unsyncedQueues.Remove(queue);
                return e;
            }

            DateTime minDateTime = DateTime.MaxValue;
            IEventQueue removedQueue = null;
            IEventQueue minDateTimeQueue = null;
            Event evt;
            foreach (var q in syncedQueues)
            {
                evt = q.Peek();
                if (evt.TypeId != EventType.OnQueueClosed || (evt as OnQueueClosed).Queue != q)
                {
                    minDateTime = minDateTime > evt.DateTime ? evt.DateTime : minDateTime;
                    minDateTimeQueue = q;
                }
                else
                {
                    removedQueue = q;
                    break;
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
