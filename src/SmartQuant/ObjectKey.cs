// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class ObjectKey : IComparable<ObjectKey>
	{
        protected internal bool changed;

        private string name;
        private object obj;
        private DataFile file;

        public byte TypeId { get; private set; }

        public ObjectKey()
        {
        }

        public ObjectKey(DataFile file, string name = null, object obj = null)
        {
            this.file = file;
            this.name = name;
            this.obj = obj;
        }

        public virtual object GetObject()
        {
            return null;
        }

        public virtual void Dump()
        {
        }

        public int CompareTo(ObjectKey other)
        {
            throw new NotImplementedException();
        }
	}
}

