// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class LinkedList<T> : IEnumerable<T>
	{
        public int Count;

        public LinkedListNode<T> First;

        // TODO: Need Test!!!
        public void Add(T data)
        {
            LinkedListNode<T> lastNode = this.First;
            while (lastNode != null)
            {
                if (lastNode.Data.Equals(data))
                    return;
                if (lastNode.Next == null)
                    break;
                lastNode = lastNode.Next;
            }

            var newNode = new LinkedListNode<T>(data);
            ++this.Count;
            if (lastNode == null)
                lastNode = newNode;
            else
                lastNode.Next = newNode;
        }

        public void Remove(T data)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            this.First = null;
            this.Count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
//            return new Class37<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

//        private class Class37<T1> :  IEnumerator<T1>, IDisposable
//        {
//            private LinkedList<T1> linkedList_0;
//            private LinkedListNode<T1> linkedListNode_0;
//            private int int_0;
//
//            public T1 Current
//            {
//                get
//                {
//                    return this.linkedListNode_0.Data;
//                }
//            }
//
//            object IEnumerator.Current
//            {
//                get
//                {
//                    return this.Current;
//                }
//            }
//
//            public Class37(LinkedList<T1> list)
//            {
//                this.linkedList_0 = list;
//                this.Reset();
//            }
//
//            public void Dispose()
//            {
//            }
//
//            public bool MoveNext()
//            {
//                if (this.int_0 >= this.linkedList_0.Count)
//                    return false;
//                this.linkedListNode_0 = this.int_0 == 0 ? this.linkedList_0.First : this.linkedListNode_0.Next;
//                ++this.int_0;
//                return true;
//            }
//
//            public void Reset()
//            {
//                this.linkedListNode_0 = null;
//                this.int_0 = 0;
//            }
//        }
    }
}
