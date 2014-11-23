// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;

namespace SmartQuant
{
    public class EventSortedSet : IEnumerable
    {
        private List<Event> events = new List<Event>();

        public string Name { get; set; }

        public string Description { get; set; }

        public EventSortedSet()
        {
        }

        public int Count
        {
            get
            {
                return this.events.Count;
            }
        }

        public Event this [int index]
        {
            get
            {
                return this.events[index];
            }
        }

        public void Add(Event e)
        {
            // Don't care what finding algorithm it uses at the moment.
            var i = this.events.FindIndex(new Predicate<Event>(evt => evt.DateTime > e.DateTime));
            if (i == -1)
                this.events.Add(e);
            else
                this.events.Insert(i, e);
        }

        public void Clear()
        {
            this.events.Clear();
        }

        internal Event Pop()
        {
            var e = this[0];
            this.events.RemoveAt(0);
            return e;
        }

        public IEnumerator GetEnumerator()
        {
            return this.events.GetEnumerator();
        }
    }
}
