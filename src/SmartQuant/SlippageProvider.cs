// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class SlippageProvider : ISlippageProvider
    {
        public double Slippage { get; set; }

        public virtual double GetPrice(ExecutionReport report)
        {
            double avgPx = report.AvgPx;
            switch (report.Side)
            {
                case OrderSide.Buy:
                    avgPx += avgPx * this.Slippage;
                    break;
                case OrderSide.Sell:
                    avgPx -= avgPx * this.Slippage;
                    break;
            }
            return avgPx;
        }
    }
}
