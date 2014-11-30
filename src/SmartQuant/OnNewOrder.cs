using System;

namespace SmartQuant
{
    public class OnNewOrder : Event
    {
        internal Order Order { get; private set;}

        public override byte TypeId
        {
            get
            {
                return DataObjectType.OnNewOrder;
            }
        }

        public OnNewOrder(Order order)
        {
            Order = order;
        }
    }
}
