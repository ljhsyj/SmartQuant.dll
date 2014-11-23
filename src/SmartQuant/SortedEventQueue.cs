// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class SortedEventQueue : IEventQueue
    {
        internal EventSortedSet events;
        internal DateTime dateTime;

        public byte Id { get; private set; }

        public byte Type { get; private set; }

        public bool IsSynched { get; set; }

        public string Name { get; private set; }

        public byte Priority { get; private set; }

        public long Count
        {
            get
            {
                return this.events.Count;
            }
        }

        public long EmptyCount
        {
            get
            {
                throw new NotImplementedException("Not implemented in SortedEventQueue");
            }
        }

        public long FullCount
        {
            get
            {
                throw new NotImplementedException("Not implemented in SortedEventQueue");
            }
        }

        public SortedEventQueue(byte id, byte type = EventQueueType.Master, byte priority = EventQueuePriority.Normal)
        {
            Id = id;
            Type = type;
            Priority = priority;
            this.events = new EventSortedSet();
        }

        public Event Peek()
        {
            lock (this)
                return this.events[0];
        }

        public DateTime PeekDateTime()
        {
            return this.dateTime;
        }

        public Event Read()
        {
            Event e;
            lock (this)
            {
                e = this.events.Pop();
                if (this.events.Count > 0)
                    this.dateTime = this.events[0].DateTime;
            }
            return e;
        }

        public void Write(Event e)
        {
            throw new NotImplementedException("Not implemented in SortedEventQueue");
        }

        public Event Dequeue()
        {
            return Read();
        }

        public void Enqueue(Event e)
        {
            lock (this)
            {
                this.events.Add(e);
                this.dateTime = this.events[0].DateTime;
            }    
        }

        public bool IsEmpty()
        {
            return this.events.Count == 0;
        }

        public bool IsFull()
        {
            return false;
        }

        public void Clear()
        {
            this.events.Clear();
        }

        public void ResetCounts()
        {
            // no-op
        }
    }
}
