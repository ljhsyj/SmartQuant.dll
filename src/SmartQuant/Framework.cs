
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SmartQuant
{
    public class Framework : IDisposable
    {
        private static Framework framework;
        private bool disposed;
        private bool isDisposable;

        private FrameworkMode mode;

        public Clock Clock { get; private set; }

        public Clock ExchangeClock { get; private set; }

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
        public AccountDataManager AccountDataManager { get; private set; }

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

        public EventLoggerManager EventLoggerManager { get; private set; }

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

        public static Framework Current
        {
            get
            {
                if (framework == null)
                    framework = new Framework("", true);
                return framework;
            }
        }

        public IExecutionProvider ExecutionProvider
        {
            get
            {
                return ProviderManager.GetExecutionProvider(Configuration.DefaultExecutionProvider);
            }
        }

        public IDataProvider DataProvider
        {
            get
            {
                return ProviderManager.GetDataProvider(Configuration.DefaultDataProvider);
            }
        }

        public Framework(string name, EventBus externalBus, InstrumentServer instrumentServer, DataServer dataServer = null)
        {
            Init(name, false, externalBus, instrumentServer, dataServer);
        }

        public Framework(string name, InstrumentServer instrumentServer, DataServer dataServer)
        {
            Init(name, false, null, instrumentServer, dataServer);
        }

        public Framework(string name = "", bool createServers = true)
        {
            Init(name, createServers, null, null, null);
        }

        private void Init(string name, bool createServers, EventBus externalBus, InstrumentServer instrumentServer, DataServer dataServer)
        {
            this.isDisposable = true;
            Name = name;
            IsExternalDataQueue = true;
            LoadConfiguration();
            Mode = FrameworkMode.Simulation;
            EventBus = new EventBus(this, EventBusMode.Simulation);
            Clock = new Clock(this, ClockType.Local);
            EventBus.LocalClockQueue = Clock.Queue;
            ExchangeClock = new Clock(this, ClockType.Exchange);
            EventBus.ExchangeClockQueue = ExchangeClock.Queue;
            if (externalBus != null)
                externalBus.Attach(EventBus);
            EventServer = new EventServer(this, EventBus);
            EventManager = new EventManager(this, EventBus);

            // StreamerManager should be created early.
            StreamerManager = new StreamerManager();
            LoadStreamerPlugins();
            InstrumentServer = createServers ? (Configuration.IsInstrumentFileLocal ? new FileInstrumentServer(this, Configuration.InstrumentFileName) : new FileInstrumentServer(this, "instruments.quant", Configuration.InstrumentFileHost)) : instrumentServer;
            InstrumentManager = new InstrumentManager(this, InstrumentServer);
            InstrumentManager.Load();
            DataServer = createServers ? (Configuration.IsDataFileLocal ? new FileDataServer(this, Configuration.DataFileName) : new FileDataServer(this, "data.quant", Configuration.DataFileHost)) : dataServer;
            DataManager = new DataManager(this, DataServer);
            ProviderManager = new ProviderManager(this);
            LoadProviderPlugins();
            EventLoggerManager = new EventLoggerManager();
            SubscriptionManager = new SubscriptionManager(this);
            OrderManager = new OrderManager(this, null);
            PortfolioManager = new PortfolioManager(this);
            StatisticsManager = new StatisticsManager(this);
            StrategyManager = new StrategyManager(this);
            GroupManager = new GroupManager(this);
            AccountDataManager = new AccountDataManager(this);
            CurrencyConverter = new CurrencyConverter(this);
            DataFileManager = new DataFileManager(Installation.DataDir.FullName);
            framework = framework ?? this;
        }

        ~Framework()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            if (this.isDisposable)
            {
                Console.WriteLine("Framework::Dispose {0}", Name);
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            else
                Console.WriteLine("Framework::Dispose Framework is not disposable {0}", Name);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;
                
            if (disposing)
            {
                SaveConfiguration();

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
            if (string.IsNullOrEmpty(text))
                Configuration = Configuration.DefaultConfiguaration();
            else
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
                    Configuration = new XmlSerializer(typeof(Configuration)).Deserialize(stream) as Configuration;
        }

        private void SaveConfiguration()
        {
            using (var writer = new StreamWriter(Path.Combine(Installation.ConfigDir.FullName, "configuration.xml")))
                new XmlSerializer(typeof(Configuration)).Serialize(writer, Configuration);
        }

        public void Clear()
        {
            Console.WriteLine("{0} Framework::Clear", DateTime.Now);
            Clock.Clear();
            ExchangeClock.Clear();
            EventBus.Clear();
            EventManager.Clear();
            ProviderManager.DisconnectAll();
            ProviderManager.Clear();
            InstrumentManager.Clear();
            DataManager.Clear();
            SubscriptionManager.Clear();
            OrderManager.Clear();
            PortfolioManager.Clear();
            StrategyManager.Clear();
            AccountDataManager.Clear();
            GroupManager.Clear();
            EventServer.OnFrameworkCleared(this);
            GC.Collect();
        }

        private void LoadProviderPlugins()
        {
            foreach (var plugin in Configuration.Providers)
            {
                var type = Type.GetType(plugin.TypeName);
                var provider = (Provider)Activator.CreateInstance(type);
                ProviderManager.AddProvider(provider);
            }
        }

        private void LoadStreamerPlugins()
        {
            foreach (var plugin in Configuration.Streamers)
            {
                var type = Type.GetType(plugin.TypeName);
                var streamer = (ObjectStreamer)Activator.CreateInstance(type);
                StreamerManager.Add(streamer);
            }
        }

        public void Load(BinaryReader reader)
        {
            Name = reader.ReadString();
            DataManager.Load(reader);
            SubscriptionManager.Load(reader);
            PortfolioManager.Load(reader);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Name);
            DataManager.Save(writer);
            SubscriptionManager.Save(writer);
            PortfolioManager.Save(writer);
        }

        public void Dump()
        {
            Console.WriteLine("Framework {0}", Name);
            DataManager.Dump();
        }
    }
}