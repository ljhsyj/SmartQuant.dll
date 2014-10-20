// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading.Tasks;

namespace SmartQuant
{
    public class IdArray<T>
    {
        private T[] array;
        private int size;
        private readonly int reserved;

        public int Size
        {
            get
            {
                return this.size;
            }
        }

        public T this[int id]
        {
            get
            {
                if (id >= this.size)
                    this.Resize(id);
                return this.array[id];
            }
            set
            {
                if (id >= this.size)
                    this.Resize(id);
                this.array[id] = value;
            }
        }

        public IdArray(int size = 1024)
        {
            this.size = size;
            this.reserved = size;
            this.array = new T[size];
        }

        public void Clear()
        {
            Parallel.ForEach(this.array, elem => elem = default(T));
        }

        public void Add(int id, T value)
        {
            if (id >= this.size)
                this.Resize(id);
            this.array[id] = value;
        }

        public void Remove(int id)
        {
            if (id >= this.size)
                this.Resize(id);
            this.array[id] = default(T);
        }

        private void Resize(int id)
        {
            Console.WriteLine("IdArray::Resize index = {0}", id);
            var length = id + this.reserved;
            Array.Resize(ref this.array, length);
            this.size = length;
        }

        public void CopyTo(IdArray<T> array)
        {
            Parallel.For(0, array.Size, i => array[i] = i > this.Size - 1 ? default(T) : this.array[i]);
        }
    }
}
