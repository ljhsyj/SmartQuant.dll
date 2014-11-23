
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SmartQuant
{
    public class Framework : IDisposable
    {
        private bool disposed;

        private bool isDisposable;

        private FrameworkMode mode;

        public Clock Clock { get; private set; }

        public Clock ExchangeClock { get; private set; }

        public static Framework Current { get; private set; }

        public string Name { get; private set; }

        public bool IsExternalDataQueue { get; set; }

        public bool IsDisposable
        {
            get
            {
                return isDisposable;
            }
            set
            {
                isDisposable = true;
            }
        }

        public Configuration Configuration { get; private set; }

        public EventBus EventBus { get; private set; }

        public EventServer EventServer { get; private set; }

        public EventManager EventManager { get; private set; }

        public InstrumentServer InstrumentServer { get; private set; }

        public InstrumentManager InstrumentManager { get; private set; }

        public DataServer DataServer { get; private set; }

        public DataManager DataManager { get; private set; }

        public ProviderManager ProviderManager { get; private set; }

        public OrderServer OrderServer { get; private set; }

        public OrderManager OrderManager { get; private set; }

        public PortfolioManager PortfolioManager { get; private set; }

        public StatisticsManager StatisticsManager { get; private set; }

        public StrategyManager StrategyManager { get; private set; }

        public CurrencyConverter CurrencyConverter { get; set; }

        public GroupManager GroupManager { get; private set; }

        public GroupDispatcher GroupDispatcher { get; set; }

        public StreamerManager StreamerManager { get; private set; }

        public DataFileManager DataFileManager { get; private set; }

        public SubscriptionManager SubscriptionManager { get; private set; }

        public FrameworkMode Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                if (this.mode == value)
                    return;
                ProviderManager.DisconnectAll();
                this.mode = value;
                if (this.mode == FrameworkMode.Simulation)
                {
                    Clock.Mode = ClockMode.Simulation;
                    EventBus.Mode = EventBusMode.Simulation;
                }
                else if (this.mode == FrameworkMode.Realtime)
                {
                    Clock.Mode = ClockMode.Realtime;
                    EventBus.Mode = EventBusMode.Realtime;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        public Framework(string name, EventBus externalBus, InstrumentServer instrumentServer, DataServer dataServer = null)
        {
            this.Init(name, false, externalBus, instrumentServer, dataServer);
        }

        public Framework(string name, InstrumentServer instrumentServer, DataServer dataServer)
        {
            this.Init(name, false, null, instrumentServer, dataServer);
        }

        public Framework(string name = "", bool createServers = true)
        {
            this.Init(name, createServers, null, null, null);
        }

        private void Init(string name, bool createServers, EventBus externalBus, InstrumentServer instrumentServer, DataServer dataServer)
        {
            this.isDisposable = true;
            Name = name;
            IsExternalDataQueue = true;
            LoadConfiguration();
            Mode = FrameworkMode.Simulation;
            EventBus = new EventBus(this, EventBusMode.Simulation);
            Clock = new Clock(this, ClockType.Local, ClockMode.Simulation, false);
            EventBus.LocalClockQueue = Clock.Queue;
            ExchangeClock = new Clock(this, ClockType.Exchange, ClockMode.Simulation, false);
            EventBus.ExchangeClockQueue = ExchangeClock.Queue;
            if (externalBus != null)
                externalBus.Attach(EventBus);
            EventServer = new EventServer(this, EventBus);
            EventManager = new EventManager(this, EventBus);
            InstrumentServer = createServers ? (!Configuration.IsInstrumentFileLocal ? new FileInstrumentServer(this, "instruments.quant", Configuration.InstrumentFileHost) : new FileInstrumentServer(this, Configuration.InstrumentFileName, null)) : instrumentServer;
            InstrumentManager = new InstrumentManager(this, InstrumentServer);
            DataServer = createServers ? (!Configuration.IsDataFileLocal ? new FileDataServer(this, "data.quant", Configuration.DataFileHost) : new FileDataServer(this, Configuration.DataFileName, null)) : dataServer;
            DataManager = new DataManager(this, DataServer);
            StreamerManager = new StreamerManager();
            LoadStreamerPlugins();
            ProviderManager = new ProviderManager(this);
            LoadProviderPlugins();
            OrderManager = new OrderManager(this, null);
            PortfolioManager = new PortfolioManager(this);
            StatisticsManager = new StatisticsManager(this);
            StrategyManager = new StrategyManager(this);
            GroupManager = new GroupManager(this);
            CurrencyConverter = new CurrencyConverter(this);
            DataFileManager = new DataFileManager(Installation.DataDir.FullName);
            SubscriptionManager = new SubscriptionManager(this);
            Framework.Current = Framework.Current ?? this;
        }

        ~Framework()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;
                
            if (disposing)
            {
                this.SaveConfiguration();

                // EventManager has its inner thread running,
                // this let it exit gracefully
                if (EventManager != null)
                    EventManager.Close();
            }
            disposed = true;
        }
            
        private void LoadConfiguration()
        {
            string path = Path.Combine(Installation.ConfigDir.FullName, "configuration.xml");
            string text = File.Exists(path) ? File.ReadAllText(path) : "";
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
                this.Configuration = new XmlSerializer(typeof(Configuration)).Deserialize(stream) as Configuration;
        }

        private void SaveConfiguration()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(Installation.ConfigDir.FullName, "configuration.xml")))
                new XmlSerializer(typeof(Configuration)).Serialize(writer, this.Configuration);
        }

        public void Clear()
        {
            this.StrategyManager.Clear();
        }

        private void LoadProviderPlugins()
        {
            foreach (ProviderPlugin plugin in this.Configuration.Providers)
            {
                Type type = Type.GetType(plugin.TypeName);
                IProvider provider = (Provider)Activator.CreateInstance(type);
                this.ProviderManager.AddProvider(provider);
            }
        }

        private void LoadStreamerPlugins()
        {
            foreach (StreamerPlugin plugin in this.Configuration.Streamers)
            {
                Type type = Type.GetType(plugin.TypeName);
                ObjectStreamer streamer = (ObjectStreamer)Activator.CreateInstance(type);
                this.StreamerManager.Add(streamer);
            }
        }
    }
}