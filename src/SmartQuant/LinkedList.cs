// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
	public class LinkedList<T>
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
	}
}
