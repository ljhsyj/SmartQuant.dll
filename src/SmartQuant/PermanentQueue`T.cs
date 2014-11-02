// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class PermanentQueue<T>
    {
        private List<T> list = new List<T>();
        private Dictionary<object, int> readerIndices = new Dictionary<object, int>();

        public void Enqueue(T item)
        {
            lock (this.list)
                this.list.Add(item);
        }

        public T[] DequeueAll(object reader)
        {
            lock (this.list)
            {
                int index = this.readerIndices[reader];
                if (this.list.Count < index + 1)
                    return null;
                var newList = new T[this.list.Count - index];
                this.list.CopyTo(index, newList, 0, newList.Length);
                this.readerIndices[reader] = index + newList.Length;
                return newList;
            }
        }

        public void AddReader(object reader)
        {
            lock (this.list)
                this.readerIndices[reader] = 0;
        }

        public void RemoveReader(object reader)
        {
            lock (this.list)
                this.readerIndices.Remove(reader);
        }

        public int Count(int startIndex)
        {
            lock (this.list)
                return this.list.Count - startIndex;
        }

        public void Clear()
        {
            lock (this.list)
            {
                foreach (object reader in this.readerIndices.Keys)
                    this.readerIndices[reader] = 0;
                this.list.Clear();
            }
        }
    }
}
