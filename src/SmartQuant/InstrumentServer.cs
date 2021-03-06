﻿// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public abstract class InstrumentServer : IDisposable
    {
        protected Framework framework;
        protected InstrumentList instruments = new InstrumentList();

        public InstrumentServer(Framework framework)
        {
            this.framework = framework;
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

        public virtual InstrumentList Load()
        {
            return this.instruments;
        }

        public virtual void Save(Instrument instrument)
        {
        }

        public virtual void Delete(Instrument instrument)
        {
        }

        public void Dispose()
        {
            Close();
        }
    }
}
