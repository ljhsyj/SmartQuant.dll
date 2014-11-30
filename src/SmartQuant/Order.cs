// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SmartQuant
{
    public class Order : DataObject
    {
        internal int strategyId;

        public override byte TypeId
        {
            get
            {
                return DataObjectType.Order;
            }
        }

        public int Id { get; private set; }

        public ObjectTable Fields { get; private set; }

        [Category("Message")]
        [Description("Reports")]
        public List<ExecutionReport> Reports  { get; set; }

        [Description("Commands")]
        [Category("Message")]
        public List<ExecutionCommand> Commands { get; set; }

        [Description("Messages")]
        [Category("Message")]
        public List<ExecutionMessage> Messages { get; set; }

        [Browsable(false)]
        public Portfolio Portfolio { get; set; }

        [Browsable(false)]
        public IExecutionProvider Provider { get; set; }

        [ReadOnly(true)]
        public string OCA { get; set; }

        [ReadOnly(true)]
        public string Text { get; set; }

        public string Account { get; set; }

        public string ClientID { get; set; }

        [Browsable(false)]
        public bool IsNotSent
        {
            get
            {
                return this.Status == OrderStatus.NotSent;
            }
        }

        [Browsable(false)]
        public bool IsPendingNew
        {
            get
            {
                return this.Status == OrderStatus.PendingNew;
            }
        }

        [Browsable(false)]
        public bool IsNew
        {
            get
            {
                return this.Status == OrderStatus.New;
            }
        }

        [Browsable(false)]
        public bool IsRejected
        {
            get
            {
                return this.Status == OrderStatus.Rejected;
            }
        }

        [Browsable(false)]
        public bool IsPartiallyFilled
        {
            get
            {
                return this.Status == OrderStatus.PartiallyFilled;
            }
        }

        [Browsable(false)]
        public bool IsFilled
        {
            get
            {
                return this.Status == OrderStatus.Filled;
            }
        }

        [Browsable(false)]
        public bool IsPendingCancel
        {
            get
            {
                return this.Status == OrderStatus.PendingCancel;
            }
        }

        [Browsable(false)]
        public bool IsCancelled
        {
            get
            {
                return this.Status == OrderStatus.Cancelled;
            }
        }

        [Browsable(false)]
        public bool IsPendingReplace
        {
            get
            {
                return this.Status == OrderStatus.PendingReplace;
            }
        }

        [Browsable(false)]
        public bool IsReplaced
        {
            get
            {
                return this.Status == OrderStatus.Replaced;
            }
        }

        [Browsable(false)]
        public bool IsDone
        {
            get
            {
                return this.Status != OrderStatus.Filled && this.Status != OrderStatus.Cancelled && this.Status != OrderStatus.Rejected;
            }
        }

        [Browsable(false)]
        public bool IsOCA
        {
            get
            {
                return !string.IsNullOrEmpty(this.OCA);
            }
        }

        public OrderStatus Status { get; private set; }

        public Instrument Instrument { get; private set; }

        [ReadOnly(true)]
        public double StopPx { get; set; }

        [ReadOnly(true)]
        public double Price { get; set; }

        [ReadOnly(true)]
        public OrderSide Side { get; set; }

        [ReadOnly(true)]
        public OrderType Type { get; set; }

        [ReadOnly(true)]
        public double Qty { get; set; }

        public double CumQty { get; private set; }

        public double LeavesQty { get; private set; }

        public double AvgPx { get; private set; }

        [ReadOnly(true)]
        public TimeInForce TimeInForce { get; set; }

        [ReadOnly(true)]
        public DateTime ExpireTime { get; set; }

        [ReadOnly(true)]
        public byte Route { get; set; }

        public DateTime TransactTime { get; private set; }

        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Order()
        {
        }

        public Order(IExecutionProvider provider, Instrument instrument, OrderType type, OrderSide side, double qty, double price, double stopPx, TimeInForce timeInForce, string text)
            : this(provider, null, instrument, type, side, qty, price, stopPx, timeInForce, 0, text)
        {
        }

        public Order(IExecutionProvider provider, Portfolio portfolio, Instrument instrument, OrderType type, OrderSide side, double qty, double price, double stopPx, TimeInForce timeInForce, byte route, string text)
        {
        }

        public Order (Order order)
        {
        }

        public string GetSideAsString()
        {
            switch (Side)
            {
                case OrderSide.Buy:
                    return "Buy";
                case OrderSide.Sell:
                    return "Sell";
                default:
                    return "Undefined";
            }
        }

        public string GetTypeAsString()
        {
            switch (Type)
            {
                case OrderType.Market:
                    return "Market";
                case OrderType.Stop:
                    return "Stop";
                case OrderType.Limit:
                    return "Limit";
                case OrderType.StopLimit:
                    return "StopLimit";
                default:
                    return "Undefined";
            }
        }

        public string GetStatusAsString()
        {
            switch (Status)
            {
                case OrderStatus.NotSent:
                    return "NotSent";
                case OrderStatus.PendingNew:
                    return "PendingNew";
                case OrderStatus.New:
                    return "New";
                case OrderStatus.Rejected:
                    return "Rejected";
                case OrderStatus.PartiallyFilled:
                    return "PartiallyFilled";
                case OrderStatus.Filled:
                    return "Filled";
                case OrderStatus.PendingCancel:
                    return "PendingCancel";
                case OrderStatus.Cancelled:
                    return "Cancelled";
                case OrderStatus.PendingReplace:
                    return "PendingReplace";
                case OrderStatus.Replaced:
                    return "Replaced";
                default:
                    return "Undefined";
            }
        }

        public void OnExecutionCommand (ExecutionCommand command)
        {
            throw new NotImplementedException();
        }

        public void OnExecutionReport (ExecutionReport report)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Id, DateTime, GetTypeAsString(), GetSideAsString());
        }

    }
}