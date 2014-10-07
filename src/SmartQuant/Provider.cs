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

        public string Description { get { return this.description; } }

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
            throw new NotImplementedException();
        }

        public virtual void Subscribe(InstrumentList instruments)
        {
            foreach (Instrument instrument in instruments)
                this.Subscribe(instrument);
        }

        public void Unsubscribe(Instrument instrument)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Unsubscribe(InstrumentList instruments)
        {
            foreach (Instrument instrument in instruments)
                this.Unsubscribe(instrument);
        }

        public void Send(ExecutionCommand command)
        {
            throw new NotImplementedException();
        }

        public virtual void Process(Event e)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnConnected()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnDisconnected()
        {
            throw new NotImplementedException();
        }

        public virtual void Send(HistoricalDataRequest request)
        {
        }

        public virtual void Send(InstrumentDefinitionRequest request)
        {
        }

        public virtual void RequestHistoricalData(HistoricalDataRequest request)
        {
        }

        public virtual void RequestInstrumentDefinitions(InstrumentDefinitionRequest request)
        {
        }

        protected internal void EmitProviderError(ProviderError error)
        {
        }

        protected internal void EmitError(int id, int code, string text)
        {
        }

        protected internal void EmitError(string text)
        {
        }

        protected internal void EmitWarning(int id, int code, string text)
        {
        }

        protected internal void EmitWarning(string text)
        {
        }

        protected internal void EmitMessage(int id, int code, string text)
        {
        }

        protected internal void EmitMessage(string text)
        {
        }

        protected internal void EmitData(DataObject data)
        {
        }

        protected internal void EmitExecutionReport(ExecutionReport report)
        {
        }

        protected internal void EmitAccountData(AccountData data)
        {
        }

        protected internal void EmitHistoricalData(HistoricalData data)
        {
        }

        protected internal void EmitHistoricalDataEnd(HistoricalDataEnd end)
        {
        }

        protected internal void EmitHistoricalDataEnd(string requestId, RequestResult result, string text)
        {
        }

        protected internal void EmitInstrumentDefinition(InstrumentDefinition definition)
        {
        }

        protected internal void EmitInstrumentDefinitionEnd(InstrumentDefinitionEnd end)
        {
        }

        protected internal void EmitInstrumentDefinitionEnd(string requestId, RequestResult result, string text)
        {
        }

        protected internal virtual ProviderPropertyList GetProperties()
        {
            return null;
        }

        protected internal virtual void SetProperties(ProviderPropertyList properties)
        {
        }

        public override string ToString()
        {
            return string.Format("provider id = {0} ({1} {2} {3})", this.Id, this.Name, this.Description, this.Url);
        }
    }
}