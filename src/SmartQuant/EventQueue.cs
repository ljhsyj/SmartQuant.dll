// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;

namespace SmartQuant
{
    // This implemetation is not thread-safe. Use carefully!
    public class EventQueue : IEventQueue
    {
        private volatile int readPosition;
        private volatile int writePosition;
        private Event[] events;

        public byte Id { get; private set; }

        public byte Type { get; private set; }

        public int Size { get; private set; }

        public bool IsSynched { get; set; }

        public string Name { get; private set; }

        public byte Priority { get; private set; }

        public long Count { get { return EnqueueCount - DequeueCount; } }

        public long EnqueueCount { get; private set; }

        public long DequeueCount { get; private set; }

        public long FullCount { get; private set; }

        public long EmptyCount { get; private set; }

        public EventQueue(byte id, byte type = EventQueueType.Master, byte priority = EventQueuePriority.Normal, int size = 100000)
        {
            Id = id;
            Type = type;
            Priority = priority;
            Size = size;
            this.events = new Event[Size];
        }

        public Event Peek()
        {
            return this.events[this.readPosition];
        }

        public DateTime PeekDateTime()
        {
            return Peek().DateTime;
        }

        public Event Read()
        {
            Event e = Peek();
            this.readPosition = (this.readPosition + 1) % Size;
            ++DequeueCount;
            return e;
        }

        public void Write(Event obj)
        {
            this.events[this.writePosition] = obj;
            this.writePosition = (this.writePosition + 1) % Size;
            ++EnqueueCount;
        }

        public Event Dequeue()
        {
            while (this.IsEmpty())
            {
                ++EmptyCount;
                Thread.Sleep(1);
            }
            return Read();
        }

        public void Enqueue(Event obj)
        {
            while (this.IsFull())
            {
                ++FullCount;
                Thread.Sleep(1);
            }
            Write(obj);
        }

        public bool IsEmpty()
        {
            return this.readPosition == this.writePosition;
        }

        public bool IsFull()
        {
            return (this.writePosition + 1) % Size == this.readPosition;
        }

        public void Clear()
        {
            this.readPosition = this.writePosition = 0;
            EmptyCount = FullCount = EnqueueCount = DequeueCount = 0;
        }

        public void ResetCounts()
        {
            FullCount = EmptyCount = 0;
        }

        public override string ToString()
        {
            return string.Format("Id: {0} Count = {1} Enqueue = {2} Dequeue = {3}", Id, Count, EnqueueCount, DequeueCount);
        }
    }
}
