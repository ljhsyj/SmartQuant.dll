// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SmartQuant.Optimization
{
    public class OptimizationParameterSet : IEnumerable<OptimizationParameter>
    {
        private List<OptimizationParameter> parameters = new List<OptimizationParameter>();

        public double Objective { get; set; }

        public OptimizationParameter this [int index]
        {
            get
            {
                return this.parameters[index];
            }
        }

        public void Add(OptimizationParameter parameter)
        {
            this.parameters.Add(parameter);
        }

        public void Add(string name, object value)
        {
            this.parameters.Add(new OptimizationParameter(name, value));
        }

        public IEnumerator<OptimizationParameter> GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(" ", this.parameters.Select(p => string.Format("{0} = {1}", p.Name, p.Value)));
        }
    }
}
