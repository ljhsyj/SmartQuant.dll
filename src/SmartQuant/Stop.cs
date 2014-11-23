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
        }

        public Stop(Strategy strategy, Position position, DateTime time)
        {
        }

        public void Cancel()
        {
        }

        protected virtual double GetPrice(double price)
        {
            return price;
        }

        protected virtual double GetInstrumentPrice()
        {
//            if (this.position.Side == PositionSide.Long)
//            {
//                Bid bid = this.strategy.framework.alThAjnhqO.GetBid(this.instrument);
//                if (bid != null)
//                    return this.GetPrice(bid.Price);
//            }
//            if (this.position.Side == PositionSide.Short)
//            {
//                Ask ask = this.strategy.framework.alThAjnhqO.GetAsk(this.instrument);
//                if (ask != null)
//                    return this.GetPrice(ask.Price);
//            }
//            Trade trade = this.strategy.framework.alThAjnhqO.GetTrade(this.instrument);
//            if (trade != null)
//                return this.GetPrice(trade.Price);
//            Bar bar = this.strategy.framework.alThAjnhqO.GetBar(this.instrument);
//            if (bar != null)
//                return this.GetPrice(bar.Close);
//            else
                return 0.0;
        }

        protected virtual double GetStopPrice()
        {
            return 0;
//            this.initPrice = this.trailPrice;
//            switch (this.mode)
//            {
//                case StopMode.Absolute:
//                    switch (this.side)
//                    {
//                        case PositionSide.Long:
//                            return this.trailPrice - Math.Abs(this.level);
//                        case PositionSide.Short:
//                            return this.trailPrice + Math.Abs(this.level);
//                        default:
//                            throw new ArgumentException("Unknown position side : " + (object) this.position.Side);
//                    }
//                case StopMode.Percent:
//                    switch (this.position.Side)
//                    {
//                        case PositionSide.Long:
//                            return this.trailPrice - Math.Abs(this.trailPrice * this.level);
//                        case PositionSide.Short:
//                            return this.trailPrice + Math.Abs(this.trailPrice * this.level);
//                        default:
//                            throw new ArgumentException("Unknown position side : " + (object) this.position.Side);
//                    }
//                default:
//                    throw new ArgumentException("Unknown stop mode : " + (object) this.mode);
//            }
        }

        public void Disconnect()
        {
        }
    }

}

