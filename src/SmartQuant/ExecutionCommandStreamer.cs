using System.IO;
using System;

namespace SmartQuant
{
    public class ExecutionCommandStreamer : ObjectStreamer
    {
        public ExecutionCommandStreamer()
        {
            this.typeId = DataObjectType.ExecutionCommand;
            this.type = typeof(ExecutionCommand);
        }

        public override object Read(BinaryReader reader)
        {
            var command = new ExecutionCommand();
            var version = reader.ReadByte();
            command.Id = reader.ReadInt32();
            command.Type = (ExecutionCommandType)reader.ReadByte();
            command.TransactTime = new DateTime(reader.ReadInt64());
            command.OrderId = reader.ReadInt32();
            command.InstrumentId = reader.ReadInt32();
            command.ProviderId = reader.ReadInt16();
            command.PortfolioId = reader.ReadInt16();
            command.Side = (OrderSide)reader.ReadByte();
            command.OrdType = (OrderType)reader.ReadByte();
            command.TimeInForce = (TimeInForce)reader.ReadByte();
            command.Price = reader.ReadDouble();
            command.StopPx = reader.ReadDouble();
            command.Qty = reader.ReadDouble();
            command.OCA = reader.ReadString();
            command.Text = reader.ReadString();
            if (reader.ReadBoolean())
                command.Account = reader.ReadString();
            if (reader.ReadBoolean())
                command.ClientID = reader.ReadString();
            if (reader.ReadBoolean())
                command.Fields = (ObjectTable)this.streamerManager.Deserialize(reader);
            return  command;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            var command = obj as ExecutionCommand;
            writer.Write(version);
            writer.Write(command.Id);
            writer.Write((byte)command.Type);
            writer.Write(command.TransactTime.Ticks);
            writer.Write(command.OrderId);
            writer.Write(command.InstrumentId);
            writer.Write((short)command.Provider.Id);
            writer.Write((short)command.Portfolio.Id);
            writer.Write((byte)command.Side);
            writer.Write((byte)command.OrdType);
            writer.Write((byte)command.TimeInForce);
            writer.Write(command.Price);
            writer.Write(command.StopPx);
            writer.Write(command.Qty);
            writer.Write(command.OCA);
            writer.Write(command.Text);
            if (command.Account != null)
            {
                writer.Write(true);
                writer.Write(command.Account);
            }
            else
                writer.Write(false);
            if (command.ClientID != null)
            {
                writer.Write(true);
                writer.Write(command.ClientID);
            }
            else
                writer.Write(false);
            if (command.Fields != null)
            {
                writer.Write(true);
                this.streamerManager.Serialize(writer, command.Fields);
            }
            else
                writer.Write(false);
        }
    }
}
