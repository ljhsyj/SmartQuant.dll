// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class SortedEventQueue : IEventQueue
    {
        private EventSortedSet events;

        public byte Id { get; private set; }

        public byte Type { get; private set; }

        public bool IsSynched { get; set; }

        public string Name { get; private set; }

        public byte Priority { get; private set; }

        public SortedEventQueue(byte id, byte type = EventQueueType.Master, byte priority = EventQueuePriority.Normal)
        {
            Id = id;
            Type = type;
            Priority = priority;
            this.events = new EventSortedSet();
        }

        public Event Peek()
        {
            throw new NotImplementedException();
        }

        public DateTime PeekDateTime()
        {
            throw new NotImplementedException();
        }

        public Event Read()
        {
            throw new NotImplementedException();
        }

        public void Write(Event obj)
        {
            throw new NotImplementedException();
        }

        public Event Dequeue()
        {
            throw new NotImplementedException();
        }

        public void Enqueue(Event obj)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }



        public long Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public long FullCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public long EmptyCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

