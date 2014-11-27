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

        protected Framework framework;

        [Category("Information")]
        public byte Id
        { 
            get
            {
                return id;
            }
        }

        [Category("Information")]
        public string Name
        {
            get
            { return name; }
        }

        [Category("Information")]
        public string Description
        { 
            get
            { 
                return this.description;
            }
        }

        [Category("Information")]
        public string Url
        { 
            get
            { 
                return this.url;
            }
        }

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
                        OnConnected();
                    if (status == ProviderStatus.Disconnected)
                        OnDisconnected();

                    EmitProviderStatusChanged(this);
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
                Subscribe(instrument);
        }

        public virtual void Unsubscribe(Instrument instrument)
        {
            // do nothing
        }

        public virtual void Unsubscribe(InstrumentList instruments)
        {
            foreach (Instrument instrument in instruments)
                Unsubscribe(instrument);
        }

        public virtual void Process(Event e)
        {
            switch (e.TypeId)
            {
                case EventType.OnConnect:
                    Connect();
                    break;
                case EventType.OnDisconnect:
                    Disconnect();
                    break;
                case EventType.OnSubscribe:
                    Subscribe((e as OnSubscribe).Instrument);
                    break;
                case EventType.OnUnsubscribe:
                    Unsubscribe((e as OnUnsubscribe).Instrument);
                    break;
            }
        }

        protected virtual void OnConnected()
        {
            if (this is IDataProvider)
            {
                this.dataQueue = new EventQueue(EventQueueId.Data, EventQueueType.Master, EventQueuePriority.Normal, 16384);
                this.dataQueue.Enqueue(new OnQueueOpened(this.dataQueue));
                this.dataQueue.Name = string.Format("{0} data queue", this.name);
                this.framework.EventBus.DataPipe.Add(this.dataQueue);
            }
            if (this is IExecutionProvider)
            {
                this.executionQueue = new EventQueue(EventQueueId.Execution, EventQueueType.Master, EventQueuePriority.Normal, 16384);
                this.executionQueue.Enqueue(new OnQueueOpened(this.executionQueue));
                this.executionQueue.Name = string.Format("{0} execution queue", this.name);
                this.framework.EventBus.ExecutionPipe.Add(this.executionQueue);
            }
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
            Console.WriteLine("Provider::Send is not implemented in the base class");
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
            EmitProviderError(new ProviderError(this.framework.Clock.DateTime, ProviderErrorType.Error, this.id, id, code, text));
        }

        protected internal void EmitError(string text)
        {
            EmitError(-1, -1, text);
        }

        protected internal void EmitWarning(int id, int code, string text)
        {
            EmitProviderError(new ProviderError(this.framework.Clock.DateTime, ProviderErrorType.Warning, this.id, id, code, text));
        }

        protected internal void EmitWarning(string text)
        {
            EmitWarning(-1, -1, text);
        }

        protected internal void EmitMessage(int id, int code, string text)
        {
            EmitProviderError(new ProviderError(this.framework.Clock.DateTime, ProviderErrorType.Message, this.id, id, code, text));
        }

        protected internal void EmitMessage(string text)
        {
            EmitMessage(-1, -1, text);
        }

        protected internal void EmitData(DataObject data, bool queued = true)
        {
            if (queued)
                this.dataQueue.Enqueue(data);
            else
                this.framework.EventServer.OnData(data);
        }

        protected internal void EmitExecutionReport(ExecutionReport report, bool queued = true)
        {
            if (queued)
                this.executionQueue.Enqueue(report);
            else
                this.framework.EventServer.OnExecutionReport(report);
        }

        protected internal void EmitAccountData(AccountData data)
        {
            this.executionQueue.Enqueue(data);
        }

        protected internal void EmitHistoricalData(HistoricalData data)
        {
            EnsureHistoricalQueueOpened();
            this.historicalQueue.Enqueue(data);
        }

        protected internal void EmitHistoricalDataEnd(HistoricalDataEnd end)
        {
            EnsureHistoricalQueueOpened();
            this.historicalQueue.Enqueue(end);
            EnsureHistoricalQueueClosed();
        }

        protected internal void EmitHistoricalDataEnd(string requestId, RequestResult result, string text)
        {            
            EmitHistoricalDataEnd(new HistoricalDataEnd(requestId, result, text));
        }

        protected internal void EmitInstrumentDefinition(InstrumentDefinition definition)
        {
            EnsureInstrumentQueueOpened();
            this.instrumentQueue.Enqueue(new OnInstrumentDefinition(definition));
        }

        protected internal void EmitInstrumentDefinitionEnd(InstrumentDefinitionEnd end)
        {
            EnsureInstrumentQueueOpened();
            this.instrumentQueue.Enqueue(new OnInstrumentDefinitionEnd(end));
            EnsureInstrumentQueueClosed();
        }

        protected internal void EmitInstrumentDefinitionEnd(string requestId, RequestResult result, string text)
        {
            EmitInstrumentDefinitionEnd(new InstrumentDefinitionEnd(requestId, result, text));
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
                        var obj = info.GetValue(this, null);
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
                        var val = properties.GetStringValue(info.Name, null);
                        if (val != null && converter.IsValid(val))
                            info.SetValue(this, converter.ConvertFromInvariantString(val), null);
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format("provider id = {0} {1} ({2}) {3}", Id, Name, Description, Url);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private void EnsureHistoricalQueueOpened()
        {
            if (this.historicalQueue != null)
                return;
            var q = new EventQueue(EventQueueId.All, EventQueueType.Master, EventQueuePriority.Normal, 1024);
            q.Name = string.Format("{0} historical queue", this.name);
            q.Enqueue(new OnQueueOpened(q));
            this.historicalQueue = q;
            this.framework.EventBus.HistoricalPipe.Add(this.historicalQueue);
        }

        private void EnsureHistoricalQueueClosed()
        {
            if (this.historicalQueue == null)
                return;
            this.historicalQueue.Enqueue(new OnQueueClosed(this.historicalQueue));
            this.historicalQueue = null;
        }

        private void EnsureInstrumentQueueOpened()
        {
            if (this.instrumentQueue != null)
                return;
            var q = new EventQueue(EventQueueId.All, EventQueueType.Master, EventQueuePriority.Normal, 1024);
            q.Name = string.Format("{0} instrument queue", this.name);
            q.Enqueue(new OnQueueOpened(q));
            this.instrumentQueue = q;
            this.framework.EventBus.ServicePipe.Add(this.instrumentQueue);
        }

        private void EnsureInstrumentQueueClosed()
        {
            if (this.instrumentQueue == null)
                return;
            this.instrumentQueue.Enqueue(new OnQueueClosed(this.instrumentQueue));
            this.instrumentQueue = null;
        }
    }
}
