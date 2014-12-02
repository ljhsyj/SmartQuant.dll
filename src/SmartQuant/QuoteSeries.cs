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
        private List<Quote> quotes = new List<Quote>();

        public QuoteSeries(string name = "")
        {
            this.name = name;
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
                return this.quotes[Count - 1].DateTime;

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
            if (datetime < FirstDateTime)
                return option == IndexOption.Null || option == IndexOption.Prev ? -1 : 0;
            if (datetime > LastDateTime)
                return option == IndexOption.Null || option == IndexOption.Next ? -1 : Count - 1;
            var i = this.quotes.BinarySearch(new Quote() { DateTime = datetime }, new DataObjectComparer());
            if (i >= 0)
                return i;
            else if (option == IndexOption.Next)
                return ~i;
            else if (option == IndexOption.Prev)
                return ~i - 1;
            return -1; // option == IndexOption.Null
        }

        public IEnumerator<Quote> GetEnumerator()
        {
            return this.quotes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.quotes.GetEnumerator();
        }

        private void EnsureNotEmpty()
        {
            if (Count <= 0)
                throw new ApplicationException("Array has no elements");
        }
    }
}

