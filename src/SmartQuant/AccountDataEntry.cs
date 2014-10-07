// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class AccountDataEntry
    {
        public string Account { get; private set; }

        public AccountData Values { get; private set; }

        public AccountData[] Positions { get; private set; }

        public AccountData[] Orders { get; private set; }

        internal AccountDataEntry(string account, AccountData values, AccountData[] positions, AccountData[] orders)
        {
            Account = account;
            Values = values;
            Positions = positions;
            Orders = orders;
        }
    }
}
