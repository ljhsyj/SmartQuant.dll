using System.IO;

namespace SmartQuant
{
    public class ObjectTableStreamer : ObjectStreamer
    {

        public ObjectTableStreamer()
        {

            this.typeId = DataObjectType.ObjectTable;
            this.type = typeof(ObjectTable);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            var objectTable = new ObjectTable();
            int index;
            while ((index = reader.ReadInt32()) != -1)
                objectTable.fields[index] = this.streamerManager.Deserialize(reader);
            return (object)objectTable;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((byte)0);
            var table = (ObjectTable)obj;
            for (int i = 0; i < table.fields.Size; ++i)
            {
                if (table.fields[i] != null)
                {
                    writer.Write(i);
                    this.streamerManager.Serialize(writer, table.fields[i]);
                }
            }
            writer.Write(-1);
        }
    }
}
