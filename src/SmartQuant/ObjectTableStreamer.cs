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
            return ObjectTable.FromReader(reader, this.streamerManager);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            (obj as ObjectTable).ToWriter(writer, this.streamerManager);
        }
    }
}