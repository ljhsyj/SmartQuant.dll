// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;
using System.Collections;
using System;

namespace SmartQuant
{
    public class ProviderList : IEnumerable<IProvider>
	{
        public IProvider GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IProvider GetByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<IProvider> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
	}
}
