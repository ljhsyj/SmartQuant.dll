// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System.Collections.Generic;
using System;
using System.Reflection;

namespace SmartQuant
{
    public class StatisticsManager
    {
        private Framework framework;

        public PortfolioStatisticsItemList Statistics { get; internal set; }

        public StatisticsManager(Framework framework)
        {
            this.framework = framework;
            Statistics = new PortfolioStatisticsItemList();

            foreach (var info in typeof(PortfolioStatisticsType).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (info.FieldType == typeof(int))
                {
                    Type t = Type.GetType(info.Name);
                    if (t != null)
                    {
                        var item = (PortfolioStatisticsItem)Activator.CreateInstance(t);
                        Add(item);
                    }
                }
            }
        }

        public void Add(PortfolioStatisticsItem item)
        {
            Statistics.Add(item);
        }

        public bool Contains(int type)
        {
            return Statistics.Contains(type);
        }

        public void Remove(int type)
        {
            Statistics.Remove(type);
        }

        public PortfolioStatisticsItem Get(int type)
        {
            return Statistics.GetByType(type);
        }

        public PortfolioStatisticsItem Clone(int type)
        {
            return (PortfolioStatisticsItem)Activator.CreateInstance(Get(type).GetType());
        }

        public List<PortfolioStatisticsItem> CloneAll()
        {
            var list = new List<PortfolioStatisticsItem>();
            foreach (var obj in Statistics)
                list.Add((PortfolioStatisticsItem)Activator.CreateInstance(obj.GetType()));
            return list;
        }
    }
}
