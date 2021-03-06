﻿// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.ComponentModel;

namespace SmartQuant.Indicators
{
    [Serializable]
    public class SMA : Indicator
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

        [Description("")]
        [Category("Parameters")]
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

        public SMA(ISeries input, int length, BarData barData = BarData.Close) : base(input)
        {
            this.length = length;
            this.barData = barData;
            Init();
        }

        protected override void Init()
        {
            this.name = string.Format("SMA ({0})", this.length);
            this.description = "Simple Moving Average";
            Clear();
            this.calculate = true;
        }

        public override void Calculate(int index)
        {
            if (index < this.length - 1)
                return;
            double num = 0;
            if (index == this.length - 1)
                for (int index1 = index; index1 >= index - this.length + 1; --index1)
                    num += this.input[index1, this.barData] / (double) this.length;
            else
                num = ((TimeSeries) this)[this.input.GetDateTime(index - 1), SearchOption.ExactFirst] + (this.input[index, this.barData] - this.input[index - this.length, this.barData]) / (double) this.length;
            Add(this.input.GetDateTime(index), num);
        }

        public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close)
        {
            if (index < length - 1)
                return double.NaN;
            double num = 0;
            for (int index1 = index; index1 >= index - length + 1; --index1)
                num += input[index1, barData];
            return num / (double) length;
        }
    }
}
