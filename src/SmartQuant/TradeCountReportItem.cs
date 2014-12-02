// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{

    public class TradeCountReportItem : ReportItem
    {
        private int count;

        public TradeCountReportItem()
        {
            this.name = "Trade count";
            this.description = "Number of all trades (long + short)";
        }

        protected override void OnExecutionReport(ExecutionReport report)
        {
            if (report.ExecType == ExecType.ExecTrade)
                ++this.count;
        }

        protected internal override void Clear()
        {
            this.count = 0;
        }
    }
}
