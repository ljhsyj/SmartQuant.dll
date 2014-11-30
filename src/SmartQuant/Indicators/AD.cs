using System;

namespace SmartQuant
{
    public class AD : Indicator
    {
        public AD(ISeries input)
            : base(input)
        {
            Init();
        }

        protected override void Init()
        {
            Name = "AD";
            Description = "Accumulation/Distribution";
            Clear();
            this.calculate = true;
        }

        public override void Calculate(int index)
        {
            throw new NotImplementedException();
        }

        public static double Value(ISeries input, int index)
        {
            throw new NotImplementedException();
        }
    }
}

