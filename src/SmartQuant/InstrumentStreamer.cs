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
            return new Instrument(reader);
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            (obj as Instrument).Write(writer, this.streamerManager);
        }
    }
}