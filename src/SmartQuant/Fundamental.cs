// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;

namespace SmartQuant
{
    public class Fundamental : DataObject
    {
        private static Dictionary<string, byte> mapping;

        internal int ProviderId { get; set; }

        internal int InstrumentId { get; set; }

        internal IdArray<double> Fields { get; set; }

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
                return Fields[index];
            }
            set
            {
                if (Fields == null)
                    Fields = new IdArray<double>(16);
                Fields[index] = value;
            }
        }

        public double this [string name]
        {
            get
            {
                return this[mapping[name]];
            }
            set
            {
                this[mapping[name]] = value;
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
            ProviderId = providerId;
            InstrumentId = instrumentId;
        }

        public static void AddField(string name, byte index)
        {
            mapping.Add(name, index);
        }

        public override string ToString()
        {
            return "";
        }

        #region Extra Helper Methods

        internal Fundamental(BinaryReader reader)
        {
            var version = reader.ReadByte();
            var datetime = new DateTime(reader.ReadInt64());
            var providerId = reader.ReadInt32();
            var instrumentId = reader.ReadInt32();
            var fundamental = new Fundamental(datetime, providerId, instrumentId);
            int count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
                fundamental.Fields[i] = reader.ReadDouble();
        }

        internal void Write(BinaryWriter writer)
        {
            byte version = 0;
            var fundamental = this;
            writer.Write(version);       
            writer.Write(fundamental.DateTime.Ticks);
            writer.Write(fundamental.ProviderId);
            writer.Write(fundamental.InstrumentId);
            if (fundamental.Fields != null)
            {
                writer.Write(fundamental.Fields.Size);
                for (int i = 0; i < fundamental.Fields.Size; ++i)
                    writer.Write(fundamental.Fields[i]);
            }
            else
                writer.Write(0);
        }

        #endregion
    }
}
