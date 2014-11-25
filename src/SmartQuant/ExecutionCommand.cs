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

        public string OCA { get; internal set; }

        public string Text { get; internal set; }

        public double StopPx { get; internal set; }

        public double Price { get; internal set; }

        public OrderSide Side { get; internal set; }

        public OrderType OrdType { get; internal set; }

        internal TimeInForce TimeInForce { get; set; }

        public double Qty { get; internal set; }

        public DateTime TransactTime { get; internal set; }

        public new Order Order { get; private set; }

        public ExecutionCommandType Type { get; internal set; }

        public string Account { get; internal set; }

        public string ClientID { get; internal set; }

        internal short ProviderId { get; set; }

        internal short PortfolioId { get; set; }

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
            Account = command.Account;
            ClientID = command.ClientID;
        }
    }
}
