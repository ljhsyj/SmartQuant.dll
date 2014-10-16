// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.
using System;

namespace SmartQuant
{
    public class ExecutionCommand : ExecutionMessage
    {
        public override byte TypeId
        {
            get
            {
                return DataObjectType.ExecutionCommand;
            }
        }

        public Portfolio Portfolio { get; private set; }

        public IExecutionProvider Provider { get; private set; }

        public string OCA { get; private set; }

        public string Text { get; private set; }

        public double StopPx { get; private set; }

        public double Price { get; private set; }

        public OrderSide Side { get; private set; }

        public OrderType OrdType { get; private set; }

        public double Qty { get; private set; }

        public DateTime TransactTime { get; private set; }

        public new Order Order { get; private set; }

        public ExecutionCommandType Type { get; private set; }

        public string Account { get; private set; }

        public string ClientID { get; private set; }

        public ExecutionCommand()
        {
            Text = "";
            OCA = "";
        }

        public ExecutionCommand(ExecutionCommandType type, Order order)
            : this()
        {
            Type = type;
            Order = order;
            OrderId = order.Id;
        }

        public ExecutionCommand(ExecutionCommand command)
            : this()
        {
            this.Account = command.Account;
            this.ClientID = command.ClientID;
        }
    }
}
