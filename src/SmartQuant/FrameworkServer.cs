// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class FrameworkServer : IDisposable
    {
        public Framework Framework { get; internal set; }

        public string ConnectionString { get; set; }
       
        protected FrameworkServer()
        {
        }

        public virtual void Open()
        {
        }

        public virtual void Close()
        {
        }

        public virtual void Flush()
        {
        }

        public virtual void Dispose()
        {
            Close();
        }

        protected string GetStringValue(string key, string defaultValue)
        {
            throw new NotImplementedException();
        }

        protected int GetInt32Value(string key, int defaultValue)
        {
            throw new NotImplementedException();
        }

        protected bool GetBooleanValue(string key, bool defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}

