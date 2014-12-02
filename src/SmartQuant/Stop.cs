using System;

namespace SmartQuant
{
    public class Stop
    {
        protected internal Strategy strategy;
        protected internal Position position;
        protected internal Instrument instrument;
        protected internal bool connected;
        protected internal StopType type;
        protected internal StopMode mode;
        protected internal StopStatus status;
        protected internal double level;
        protected internal double initPrice;
        protected internal double currPrice;
        protected internal double stopPrice;
        protected internal double fillPrice;
        protected internal double trailPrice;
        protected internal double qty;
        protected internal PositionSide side;
        protected internal DateTime creationTime;
        protected internal DateTime completionTime;
        protected internal bool traceOnQuote;
        protected internal bool traceOnTrade;
        protected internal bool traceOnBar;
        protected internal bool traceOnBarOpen;
        protected internal bool trailOnOpen;
        protected internal bool trailOnHighLow;
        protected internal long filterBarSize;
        protected internal BarType filterBarType;
        protected internal StopFillMode fillMode;

        public Strategy Strategy
        {
            get
            {
                return this.strategy;
            }
        }

        public Position Position
        {
            get
            {
                return this.position;
            }
        }

        public Instrument Instrument
        {
            get
            {
                return this.instrument;
            }
        }

        public bool Connected
        {
            get
            {
                return this.connected;
            }
        }

        public StopType Type
        {
            get
            {
                return this.type;
            }
        }

        public StopMode Mode
        {
            get
            {
                return this.mode;
            }
        }

        public StopStatus Status
        {
            get
            {
                return this.status;
            }
        }

        public double Level
        {
            get
            {
                return this.level;
            }
        }

        public double Qty
        {
            get
            {
                return this.qty;
            }
        }

        public PositionSide Side
        {
            get
            {
                return this.side;
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return this.creationTime;
            }
        }

        public DateTime CompletionTime
        {
            get
            {
                return this.completionTime;
            }
        }

        public bool TraceOnQuote
        {
            get
            {
                return this.traceOnQuote;
            }
            set
            {
                this.traceOnQuote = value;
            }
        }

        public bool TraceOnTrade
        {
            get
            {
                return this.traceOnTrade;
            }
            set
            {
                this.traceOnTrade = value;
            }
        }

        public bool TraceOnBar
        {
            get
            {
                return this.traceOnBar;
            }
            set
            {
                this.traceOnBar = value;
            }
        }

        public bool TraceOnBarOpen
        {
            get
            {
                return this.traceOnBarOpen;
            }
            set
            {
                this.traceOnBarOpen = value;
            }
        }

        public bool TrailOnOpen
        {
            get
            {
                return this.trailOnOpen;
            }
            set
            {
                this.trailOnOpen = value;
            }
        }

        public bool TrailOnHighLow
        {
            get
            {
                return this.trailOnHighLow;
            }
            set
            {
                this.trailOnHighLow = value;
            }
        }

        public long FilterBarSize
        {
            get
            {
                return this.filterBarSize;
            }
            set
            {
                this.filterBarSize = value;
            }
        }

        public BarType FilterBarType
        {
            get
            {
                return this.filterBarType;
            }
            set
            {
                this.filterBarType = value;
            }
        }

        public StopFillMode FillMode
        {
            get
            {
                return this.fillMode;
            }
            set
            {
                this.fillMode = value;
            }
        }

        public Stop(Strategy strategy, Position position, double level, StopType type, StopMode mode)
        {
            throw new NotImplementedException(); 
        }

        public Stop(Strategy strategy, Position position, DateTime time)
        {
            throw new NotImplementedException(); 
        }

        public void Cancel()
        {
            throw new NotImplementedException(); 
        }

        protected virtual double GetPrice(double price)
        {
            return price;
        }

        protected virtual double GetInstrumentPrice()
        {
            throw new NotImplementedException(); 
        }

        protected virtual double GetStopPrice()
        {
            throw new NotImplementedException(); 
        }

        public void Disconnect()
        {
            throw new NotImplementedException(); 
        }
    }
}

