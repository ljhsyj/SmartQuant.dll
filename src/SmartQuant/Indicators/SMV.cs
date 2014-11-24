// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.ComponentModel;

namespace SmartQuant.Indicators
{
    [Serializable]
    public class SMV : Indicator
    {
        protected int length;
        protected BarData barData;

        [Description("")]
        [Category("Parameters")]
        public BarData BarData
        {
            get
            {
                return this.barData;
            }
            set
            {
                this.barData = value;
                Init();
            }
        }

        [Category("Parameters")]
        [Description("")]
        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
                Init();
            }
        }

        public SMV(ISeries input, int length, BarData barData = BarData.Close)
            :base(input)
        {
            this.length = length;
            this.barData = barData;
            Init();
        }

        protected override void Init()
        {
            this.name = string.Format("SMV ({0}, {1})",this.length ,this.barData);
            this.description = "Simple Moving Variance";
            Clear();
            this.calculate = true;
        }

        public override void Calculate(int index)
        {
            double d = Value(this.input, index, this.length, this.barData);
            if (!double.IsNaN(d))
            Add(this.input.GetDateTime(index), d);
        }

        public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close)
        {
            if (index < length - 1)
                return double.NaN;
            double num1 = 0;
            double num2 = SMA.Value(input, index, length, barData);
            for (int index1 = index; index1 > index - length; --index1)
                num1 += (num2 - input[index1, barData]) * (num2 - input[index1, barData]);
            return num1 / (double) length;
        }
    }
}
