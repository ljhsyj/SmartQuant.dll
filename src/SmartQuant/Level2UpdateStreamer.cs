using System.IO;
using System;

namespace SmartQuant
{
    public class Level2UpdateStreamer : ObjectStreamer
    {

        public Level2UpdateStreamer()
        {
            this.typeId = DataObjectType.Level2Update;
            this.type = typeof(Level2Update);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            var l2u = (Level2Update)obj;
            writer.Write((byte)0);
            writer.Write(l2u.DateTime.ToBinary());
            writer.Write(l2u.ProviderId);
            writer.Write(l2u.InstrumentId);
            writer.Write(l2u.Entries.Length);
            foreach (var level2 in l2u.Entries)
                this.streamerManager.Serialize(writer, level2);
        }

        public override object Read(BinaryReader reader)
        {
            var l2u = new Level2Update();
            var version =  reader.ReadByte();
            l2u.DateTime = DateTime.FromBinary(reader.ReadInt64());
            l2u.ProviderId = reader.ReadByte();
            l2u.InstrumentId = reader.ReadInt32();
            int length = reader.ReadInt32();
            l2u.Entries = new Level2[length];
            for (int i = 0; i < length; ++i)
                l2u.Entries[i] = (Level2)this.streamerManager.Deserialize(reader);
            return l2u;
        }
    }
}
