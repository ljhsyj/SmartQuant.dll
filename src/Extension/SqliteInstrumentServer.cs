using System;

namespace SmartQuant
{
    public class SqliteInstrumentServer : InstrumentServer
    {
        public SqliteInstrumentServer(Framework framework, string fileName, string host = null)
            : base(framework)
        {
        }

        public override void Open()
        {
        }

        public override void Close()
        {
        }

        public override void Flush()
        {
        }

        public override void Save(Instrument instrument)
        {
        }

        public override void Delete(Instrument instrument)
        {
        }
    }
}

