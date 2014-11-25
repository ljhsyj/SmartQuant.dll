using System;
using System.IO;

namespace SmartQuant
{
    public class FileInstrumentServer : InstrumentServer
    {
        private DataFile dataFile;
        private bool opened;

        public FileInstrumentServer(Framework framework, string fileName, string host = null) : base(framework)
        {
            if (host == null)
                this.dataFile = new DataFile(fileName, framework.StreamerManager);
            else
                ;
              //  this.dataFile =  new NetDataFile_(fileName, host, framework.streamerManager_0);

        }

        public override void Open()
        {
            if (this.opened)
                return;
            this.dataFile.Open(FileMode.OpenOrCreate);
            this.opened = true;
        }

        public override void Close()
        {
            if (!this.opened)
                return;
            this.dataFile.Close();
            this.opened = false;
        }

        public override void Flush()
        {
            this.dataFile.Flush();
        }

        public override InstrumentList Load()
        {
            this.instruments.Clear();
            foreach (var key in this.dataFile.Keys.Values)
            {
                if (key.TypeId == ObjectType.Instrument)
                    this.instruments.Add(this.dataFile.Get(key.name) as Instrument);
            }
            return this.instruments;
        }

        public override void Save(Instrument instrument)
        {
            this.dataFile.Write(instrument.Symbol, instrument);
        }

        public override void Delete(Instrument instrument)
        {
            this.dataFile.Delete(instrument.Symbol);
        }
    }
}

