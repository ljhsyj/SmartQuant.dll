using System;

namespace SmartQuant
{
    public class OnSendOrder : Event
    {
        internal Order Order { get; private set; }

        public override byte TypeId
        {
            get
            {
                return DataObjectType.OnSendOrder;
            }
        }

        public OnSendOrder(Order order)
        {
            Order = order;
        }
    }
}

