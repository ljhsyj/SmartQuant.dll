// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class OnPositionClosed : Event
    {
        internal Portfolio Portfolio { get; set; }
        internal Position Position { get; set; }

        public override byte TypeId
        {
            get
            {
                return EventType.OnPositionClosed;
            }
        }

        public OnPositionClosed(Portfolio portfolio, Position position)
        {
            Portfolio = portfolio;
            Position = position;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", GetType().Name, Position);
        }
    }
}
