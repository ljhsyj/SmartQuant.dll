// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant.Optimization
{
    public class OptimizationParameter
    {
        public string Name { get; private set; }

        public object Value { get; private set; }

        public OptimizationParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
