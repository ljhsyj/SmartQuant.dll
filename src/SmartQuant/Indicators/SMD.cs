// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.ComponentModel;

namespace SmartQuant.Indicators
{
    [Serializable]
    public class SMD : Indicator
    {
        protected int length;
        protected BarData barData;

        [Description("")]
        [Category("Parameters")]
        public BarData Option
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

        public SMD(ISeries input, int length, BarData barData = BarData.Close)
            :base(input){
            this.length = length;
            this.barData = barData;
            this.Init();
        }

        protected override void Init()
        {
            this.name = string.Format("SMD ({0}, {1})", this.length, this.barData);
            this.description = "Simple Moving Deviation";
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
            return index >= length - 1 ? Math.Sqrt(SMV.Value(input, index, length, barData)) : double.NaN;
        }
    }
}
