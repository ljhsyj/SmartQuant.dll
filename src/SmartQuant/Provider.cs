﻿using System;

namespace SmartQuant
{
	public class Provider : IProvider
	{
        private Framework framework;

        public Provider(Framework framework)
        {
            this.framework = framework;
        }
        public ProviderStatus Status
        {
            get { throw new NotImplementedException(); }
        }

        public byte Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
        public void Subscribe(Instrument instrument)
        {
            throw new System.NotImplementedException();
        }

        public void Subscribe(InstrumentList instrument)
        {
            throw new System.NotImplementedException();
        }

        public void Unsubscribe(Instrument instrument)
        {
            throw new System.NotImplementedException();
        }

        public void Unsubscribe(InstrumentList instrument)
        {
            throw new System.NotImplementedException();
        }

        public void Send(ExecutionCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}

