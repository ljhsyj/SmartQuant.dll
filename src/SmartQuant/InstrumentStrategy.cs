using System;

namespace SmartQuant
{
    public class InstrumentStrategy : Strategy
    {
        public InstrumentStrategy(Framework framework, string name)
            : base(framework, name)
        {
            this.raiseEvents = false;
        }
    }
}

