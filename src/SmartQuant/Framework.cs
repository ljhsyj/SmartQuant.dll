
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SmartQuant
{
    public class Framework : IDisposable
    {
        private bool disposed;

        private FrameworkMode mode;

        public Clock Clock {   get { throw new NotImplementedException();} }

        public static Framework Current { get; private set; }

        public string Name { get; private set; }

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

        public StreamerManager StreamerManager { get; private set; }

        public DataFileManager DataFileManager { get; private set; }

        public FrameworkMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                throw new NotImplementedException();
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
            this.Name = name;
            this.LoadConfiguration();
//            this.Mode = FrameworkMode.Simulation;
            this.EventBus = new EventBus(this, EventBusMode.Simulation);
            if (externalBus != null)
                externalBus.Attach(this.EventBus);
            this.EventServer = new EventServer(this, this.EventBus);
            this.EventManager = new EventManager(this, this.EventBus);
            this.InstrumentServer = createServers ? (!this.Configuration.IsInstrumentFileLocal ? new FileInstrumentServer(this, "instruments.quant", this.Configuration.InstrumentFileHost) : new FileInstrumentServer(this, this.Configuration.InstrumentFileName, null)) : instrumentServer;
            this.InstrumentManager = new InstrumentManager(this, this.InstrumentServer);
            this.DataServer = createServers ? (!this.Configuration.IsDataFileLocal ? new FileDataServer(this, "data.quant", this.Configuration.DataFileHost) : new FileDataServer(this, this.Configuration.DataFileName, null)) : dataServer;
            this.DataManager = new DataManager(this, this.DataServer);
            this.StreamerManager = new StreamerManager();
            this.LoadStreamerPlugins();
            this.ProviderManager = new ProviderManager(this);
            this.LoadProviderPlugins();
            this.OrderManager = new OrderManager(this, null);
            this.PortfolioManager = new PortfolioManager(this);
            this.StatisticsManager = new StatisticsManager(this);
            this.StrategyManager = new StrategyManager(this);
            this.GroupManager = new GroupManager(this);
            this.CurrencyConverter = new CurrencyConverter(this);
            this.DataFileManager = new DataFileManager(Installation.DataDir.FullName);
            Framework.Current = Framework.Current == null ? this : Framework.Current;
        }

        ~Framework()
        {
            this.Dispose(false);
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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.SaveConfiguration();
                }
                this.disposed = true;
            }
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
