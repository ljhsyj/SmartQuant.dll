// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class LinkedList<T> :  IEnumerable<T>
    {
        private System.Collections.Generic.LinkedList<T> list = new System.Collections.Generic.LinkedList<T>();

        public int Count;

        public LinkedListNode<T> First;

        public void Add(T data)
        {
            list.AddLast(data);
            First = new SmartQuant.LinkedListNode<T>(list.First.Value);
            ++Count;
        }

        public void Remove(T data)
        {
            bool newFirst = list.First.Value.Equals(data);
            list.Remove(data);
            if (newFirst)
                First = new SmartQuant.LinkedListNode<T>(list.First.Value);
            --Count;
        }

        public void Clear()
        {
            list.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
