using System.Collections;
using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public static class SomeExtensions
    {
        public static int MaxIndex<T>(this IEnumerable<T> sequence) where T : IComparable<T>
        {
            int maxIndex = -1;
            T maxValue = default(T);
            int i = 0;
            foreach (var value in sequence)
            {
                if (value.CompareTo(maxValue) > 0 || maxIndex == -1)
                {
                    maxIndex = i++;
                    maxValue = value;
                }
            }
            return maxIndex;
        }
    }
}

