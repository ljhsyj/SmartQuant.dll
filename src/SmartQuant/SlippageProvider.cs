// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class SlippageProvider : ISlippageProvider
    {
        public double Slippage { get; set; }

        public virtual double GetPrice(ExecutionReport report)
        {
            return report.AvgPx + report.AvgPx * Slippage * (report.Side == OrderSide.Buy ? 1 : -1);
        }
    }
}
