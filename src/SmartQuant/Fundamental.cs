// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class Fundamental : DataObject
    {
        private static Dictionary<string, byte> mapping;
        private int providerId;
        private int instrumentId;

        public override byte TypeId
        {
            get
            {
                return DataObjectType.Fundamental;
            }
        }

        public double this [byte index]
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

        public double this [string name]
        {
            get
            {
                return this[Fundamental.mapping[name]];
            }
            set
            {
                this[Fundamental.mapping[name]] = value;
            }
        }

        static Fundamental()
        {
            mapping = new Dictionary<string, byte>();
            foreach (string field in Enum.GetNames(typeof(FundamentalData)))
            {
                byte n = (byte)Enum.Parse(typeof(FundamentalData), field);
                AddField(field, n);
            } 
        }

        public Fundamental()
        {
        }

        public Fundamental(DateTime dateTime, int providerId, int instrumentId)
            : base(dateTime)
        {
            this.providerId = providerId;
            this.instrumentId = instrumentId;
        }

        public static void AddField(string name, byte index)
        {
            mapping.Add(name, index);
        }

        public override string ToString()
        {
            return "";
        }
    }
}
