using System;

namespace SmartQuant
{
    public class Leg
    {
        private Framework framework;
        private int instrumentId;

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

