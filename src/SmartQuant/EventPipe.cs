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
            throw new NotImplementedException();
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
