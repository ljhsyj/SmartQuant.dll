using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class StrategyList : IEnumerable<Strategy>
    {
        private GetByList<Strategy> list = new GetByList<Strategy>();
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public Strategy this[int index]
        {
            get
            {
                return this.list.GetByIndex(index);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Contains(Strategy strategy)
        {
            return this.list.Contains(strategy);
        }

        public bool Contains(int id)
        {
            return this.list.Contains(id);
        }

        public void Add(Strategy strategy)
        {
            this.list.Add(strategy);
        }

        public void Remove(Strategy strategy)
        {
            this.list.Remove(strategy);
        }

        public Strategy GetByIndex(int index)
        {
            return this.list.GetByIndex(index);
        }

        public Strategy GetById(int id)
        {
            return this.list.GetById(id);
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public IEnumerator<Strategy> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}

