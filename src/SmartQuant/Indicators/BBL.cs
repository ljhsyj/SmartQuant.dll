// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.ComponentModel;

namespace SmartQuant.Indicators
{
    [Serializable]
    public class BBL : Indicator
    {
        protected int length;
        protected double k;
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

        [Description("")]
        [Category("Parameters")]
        public double K
        {
            get
            {
                return this.k;
            }
            set
            {
                this.k = value;
                Init();
            }
        }

        public BBL(ISeries input, int length, double k, BarData barData = BarData.Close)
            :base(input)
        {
            this.length = length;
            this.barData = barData;
            this.k = k;
            Init();
        }

        protected override void Init()
        {
            this.name = string.Format("BBL ({0} , {1}, {2})", this.length, this.k, this.barData);
            this.description = "Bollinger Band Lower";
            Clear();
            this.calculate = true;
        }

        public override void Calculate(int index)
        {
            double d = Value(this.input, index, this.length, this.k, this.barData);
            if (!double.IsNaN(d))
            Add(this.input.GetDateTime(index), d);
        }

        public static double Value(ISeries input, int index, int length, double k, BarData barData = BarData.Close)
        {
            return index >= length - 1 ? SMA.Value(input, index, length, barData) - k * SMD.Value(input, index, length, barData) : double.NaN;
        }
    }
}
