// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections;
using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class AccountDataFieldList : ICollection
    {
        private Dictionary<string, Dictionary<string, object>> acccountFields = new Dictionary<string, Dictionary<string, object>>();

        public int Count
        {
            get
            { 
                return acccountFields.Values.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            { 
                return null;
            }
        }

        public object this [string name, string currency]
        {
            get
            {
                Dictionary<string, object> logger;
                if (!acccountFields.TryGetValue(name, out logger))
                    return null;
                object value;
                logger.TryGetValue(currency, out value);
                return value;
            }
            internal set
            {
                Add(name, currency, value);
            }
        }

        public object this [string name]
        {
            get
            {
                return this[name, string.Empty];
            }
        }

        internal AccountDataFieldList()
        {
        }

        public void Add(string name, string currency, object value)
        {
            Dictionary<string, object> logger;
            if (!acccountFields.TryGetValue(name, out logger))
            {
                logger = new Dictionary<string, object>();
                acccountFields.Add(name, logger);
            }
            logger.Add(currency, value);
        }

        public void Add(string name, object value)
        {
            Add(name, string.Empty, value);
        }

        public void Clear()
        {
            acccountFields.Clear();
        }

        public AccountDataField[] ToArray()
        {
            var list = new List<AccountDataField>();
            foreach (var logger in acccountFields)
                foreach (var field in logger.Value)
                    list.Add(new AccountDataField(logger.Key, field.Key, field.Value));
            return list.ToArray();
        }

        public void CopyTo(Array array, int index)
        {
            ToArray().CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return ToArray().GetEnumerator();
        }
    }
}
