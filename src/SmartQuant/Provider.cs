using System;

namespace SmartQuant
{
    public class Provider : IProvider
    {
        protected const string CATEGORY_INFO = "Information";
        protected const string CATEGORY_STATUS = "Status";
        protected byte id;
        protected string name;
        protected string description;
        protected string url;

        protected EventQueue dataQueue;
        protected EventQueue executionQueue;
        protected EventQueue historicalQueue;
        protected EventQueue instrumentQueue;

        private Framework framework;

        public Provider(Framework framework)
        {
            this.framework = framework;
        }

        public ProviderStatus Status { get; protected set; }

        public byte Id { get { return id; } }

        public string Name { get { return name; } }

        public string Descriptionv { get { return this.description; } }

        public string Url { get { return this.url; } }

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