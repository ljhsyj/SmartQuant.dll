// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

namespace SmartQuant
{
    class GetByList<T> : IEnumerable<T>
    {
        private static MethodInfo nameMethodInfo;
        private static MethodInfo idMethodInfo;

        private Dictionary<string, T> dictionary;
        private IdArray<T> array;
        private List<T> list;

        public int Count
        {
            get
            { 
                return this.list.Count; 
            }
        }

        static GetByList()
        {
            nameMethodInfo = typeof(T).GetMethod("GetName", BindingFlags.NonPublic | BindingFlags.Instance);
            idMethodInfo = typeof(T).GetMethod("GetId", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public GetByList()
        {
            this.dictionary = new Dictionary<string, T>();
            this.array = new IdArray<T>(8192);
            this.list = new List<T>();
        }

        public bool Contains(T obj)
        {
            string name = (string)((nameMethodInfo != null) ? nameMethodInfo.Invoke(obj, new object[0]) : null);
            return Contains(name);
        }

        public bool Contains(string name)
        {
            return this.dictionary.ContainsKey(name);
        }

        public bool Contains(int id)
        {
            return this.array[id] != null;
        }

        public void Add(T obj)
        {
            int id = (int)idMethodInfo.Invoke(obj, new object[0]);
            if (((T)this.array[id]).Equals(default(T)))
            {
                this.list.Add(obj);
                string name = (string)((nameMethodInfo != null) ? nameMethodInfo.Invoke(obj, new object[0]) : null);
                if (name != null)
                    this.dictionary[name] = obj;
                this.array[id] = obj;
            }
            else
                Console.WriteLine("GetByList::Add Object with id = {0} is already in the list", id);

        }
        public void Remove(int id)
        {
        }
        public void Remove(T obj)
        {
            string name = (string)((nameMethodInfo != null) ? nameMethodInfo.Invoke(obj, new object[0]) : null);
            int id = (int)idMethodInfo.Invoke(obj, new object[0]);
            this.list.Remove(obj);
            if (name != null)
                this.dictionary.Remove(name);
            this.array.Remove(id);
        }

        public T GetByName(string name)
        {
            T obj;
            this.dictionary.TryGetValue(name, out obj);
            return obj;
        }

        public T GetByIndex(int index)
        {
            return this.list[index];
        }

        public T GetById(int id)
        {
            return this.array[id];
        }

        public void Clear()
        {
            this.dictionary.Clear();
            this.array.Clear();
            this.list.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}
