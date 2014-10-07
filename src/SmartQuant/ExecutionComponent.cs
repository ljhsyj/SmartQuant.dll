// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class ExecutionComponent : StrategyComponent
    {
        public virtual void OnOrder(Order order)
        {
        }

        public virtual void OnExecutionReport(ExecutionReport report)
        {
        }

        public virtual void OnOrderFilled(Order order)
        {
        }
    }
}