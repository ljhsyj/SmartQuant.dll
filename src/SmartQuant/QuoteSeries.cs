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
        private List<Quote> quotes;

        public QuoteSeries(string name = "")
        {
            this.name = name;
            this.quotes = new List<Quote>();
        }

        public int Count
        {
            get
            {
                return this.quotes.Count;
            }
        }

        public DateTime FirstDateTime
        {
            get
            {
                EnsureNotEmpty();
                return this.quotes[0].DateTime;

            }
        }

        public DateTime LastDateTime
        {
            get
            {
                EnsureNotEmpty();
                return this.quotes[this.Count - 1].DateTime;

            }
        }

        public Quote this[int index]
        {
            get
            {
                return this.quotes[index];
            }
        }

        public void Clear()
        {
            this.quotes.Clear();
        }

        public void Add(Quote quote)
        {
            this.quotes.Add(quote);
        }

        public int GetIndex(DateTime datetime, IndexOption option)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Quote> GetEnumerator()
        {
            return  this.quotes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.quotes.GetEnumerator();
        }

        private void EnsureNotEmpty()
        {
            if (this.Count <= 0)
                throw new ApplicationException("Array has no elements");
        }
    }
}

