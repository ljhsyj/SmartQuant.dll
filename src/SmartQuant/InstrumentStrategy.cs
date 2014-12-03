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
                return FindDataProvider(this, Instrument);
            }
            set
            {
                this.dataProvider = value;
                for (var node = Strategies.First; node != null; node = node.Next)
                    node.Data.DataProvider = this.dataProvider;
            }
        }

        public override IExecutionProvider ExecutionProvider
        {
            get
            {
                return FindExecutionProvider(Instrument);
            }
            set
            {
                this.executionProvider = value;
                for (var node = Strategies.First; node != null; node = node.Next)
                    node.Data.ExecutionProvider = this.executionProvider;
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
            throw new NotImplementedException();
        }

        public bool HasPosition()
        {
            return base.HasPosition(Instrument);
        }

        public bool HasPosition(PositionSide side, double qty)
        {
            return base.HasPosition(Instrument, side, qty);
        }

        public bool HasLongPosition()
        {
            return base.HasLongPosition(Instrument);
        }

        public bool HasLongPosition(double qty)
        {
            return base.HasLongPosition(this.Instrument, qty);
        }

        public bool HasShortPosition()
        {
            return base.HasShortPosition(this.Instrument);
        }

        public bool HasShortPosition(double qty)
        {
            return base.HasShortPosition(Instrument, qty);
        }
    }
}
