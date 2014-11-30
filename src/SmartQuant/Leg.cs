using System;

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
                var instrument = this.framework.InstrumentManager[value];
                if (instrument != null)
                {
                    this.instrument = instrument;
                    this.instrumentId = instrument.Id;
                }
                else
                    Console.WriteLine("Leg::Symbol Can not find instrument with such symbol in the framework instrument manager. Symbol = {0}", this.instrumentId);
            }
        }

        public Instrument Instrument { get; set; }

        public double Weight { get; set; }

        public Leg(Instrument instrument, double weight = 1)
        {
            Instrument = instrument;
            Weight = weight;
        }

        internal void Init(Framework framework)
        {
            this.framework = framework;
            Instrument = framework.InstrumentManager.GetById(this.instrumentId);
            if (Instrument == null)
                Console.WriteLine("Leg::Init Can not find leg instrument in the framework instrument manager. Id = {0} ", this.instrumentId);
        }
    }
}

