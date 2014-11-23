// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Diagnostics;

namespace SmartQuant.Optimization
{
    public class MulticoreOptimizer
    {
        private Stopwatch stopwatch = new Stopwatch();

        public long Elapsed
        {
            get
            {
                return this.stopwatch.ElapsedMilliseconds;
            }
        }

		public long EventCount { get; private set; }

        public OptimizationParameterSet Optimize(Strategy strategy, InstrumentList instruments, OptimizationUniverse universe, int bunch = -1)
        {
			throw new NotImplementedException ();
        }
	}
}
