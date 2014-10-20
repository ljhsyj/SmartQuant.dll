using System;
using System.Linq;

using System.Threading.Tasks;

namespace SmartQuant.Quant
{
    public class Vector
    {
        public int NRows { get; private set; }

        public double[] Elements { get; private set; }

        public double this [int index]
        {
            get
            {
                return Elements[index];
            }
            set
            {
                Elements[index] = value;
            }
        }

        public Vector()
        {
            NRows = -1;
        }

        public Vector(int nrows)
        {
            if (nrows <= 0)
                throw new ArgumentException("Number of rows has to be positive");
            NRows = nrows;
            Elements = new double[NRows];
        }

        public static double operator *(Vector v1, Vector v2)
        {
            if (!Vector.AreCompatible(v1, v2))
                throw new ApplicationException("Vectors are not compatible");
            double result = 0;
            Parallel.For(0, v1.NRows, i => result += v1[i] * v2[i]);
            return result;
        }

        public static Vector operator *(Vector vector, double val)
        {
            if (!vector.IsValid())
                throw new ApplicationException("Vector is not initialized");
            Vector v = new Vector(vector.NRows);
            Parallel.For(0, v.NRows, i => v[i] = vector[i] * val);
            return v;
        }

        public static Vector operator +(Vector vector, double val)
        {
            if (!vector.IsValid())
                throw new ApplicationException("Vector is not initialized");
            Vector v = new Vector(vector.NRows);
            Parallel.For(0, v.NRows, i => v[i] = vector[i] + val);
            return v;
        }

        public static Vector operator -(Vector vector, double val)
        {
            if (!vector.IsValid())
                throw new ApplicationException("Vector is not initialized");
            Vector v = new Vector(vector.NRows);
            Parallel.For(0, v.NRows, i => v[i] = vector[i] - val);
            return v;
        }

        public static Vector operator +(Vector target, Vector source)
        {
            if (!source.IsValid())
                throw new ApplicationException("Source vector is not initialized");
            if (!target.IsValid())
                throw new ApplicationException("Target vector is not initialized");
            if (!Vector.AreCompatible(target, source))
                throw new ApplicationException("Vectors are not compatible");
            Vector v = new Vector(target.NRows);
            Parallel.For(0, v.NRows, i => v[i] = target[i] + source[i]);
            return v;
        }

        public static Vector operator -(Vector target, Vector source)
        {
            if (!source.IsValid())
                throw new ApplicationException("Source vector is not initialized");
            if (!target.IsValid())
                throw new ApplicationException("Target vector is not initialized");
            if (!Vector.AreCompatible(target, source))
                throw new ApplicationException("Vectors are not compatible");
            Vector v = new Vector(target.NRows);
            Parallel.For(0, v.NRows, i => v[i] = target[i] - source[i]);
            return v;
        }

        public bool IsValid()
        {
            return NRows != -1;
        }

        public static bool AreCompatible(Vector v1, Vector v2)
        {
            if (!v1.IsValid())
                throw new ArgumentException("Vector 1 is not initialized");
            if (!v2.IsValid())
                throw new ArgumentException("Vector 2 is not initialized");
            return v1.NRows == v2.NRows;
        }

        public void Zero()
        {
            Parallel.For(0, Elements.Length, i => Elements[i] = 0);
        }

        public void ResizeTo(int newNRows)
        {
            if (newNRows <= 0)
                throw new ArgumentException("Number of rows has to be positive");
            double[] newArray = new double[newNRows];
            int num = Math.Min(this.NRows, newNRows);
            Parallel.For(0, Math.Min(this.NRows, newNRows), i => newArray[i] = Elements[i]);
            NRows = newNRows;
            Elements = newArray;
        }

        public double Norm1()
        {
            if (!this.IsValid())
                throw new ApplicationException("Vector is not initialized");
            return Elements.Sum();
        }

        public double Norm2Sqr()
        {
            if (!this.IsValid())
                throw new ApplicationException("Vector is not initialized");
            return Elements.Sum(e => e * e);
        }

        public double NormInf()
        {
            if (!this.IsValid())
                throw new ApplicationException("Vector is not initialized");
            return Elements.Max(e => Math.Abs(e));
        }

        public Vector Abs()
        {
            if (!this.IsValid())
                throw new ApplicationException("Vector is not initialized");
            Vector v = new Vector(this.NRows);
            Parallel.For(0, this.NRows, i => v[i] = Math.Abs(this.Elements[i]));
            return v;
        }

        public Vector Sqr()
        {
            if (!this.IsValid())
                throw new ApplicationException("Vector is not initialized");
            Vector v = new Vector(this.NRows);
            Parallel.For(0, this.NRows, i => v[i] = this.Elements[i] * this.Elements[i]);
            return v;
        }

        public Vector Sqrt()
        {
            if (!this.IsValid())
                throw new ApplicationException("Vector is not initialized");
            Vector v = new Vector(this.NRows);
            Parallel.For(0, this.NRows, i => v[i] = Math.Sqrt(this.Elements[i]));
            return v;
        }

        public Vector ElementMult(Vector target, Vector source)
        {
            if (!source.IsValid())
                throw new ApplicationException("Source vector is not initialized");
            if (!target.IsValid())
                throw new ApplicationException("Target vector is not initialized");
            if (!Vector.AreCompatible(target, source))
                throw new ApplicationException("Vectors are not compatible");
            Vector v = new Vector(target.NRows);
            Parallel.For(0, this.NRows, i => v[i] = target[i] * source[i]);
            return v;
        }

        public Vector ElementDiv(Vector target, Vector source)
        {
            if (!source.IsValid())
                throw new ApplicationException("Source vector is not initialized");
            if (!target.IsValid())
                throw new ApplicationException("Target vector is not initialized");
            if (!Vector.AreCompatible(target, source))
                throw new ApplicationException("Vectors are not compatible");
            Vector v = new Vector(target.NRows);
            Parallel.For(0, this.NRows, i => v[i] = target[i] / source[i]);
            return v;
        }

        public override bool Equals(object vector)
        {
            Vector that = (Vector)vector;
            if (this.NRows != that.NRows)
                return false;
            for (int i = 0; i < that.NRows; ++i)
                if (this[i] != that[i])
                    return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void Print()
        {
            this.Print("F2");
        }

        public void Print(string format)
        {
            for (int i = 0; i < NRows; ++i)
                Console.WriteLine(Elements[i].ToString(format) + " ");
        }
    }
}
