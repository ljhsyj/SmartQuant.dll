using System;
using System.IO;

namespace SmartQuant
{
    public class Leg
    {
        private Framework framework;
        private int instrumentId;
        internal Instrument instrument;

        public string Symbol
        {
            get
            {
                return this.instrument != null ? this.instrument.Symbol : null;
            }
            set
            {
                var i = this.framework.InstrumentManager[value];
                if (i != null)
                    Instrument = i;
                else
                    Console.WriteLine("Leg::Symbol Can not find instrument with such symbol in the framework instrument manager. Symbol = {0}", this.instrumentId);
            }
        }

        public Instrument Instrument
        {
            get
            {
                return this.instrument;
            }
            set
            {
                this.instrument = value;
                this.instrumentId = this.instrument.Id;
            }
        }

        public double Weight { get; set; }

        internal Leg(Framework framework)
        {
            this.framework = framework;
        }

        public Leg(Instrument instrument, double weight = 1)
        {
            Instrument = instrument;
            Weight = weight;
            this.framework = instrument.Framework;
        }

        internal void Init(Framework framework)
        {
            this.framework = framework;
            Instrument = framework.InstrumentManager.GetById(this.instrumentId);
            if (Instrument == null)
                Console.WriteLine("Leg::Init Can not find leg instrument in the framework instrument manager. Id = {0} ", this.instrumentId);
        }

        #region Extra

        internal Leg(BinaryReader reader)
        {
            var version = reader.ReadByte();
            this.instrumentId = reader.ReadInt32();
            Weight = reader.ReadDouble();
        }

        internal void Write(BinaryWriter writer)
        {
            byte version = 0;
            writer.Write(version);
            writer.Write(this.instrumentId);
            writer.Write(Weight);
        }

        #endregion
    }
}

