// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

namespace SmartQuant
{
    public class ProviderList : IEnumerable<IProvider>
    {
        private List<IProvider> providers = new List<IProvider>();

        public int Count
        {
            get
            {
                return providers.Count;
            }
        }

        public void Add(IProvider provider)
        {
            if (GetById(provider.Id) == null && GetByName(provider.Name) == null)
                this.providers.Add(provider);
        }

        public void Remove(IProvider provider)
        {
            this.providers.Remove(provider);
        }

        public IProvider GetById(int id)
        {
            return providers.FirstOrDefault(p => p.Id == id);
        }

        public IProvider GetByName(string name)
        {
            return providers.FirstOrDefault(p => p.Name == name);
        }

        public IProvider GetByIndex(int index)
        {
            return providers[index];
        }

        public void Clear()
        {
            this.providers.Clear();
        }

        public IEnumerator<IProvider> GetEnumerator()
        {
            return this.providers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.providers.GetEnumerator();
        }
    }
}
