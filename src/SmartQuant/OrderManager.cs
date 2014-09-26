// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.
using System.Collections.Generic;

namespace SmartQuant
{
	public class OrderManager
	{
        private Framework framework;
        private OrderServer server;

        public List<Order> Orders { get; private set; }

        public OrderManager(Framework framework, OrderServer server)
        {
            this.framework = framework;
            this.server = server;
            this.Orders = new List<Order>();
        }

        public void Send(Order order)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public void Reject(Order order)
        {
        }

        public void Register(Order order)
        {
        }
            
        public void Cancel(Order order)
        {
        }
            
        public void Replace(Order order, double price, double stopPx, double qty)
        {
        }

        public void Dump()
        {
        }

        public void Load()
        {
        }
	}
}
