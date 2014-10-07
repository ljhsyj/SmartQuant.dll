﻿// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class AccountTransaction
    {
        public DateTime DateTime { get; private set; }

        public double Value { get; private set; }

        public byte CurrencyId { get; private set; }

        public string Text { get; private set; }

        public AccountTransaction(DateTime dateTime, double value, byte currencyId, string text)
        {
            DateTime = dateTime;
            Value = value;
            CurrencyId = currencyId;
            Text = text;
        }

        public AccountTransaction(Fill fill)
            : this(fill.DateTime, fill.CashFlow, fill.CurrencyId, fill.Text)
        {
        }
    }
}
