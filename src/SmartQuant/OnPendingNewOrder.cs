using System;

namespace SmartQuant
{
    public class OnPendingNewOrder : Event
    {
        internal Order Order { get; private set; }

        public override byte TypeId
        {
            get
            {
                return EventType.OnPendingNewOrder;
            }
        }

        public OnPendingNewOrder(Order order)
        {
            Order = order;
        }
    }
}

