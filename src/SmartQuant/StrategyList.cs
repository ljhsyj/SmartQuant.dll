using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{

    public class StrategyList : IEnumerable<Strategy>
    {
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Strategy this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public StrategyList()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Strategy strategy)
        {
            throw new NotImplementedException();
        }

        public bool Contains(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(Strategy strategy)
        {
            throw new NotImplementedException();
        }

        public void Remove(Strategy strategy)
        {
            throw new NotImplementedException();
        }

        public Strategy GetByIndex(int index)
        {
            throw new NotImplementedException();
        }

        public Strategy GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
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

