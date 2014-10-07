// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class QuoteSeries : IEnumerable<Quote>
    {
        private string name;

        public QuoteSeries(string name = "")
        {
            this.name = name;
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DateTime FirstDateTime
        {
            get
            {
                throw new NotImplementedException();

            }
        }

        public DateTime LastDateTime
        {
            get
            {
                throw new NotImplementedException();

            }
        }

        public Quote this [int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Add(Quote quote)
        {
            throw new NotImplementedException();
        }

        public int GetIndex(DateTime datetime, IndexOption option)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Quote> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

