// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class AccountPosition
    {
        public byte CurrencyId { get; private set; }

        public double Value { get; private set; }

        public AccountPosition(byte currencyId, double value)
        {
            CurrencyId = currencyId;
            Value = value;
        }

        public AccountPosition(AccountTransaction transaction)
            : this(transaction.CurrencyId, transaction.Value)
        {
        }

        public void Add(AccountTransaction transaction)
        {
            Value += transaction.Value;
        }
    }
}
