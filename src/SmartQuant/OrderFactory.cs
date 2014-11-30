using System;

namespace SmartQuant
{
    public class OrderFactory
    {
        private IdArray<Order> orders = new IdArray<Order>(1000000);

        public Order OnExecutionCommand(ExecutionCommand command)
        {
            throw new NotImplementedException();
        }

        public Order OnExecutionReport(ExecutionReport report)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            this.orders.Clear();
        }
    }
}

