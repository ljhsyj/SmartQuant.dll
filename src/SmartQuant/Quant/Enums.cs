// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant.Quant
{
    public enum EOptionPosition
    {
        InTheMoney,
        AtTheMoney,
        OutOfTheMoney
    }

    public enum EOptionPrice
    {
        BlackScholes,
        Binomial,
        Trinomial,
        MonteCarlo
    }

    public enum EOptionType
    {
        European,
        American,
        Exotic,
        Bermudian,
        Digial
    }

    public enum EPutCall
    {
        Call,
        Put
    }
}
