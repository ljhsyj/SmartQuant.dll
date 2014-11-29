// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Linq;

namespace SmartQuant
{
//    public class EventPipe
//    {
//        private Framework framework;
//        private System.Collections.Generic.LinkedList<IEventQueue> syncedQueues = new System.Collections.Generic.LinkedList<IEventQueue>();
//        private System.Collections.Generic.LinkedList<IEventQueue> unsyncedQueues = new System.Collections.Generic.LinkedList<IEventQueue>();
//
//        public int Count
//        {
//            get
//            {
//                return unsyncedQueues.Count;
//            }
//        }
//
//        public EventPipe(Framework framework)
//        {
//            this.framework = framework;
//        }
//
//        public void Add(IEventQueue queue)
//        {
//            if (queue.IsSynched)
//                syncedQueues.AddLast(queue);
//            else
//                unsyncedQueues.AddLast(queue);
//        }
//
//        public bool IsEmpty()
//        {
//            return unsyncedQueues.All(q => q.IsEmpty()) && syncedQueues.Any(q => q.IsEmpty());
//        }
//
//        public Event Read()
//        {
//            // Check the unsynced queues first.
//            var queue = unsyncedQueues.FirstOrDefault(q => !q.IsEmpty());
//            if (queue != null)
//            {
//                var e = queue.Read();
//                if (e.TypeId == EventType.OnQueueClosed)
//                    unsyncedQueues.Remove(queue);
//                return e;
//            }
//
//            DateTime minDateTime = DateTime.MaxValue;
//            IEventQueue removedQueue = null;
//            IEventQueue minDateTimeQueue = null;
//            Event evt;
//            foreach (var q in syncedQueues)
//            {
//                evt = q.Peek();
//                if (evt.TypeId != EventType.OnQueueClosed || (evt as OnQueueClosed).Queue != q)
//                {
//                    minDateTime = minDateTime > evt.DateTime ? evt.DateTime : minDateTime;
//                    minDateTimeQueue = q;
//                }
//                else
//                {
//                    removedQueue = q;
//                    break;
//                }
//            }
//
//            if (removedQueue != null)
//            {
//                syncedQueues.Remove(removedQueue);
//                if (syncedQueues.Count == 0 && this.framework.Mode == FrameworkMode.Simulation && removedQueue.Name != "Simulator stop queue")
//                {
//                    var newQueue = new EventQueue(EventQueueId.Data, EventQueueType.Master, EventQueuePriority.Normal, 16);
//                    newQueue.IsSynched = true;
//                    newQueue.Name = "Simulator stop queue";
//                    newQueue.Enqueue(new Event[] { new OnQueueOpened(newQueue), new OnSimulatorStop(), new OnQueueClosed(newQueue) });
//                    Add(newQueue);
//                }
//                return removedQueue.Read();
//            }
//            return minDateTimeQueue == null ? null : minDateTimeQueue.Read();
//        }
//
//        public Event Dequeue()
//        {
//            return null;
//        }
//
//        public void Clear()
//        {
//            syncedQueues.Clear();
//            unsyncedQueues.Clear();
//        }
//    }


    public class EventPipe
    {
        private Framework framework_0;
        private LinkedList<IEventQueue> linkedList_0=new LinkedList<IEventQueue>();
        private LinkedList<IEventQueue> linkedList_1=new LinkedList<IEventQueue>();

        public int Count
        {
            get
            {
                return this.linkedList_0.Count;
            }
        }
            
        public EventPipe(Framework framework)
        {
            this.framework_0 = framework;
        }

        public void Add(IEventQueue queue)
        {
            if (queue.IsSynched)
                this.linkedList_1.Add(queue);
            else
                this.linkedList_0.Add(queue);
        }

        public bool IsEmpty()
        {
            if (this.linkedList_0.Count != 0)
            {
                for (LinkedListNode<IEventQueue> linkedListNode = this.linkedList_0.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
                {
                    if (!linkedListNode.Data.IsEmpty())
                        return false;
                }
            }
            if (this.linkedList_1.Count == 0)
                return true;
            for (LinkedListNode<IEventQueue> linkedListNode = this.linkedList_1.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
            {
                if (linkedListNode.Data.IsEmpty())
                    return true;
            }
            return false;
        }

        public Event Read()
        {
            if (this.linkedList_0.Count != 0)
            {
                LinkedListNode<IEventQueue> linkedListNode1 = this.linkedList_0.First;
                LinkedListNode<IEventQueue> linkedListNode2 = (LinkedListNode<IEventQueue>) null;
                for (; linkedListNode1 != null; linkedListNode1 = linkedListNode1.Next)
                {
                    if (linkedListNode1.Data.IsEmpty())
                    {
                        linkedListNode2 = linkedListNode1;
                    }
                    else
                    {
                        Event @event = linkedListNode1.Data.Read();
                        if ((int) @event.TypeId == 206)
                        {
                            if (linkedListNode2 == null)
                                this.linkedList_0.First = linkedListNode1.Next;
                            else
                                linkedListNode2.Next = linkedListNode1.Next;
                            --this.linkedList_0.Count;
                        }
                        return @event;
                    }
                }
            }
            if (this.linkedList_1.Count == 0)
                return (Event) null;
            DateTime dateTime1 = DateTime.MaxValue;
            LinkedListNode<IEventQueue> linkedListNode3 = this.linkedList_1.First;
            LinkedListNode<IEventQueue> linkedListNode4 = (LinkedListNode<IEventQueue>) null;
            LinkedListNode<IEventQueue> linkedListNode5 = (LinkedListNode<IEventQueue>) null;
            for (; linkedListNode3 != null; linkedListNode3 = linkedListNode3.Next)
            {
                Event @event = linkedListNode3.Data.Peek();
                if ((int) @event.TypeId != 206 || ((OnQueueClosed) @event).Queue != linkedListNode3.Data)
                {
                    DateTime dateTime2 = @event.DateTime;
                    if (dateTime2 <= dateTime1)
                    {
                        linkedListNode5 = linkedListNode3;
                        dateTime1 = dateTime2;
                    }
                    linkedListNode4 = linkedListNode3;
                }
                else
                {
                    if (linkedListNode4 == null)
                        this.linkedList_1.First = linkedListNode3.Next;
                    else
                        linkedListNode4.Next = linkedListNode3.Next;
                    --this.linkedList_1.Count;

                    if (this.linkedList_1.Count == 0 && this.framework_0.Mode == FrameworkMode.Simulation && linkedListNode3.Data.Name != "Simulator stop queue")
                    {
                       // Console.WriteLine("q count:{0}", this.linkedList_1.Count);
                        EventQueue queue = new EventQueue((byte) 1, (byte) 0, (byte) 2, 10);
                        queue.IsSynched = true;
                        queue.Name = "Simulator stop queue";
                        queue.Enqueue((Event) new OnQueueOpened(queue));
                        queue.Enqueue((Event) new OnSimulatorStop());
                        queue.Enqueue((Event) new OnQueueClosed(queue));
                        Add(queue);
                    }
                    linkedListNode5 = linkedListNode3;
                    break;
                }
            }
            return linkedListNode5.Data.Read();
        }

        public Event Dequeue()
        {
            return (Event) null;
        }

        public void Clear()
        {
            this.linkedList_0.Clear();
            this.linkedList_1.Clear();
        }
    }

}
