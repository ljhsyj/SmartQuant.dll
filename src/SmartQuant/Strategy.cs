using System;

namespace SmartQuant
{
    public class Strategy
    {
        protected internal Framework framework;

        protected bool raiseEvents;

        protected LinkedList<Strategy> strategies;

        public byte Id { get; private set; }

        public string Name { get; private set; }

        public StrategyStatus Status { get; private set; }

        public StrategyMode Mode { get; private set; }

        public Strategy(Framework framework, string name)
        {
            this.framework = framework;
            this.Name = name;
        }

        public void AddInstruments(string[] symbols)
        {
            foreach (string symbol in symbols)
                this.AddInstrument(this.framework.InstrumentManager.Get(symbol));
        }

        public void AddInstruments(InstrumentList instruments)
        {
            foreach (Instrument instrument in instruments)
                this.AddInstrument(instrument);
        }

        public void AddInstrument(string symbol)
        {      
            this.AddInstrument(this.framework.InstrumentManager.Get(symbol));
        }

        public void AddInstrument(int id)
        {
            this.AddInstrument(this.framework.InstrumentManager.GetById(id));
        }

        public void AddInstrument(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public virtual void Init()
        {
        }

        public virtual double Objective()
        {
            throw new NotImplementedException();
        }

        public string GetStatusAsString()
        {
            switch (this.Status)
            {
                case StrategyStatus.Running:
                    return "Running";
                case StrategyStatus.Stopped:
                    return "Stopped";
                default:
                    return "Undefined";
            }
        }

        public string GetModeAsString()
        {
            switch (this.Mode)
            {
                case StrategyMode.Backtest:
                    return "Backtest";
                case StrategyMode.Paper:
                    return "Paper";
                case StrategyMode.Live:
                    return "Live";
                default:
                    return "Undefined";
            }
        }
    }
}
