using System;

namespace SmartQuant
{
    public class InstrumentStrategy : Strategy
    {
        public Instrument Instrument { get; private set; }

        public Position Position { get; private set; }

        public override IDataProvider DataProvider
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override IExecutionProvider ExecutionProvider
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public InstrumentStrategy(Framework framework, string name)
            : base(framework, name)
        {
            this.raiseEvents = false;
        }

        public override void Init()
        {
            base.Init();
        }

        public void AddInstance(Instrument instrument, InstrumentStrategy strategy)
        {
        }
    }
}

