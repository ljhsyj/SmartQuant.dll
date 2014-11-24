using System;
using System.ComponentModel;

namespace SmartQuant
{
    public class Provider : IProvider
    {
        private ProviderStatus status;

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

        [Category("Information")]
        public byte Id { get { return id; } }

        [Category("Information")]
        public string Name { get { return name; } }

        [Category("Information")]
        public string Description { get { return this.description; } }

        [Category("Information")]
        public string Url { get { return this.url; } }

        [Category("Status")]
        public ProviderStatus Status
        { 
            get
            {
                return status;
            }
            protected set
            {
                if (status != value)
                {
                    status = value;
                    if (status == ProviderStatus.Connected)
                        this.OnConnected();
                    if (status == ProviderStatus.Disconnected)
                        this.OnDisconnected();

                    this.EmitProviderStatusChanged(this);
                }
            }
        }

        [Category("Status")]
        public bool IsConnected
        {
            get
            {
                return Status == ProviderStatus.Connected;
            }
        }

        [Category("Status")]
        public bool IsDisconnected
        {
            get
            {
                return Status == ProviderStatus.Disconnected;
            }
        }

        public Provider(Framework framework)
        {
            this.framework = framework;
            Status = ProviderStatus.Disconnected;
        }

        public virtual void Connect()
        {
            Status = ProviderStatus.Connecting;
            Status = ProviderStatus.Connected;
        }

        public bool Connect(int timeout)
        {
            throw new NotImplementedException();
        }

        public virtual void Disconnect()
        {
            Status = ProviderStatus.Disconnecting;
            Status = ProviderStatus.Disconnected;
        }

        public virtual void Subscribe(Instrument instrument)
        {
            // do nothing
        }

        public virtual void Subscribe(InstrumentList instruments)
        {
            foreach (Instrument instrument in instruments)
                this.Subscribe(instrument);
        }

        public virtual void Unsubscribe(Instrument instrument)
        {
            // do nothing
        }

        public virtual void Unsubscribe(InstrumentList instruments)
        {
            foreach (Instrument instrument in instruments)
                this.Unsubscribe(instrument);
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
            if (this is IDataProvider && this.dataQueue != null)
            {
                this.dataQueue.Enqueue(new OnQueueClosed(this.dataQueue));
                this.dataQueue = null;
            }
            if (this is IExecutionProvider && this.executionQueue != null)
            {
                this.executionQueue.Enqueue(new OnQueueClosed(this.executionQueue));
                this.executionQueue = null;  
            }
        }

        public virtual void Send(ExecutionCommand command)
        {
            // do nothing
        }

        public virtual void Send(HistoricalDataRequest request)
        {
            // do nothing
        }

        public virtual void Send(InstrumentDefinitionRequest request)
        {
            // do nothing
        }

        public virtual void RequestHistoricalData(HistoricalDataRequest request)
        {
            // do nothing
        }

        public virtual void RequestInstrumentDefinitions(InstrumentDefinitionRequest request)
        {
            // do nothing
        }

        protected internal void EmitProviderError(ProviderError error)
        {
            this.framework.EventServer.OnProviderError(error);
        }

        protected internal void EmitError(int id, int code, string text)
        {
            this.EmitProviderError(new ProviderError(this.framework.Clock.DateTime, ProviderErrorType.Error, this.id, id, code, text));
        }

        protected internal void EmitError(string text)
        {
            this.EmitError(-1, -1, text);
        }

        protected internal void EmitWarning(int id, int code, string text)
        {
            this.EmitProviderError(new ProviderError(this.framework.Clock.DateTime, ProviderErrorType.Warning, this.id, id, code, text));
        }

        protected internal void EmitWarning(string text)
        {
            this.EmitWarning(-1, -1, text);
        }

        protected internal void EmitMessage(int id, int code, string text)
        {
            this.EmitProviderError(new ProviderError(this.framework.Clock.DateTime, ProviderErrorType.Message, this.id, id, code, text));
        }

        protected internal void EmitMessage(string text)
        {
            this.EmitMessage(-1, -1, text);
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
            this.EmitHistoricalDataEnd(new HistoricalDataEnd(requestId, result, text));
        }

        protected internal void EmitInstrumentDefinition(InstrumentDefinition definition)
        {
        }

        protected internal void EmitInstrumentDefinitionEnd(InstrumentDefinitionEnd end)
        {
        }

        protected internal void EmitInstrumentDefinitionEnd(string requestId, RequestResult result, string text)
        {
            this.EmitInstrumentDefinitionEnd(new InstrumentDefinitionEnd(requestId, result, text));
        }

        private void EmitProviderStatusChanged(Provider provider)
        {
            this.framework.EventServer.OnProviderStatusChanged(provider);
        }

        protected internal virtual ProviderPropertyList GetProperties()
        {
            var props = new ProviderPropertyList();
            foreach (var info in GetType().GetProperties())
            {
                if (info.CanRead && info.CanWrite && info.DeclaringType != typeof(Provider))
                {
                    var converter = TypeDescriptor.GetConverter(info.PropertyType);
                    if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
                    {
                        object obj = info.GetValue(this, null);
                        props.SetValue(info.Name, converter.ConvertToInvariantString(obj));
                    }
                }
            }
            return props; 
        }

        protected internal virtual void SetProperties(ProviderPropertyList properties)
        {
            foreach (var info in GetType().GetProperties())
            {
                if (info.CanRead && info.CanWrite)
                {
                    var converter = TypeDescriptor.GetConverter(info.PropertyType);
                    if (converter != null && converter.CanConvertFrom(typeof(string)))
                    {
                        string val = properties.GetStringValue(info.Name, null);
                        if (val != null && converter.IsValid(val))
                            info.SetValue(this, converter.ConvertFromInvariantString(val), null);
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format("provider id = {0} ({1} {2} {3})", this.Id, this.Name, this.Description, this.Url);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
