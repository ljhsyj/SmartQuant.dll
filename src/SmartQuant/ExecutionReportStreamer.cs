using System.IO;
using System;

namespace SmartQuant
{
    public class ExecutionReportStreamer : ObjectStreamer
    {
        public ExecutionReportStreamer()
        {
            this.typeId = DataObjectType.ExecutionReport;
            this.type = typeof(ExecutionReport);
        }

        public override object Read(BinaryReader reader)
        {
            var report = new ExecutionReport();
            var version = reader.ReadByte();
            report.DateTime = new DateTime(reader.ReadInt64());
            report.OrderId = reader.ReadInt32();
            report.InstrumentId = reader.ReadInt32();
            report.CurrencyId = reader.ReadByte();
            report.ExecType = (ExecType)reader.ReadByte();
            report.OrdStatus = (OrderStatus)reader.ReadByte();
            report.OrdType = (OrderType)reader.ReadByte();
            report.Side = (OrderSide)reader.ReadByte();
            report.TimeInForce = (TimeInForce)reader.ReadByte();
            report.Price = reader.ReadDouble();
            report.StopPx = reader.ReadDouble();
            report.OrdQty = reader.ReadDouble();
            report.CumQty = reader.ReadDouble();
            report.LeavesQty = reader.ReadDouble();
            report.LastPx = reader.ReadDouble();
            report.LastQty = reader.ReadDouble();
            report.Commission = reader.ReadDouble();
            report.Text = reader.ReadString();
            if (reader.ReadBoolean())
                report.Fields = (ObjectTable)this.streamerManager.Deserialize(reader);
            return report;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            var report = (ExecutionReport)obj;
            writer.Write((byte)0);
            writer.Write(report.DateTime.Ticks);
            writer.Write(report.OrderId);
            writer.Write(report.InstrumentId);
            writer.Write(report.CurrencyId);
            writer.Write((byte)report.ExecType);
            writer.Write((byte)report.OrdStatus);
            writer.Write((byte)report.OrdType);
            writer.Write((byte)report.Side);
            writer.Write((byte)report.TimeInForce);
            writer.Write(report.Price);
            writer.Write(report.StopPx);
            writer.Write(report.OrdQty);
            writer.Write(report.CumQty);
            writer.Write(report.LeavesQty);
            writer.Write(report.LastPx);
            writer.Write(report.LastQty);
            writer.Write(report.Commission);
            writer.Write(report.Text);
            if (report.Fields != null)
            {
                writer.Write(true);
                this.streamerManager.Serialize(writer, report.Fields);
            }
            else
                writer.Write(false);
        }
    }
}
