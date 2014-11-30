using System;
using System.IO;

namespace SmartQuant
{
    public class StrategyStatusStreamer : ObjectStreamer
    {
        public StrategyStatusStreamer()
        {
            this.typeId = DataObjectType.StrategyStatus;
            this.type = typeof(StrategyStatusInfo);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            return new StrategyStatusInfo(new DateTime(reader.ReadInt64()), (StrategyStatusType)reader.ReadByte()) { Solution = reader.ReadString(),  Mode = reader.ReadString() };
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            writer.Write(version);
            var info = obj as StrategyStatusInfo;
            writer.Write(info.DateTime.Ticks);
            writer.Write((byte)info.Type);
            writer.Write(info.Solution);
            writer.Write(info.Mode);
        }
    }
}
