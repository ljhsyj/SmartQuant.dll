//
// Author: Alex Lee <lu.lee05@gmail.com>
//

namespace SmartQuant
{
    public class OnPositionOpened : Event
    {
        internal Portfolio Portfolio { get; set; }
        internal Position Position { get; set; }

        public override byte TypeId
        {
            get
            {
                return EventType.OnPositionOpened;
            }
        }

        public OnPositionOpened(Portfolio portfolio, Position position)
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
