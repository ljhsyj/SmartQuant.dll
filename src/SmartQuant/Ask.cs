// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class Ask : Tick
    {
        public override byte TypeId
        {
            get
            {
                return DataObjectType.Ask;
            }
        }

        public Ask(DateTime dateTime, byte providerId, int instrument, double price, int size)
            : base(dateTime, providerId, instrument, price, size)
        {
        }

        public Ask(DateTime dateTime, DateTime exchangeDateTime, byte providerId, int instrument, double price, int size)
            : base(dateTime, exchangeDateTime, providerId, instrument, price, size)
        {
        }

        public Ask()
        {
        }

        public Ask(Ask ask)
            : base(ask)
        {
        }

        public override string ToString()
        {
            return string.Format("Ask {0} {1} {2} {3} {4}", DateTime, ProviderId, InstrumentId, Price, Size);
        }

        #region Extra Helper Methods

        internal Ask(BinaryReader reader)
        {
            var version = reader.ReadByte();
            DateTime = new DateTime(reader.ReadInt64());
            if (version == 1)
                ExchangeDateTime = new DateTime(reader.ReadInt64());
            ProviderId = reader.ReadByte();
            InstrumentId = reader.ReadInt32();
            Price = reader.ReadDouble();
            Size = reader.ReadInt32();
        }

        internal void Write(BinaryWriter writer)
        { 
            byte version = 0;
            if (ExchangeDateTime.Ticks != 0)
                version = 1;
            writer.Write(version);
            writer.Write(DateTime.Ticks);
            if (version == 1)
                writer.Write(ExchangeDateTime.Ticks);
            writer.Write((byte)ProviderId);
            writer.Write(InstrumentId);
            writer.Write(Price);
            writer.Write(Size);
        }

        #endregion
    }
}
