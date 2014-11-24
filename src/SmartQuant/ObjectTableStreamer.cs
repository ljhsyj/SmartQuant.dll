using System.IO;

namespace SmartQuant
{
    public class ObjectTableStreamer : ObjectStreamer
    {

        public ObjectTableStreamer()
        {

            this.typeId = DataObjectType.ObjectTable;
            this.type = typeof (ObjectTable);
        }

        public override object Read(BinaryReader reader)
        {
            var version =  reader.ReadByte();
            var objectTable = new ObjectTable();
            int index;
            while ((index = reader.ReadInt32()) != -1)
                objectTable.fields[index] = this.streamerManager.Deserialize(reader);
            return (object) objectTable;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            writer.Write((byte) 0);
            ObjectTable objectTable = (ObjectTable) obj;
            for (int index = 0; index < objectTable.fields.Size; ++index)
            {
                if (objectTable.fields[index] != null)
                {
                    writer.Write(index);
                    this.streamerManager.Serialize(writer, objectTable.fields[index]);
                }
            }
            writer.Write(-1);
        }
    }
}
