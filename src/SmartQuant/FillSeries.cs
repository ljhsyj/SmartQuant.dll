// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections;

namespace SmartQuant
{
    public class FillSeries : IEnumerable<Fill>
    {
        private string name;
        private List<Fill> fills;
        private Fill min;
        private Fill max; 

        public int Count
        {
            get
            {
                return this.fills.Count;
            }
        }

        public Fill Min
        {
            get
            {
                return this.min;
            }
        }

        public Fill Max
        {
            get
            {
                return this.max;
            }
        }

        public Fill this[int index]
        {
            get
            {
                return this.fills[index];
            }
        }
            
        public FillSeries(string name = "")
        {
            this.name = name;
            this.fills = new List<Fill>();
        }

        public void Clear()
        {
            this.fills.Clear();
            this.min = this.max = null;
        }

        public void Add(Fill fill)
        {
            this.max = this.max == null ? fill : (this.max.Price < fill.Price ? fill : this.max);
            this.min = this.min == null ? fill : (this.min.Price > fill.Price ? fill : this.min);

            if (this.fills.Count != 0 && fill.DateTime < this.fills[this.fills.Count - 1].DateTime)
                Console.WriteLine("FillSeries::Add {0} + incorrect fill order : {1}", this.name, fill);

            this.fills.Add(fill);
        }

        public IEnumerator<Fill> GetEnumerator()
        {
            return this.fills.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.fills.GetEnumerator();
        }

        public int GetIndex(DateTime datetime, IndexOption option)
        {
            int index = 0;
            int num1 = 0;
            int num2 = this.fills.Count - 1;
            bool flag = true;
            while (flag)
            {
                if (num2 < num1)
                    return -1;
                index = (num1 + num2) / 2;
                switch (option)
                {
                    case IndexOption.Null:
                        if (this.fills[index].DateTime == datetime)
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.fills[index].DateTime > datetime)
                        {
                            num2 = index - 1;
                            continue;
                        }
                        else if (this.fills[index].DateTime < datetime)
                        {
                            num1 = index + 1;
                            continue;
                        }
                        else
                            continue;
                    case IndexOption.Next:
                        if (this.fills[index].DateTime >= datetime && (index == 0 || this.fills[index - 1].DateTime < datetime))
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.fills[index].DateTime < datetime)
                        {
                            num1 = index + 1;
                            continue;
                        }
                        else
                        {
                            num2 = index - 1;
                            continue;
                        }
                    case IndexOption.Prev:
                        if (this.fills[index].DateTime <= datetime && (index == this.fills.Count - 1 || this.fills[index + 1].DateTime > datetime))
                        {
                            flag = false;
                            continue;
                        }
                        else if (this.fills[index].DateTime > datetime)
                        {
                            num2 = index - 1;
                            continue;
                        }
                        else
                        {
                            num1 = index + 1;
                            continue;
                        }
                    default:
                        continue;
                }
            }
            return index;
        }
    }
}

