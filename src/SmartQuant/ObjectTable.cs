using System.IO;


namespace SmartQuant
{
    public class ObjectTable
    {
        private IdArray<object> fields = new IdArray<object>(16);

        public int Size
        {
            get
            {
                return this.fields.Size;
            }
        }

        public object this [int index]
        {
            get
            {
                return this.fields[index];
            }
            set
            {
                this.fields[index] = value;
            }
        }

        public int GetInt(int index)
        {
            return (int)this.fields[index];
        }

        public double GetDouble(int index)
        {
            return (double)this.fields[index];
        }

        public string GetString(int index)
        {
            return this.fields[index] as string;
        }

        public void Remove(int id)
        {
            this.fields.Remove(id);
        }

        public void CopyTo(ObjectTable table)
        {
            this.fields.CopyTo(table.fields);
        }

        public void Clear()
        {
            this.fields.Clear();
        }

        #region Extra Helper Methods

        internal static object FromReader(BinaryReader reader, StreamerManager streamerManager)
        {
            var version = reader.ReadByte();
            var objectTable = new ObjectTable();
            int index;
            while ((index = reader.ReadInt32()) != -1)
                objectTable.fields[index] = streamerManager.Deserialize(reader);
            return objectTable;
        }

        internal void ToWriter(BinaryWriter writer, StreamerManager streamerManager)
        {
            byte version = 0;
            writer.Write(version);
            for (int i = 0; i < this.fields.Size; ++i)
            {
                var f = this.fields[i];
                if (f != null)
                {
                    writer.Write(i);
                    streamerManager.Serialize(writer, f);
                }
            }
            writer.Write(-1);
        }

        #endregion
    }
}
