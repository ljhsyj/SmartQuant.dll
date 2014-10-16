// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant.DriverFile
{
    public sealed class Key
    {
        private Dictionary<string, List<object>> dictionary_0;
        private List<string> list_0;

        public static Key Empty
        {
            get
            {
                return new Key();
            }
        }

        public List<object> this[string key]
        {
            get
            {
                if (!this.dictionary_0.ContainsKey(key))
                    return (List<object>) null;
                else
                    return this.dictionary_0[key];
            }
            set
            {
                if (!this.dictionary_0.ContainsKey(key))
                {
                    this.dictionary_0.Add(key, value);
                    this.list_0.Add(key);
                }
                else
                    this.dictionary_0[key] = value;
            }
        }

        public Key()
        {
            this.dictionary_0 = new Dictionary<string, List<object>>();
            this.list_0 = new List<string>();
        }

        public List<string> GetKeyNameList()
        {
            List<string> list = new List<string>(this.list_0.Count);
            for (int index = 0; index < this.list_0.Count; ++index)
                list.Add(this.list_0[index]);
            return list;
        }

        public void Clear()
        {
            this.dictionary_0.Clear();
            this.list_0.Clear();
        }
    }
}

