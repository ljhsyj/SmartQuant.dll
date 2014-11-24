 
namespace SmartQuant
{
    public class ObjectTable
    {
        internal IdArray<object> fields = new IdArray<object>(16);

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
            return (string)this.fields[index];
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
    }
}
