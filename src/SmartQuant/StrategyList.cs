using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class StrategyList : IEnumerable<Strategy>
    {
        public StrategyList()
        {
        }

        public IEnumerator<Strategy> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }
}

