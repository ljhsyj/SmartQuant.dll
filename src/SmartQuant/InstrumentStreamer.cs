using System.IO;
using System;


namespace SmartQuant
{
    public class InstrumentStreamer : ObjectStreamer
    {
        public InstrumentStreamer()
        {
            this.typeId = ObjectType.Instrument;
            this.type = typeof(Instrument);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            var instrument = new Instrument
            { 
                Id = reader.ReadInt32(),
                Type = (InstrumentType)reader.ReadByte(), 
                Symbol = reader.ReadString(), 
                Description = reader.ReadString(),
                CurrencyId = reader.ReadByte(),
                Exchange = reader.ReadString()
            };
            instrument.TickSize = reader.ReadDouble();
            instrument.Maturity = new DateTime(reader.ReadInt64());
            instrument.Factor = reader.ReadDouble();
            instrument.Strike = reader.ReadDouble();
            instrument.PutCall = (PutCall)reader.ReadByte();
            instrument.Margin = reader.ReadDouble();
            int altIdCount = reader.ReadInt32();
            for (int i = 0; i < altIdCount; ++i)
                instrument.AltId.Add((AltId)this.streamerManager.Deserialize(reader));
            int legCount = reader.ReadInt32();
            for (int i = 0; i < legCount; ++i)
                instrument.Legs.Add((Leg)this.streamerManager.Deserialize(reader));
            if (version == 0)
            {
                int num4 = reader.ReadInt32();
                for (int index = 0; index < num4; ++index)
                    instrument.Fields[index] = (object)reader.ReadDouble();
            }
            if ((int)version >= 1)
            {
                instrument.PriceFormat = reader.ReadString();
                if (reader.ReadInt32() != -1)
                    instrument.Fields = (ObjectTable)this.streamerManager.Deserialize(reader);
            }
            if (version >= 2)
            {
                instrument.CCY1 = reader.ReadByte();
                instrument.CCY2 = reader.ReadByte();
            }
            return  instrument;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 2;
            writer.Write(version);
            var instrument = (Instrument)obj;
            writer.Write(instrument.Id);
            writer.Write((byte)instrument.Type);
            writer.Write(instrument.Symbol);
            writer.Write(instrument.Description);
            writer.Write(instrument.CurrencyId);
            writer.Write(instrument.Exchange);
            writer.Write(instrument.TickSize);
            writer.Write(instrument.Maturity.Ticks);
            writer.Write(instrument.Factor);
            writer.Write(instrument.Strike);
            writer.Write((byte)instrument.PutCall);
            writer.Write(instrument.Margin);
            writer.Write(instrument.AltId.Count);
            foreach (var altId in instrument.AltId)
                this.streamerManager.Serialize(writer, altId);
            writer.Write(instrument.Legs.Count);
            foreach (var leg in instrument.Legs)
                this.streamerManager.Serialize(writer, leg);
            if (version == 0)
            {
                writer.Write(instrument.Fields.Size);
                for (int i = 0; i < instrument.Fields.Size; ++i)
                    writer.Write((double)instrument.Fields[i]);
            }
            if (version >= 1)
            {
                writer.Write(instrument.PriceFormat);
                if (instrument.Fields == null)
                {
                    writer.Write(-1);
                }
                else
                {
                    writer.Write(instrument.Fields.Size);
                    this.streamerManager.Serialize(writer, instrument.Fields);
                }
            }
            if (version >= 2)
            {
                writer.Write(instrument.CCY1);
                writer.Write(instrument.CCY2);
            }
        }
    }
}
