using System;
using System.Threading.Tasks;

namespace SmartQuant.Quant
{
    public class MatrixDiag
    {
        private Matrix matrix;

        public double this [int i]
        {
            get
            {
                if (0 <= i && i < this.NDiag)
                    return this.matrix.Elements[i, i];
                this.Error("this[]", "Out of boundry");
                return 0.0;
            }
            set
            {
                if (0 <= i && i < this.NDiag)
                    this.matrix.Elements[i, i] = value;
                else
                    this.Error("this[]", "Out of boundry");
            }
        }

        public int NDiag
        {
            get
            {
                return Math.Min(this.matrix.N, this.matrix.M);
            }
        }

        public MatrixDiag(Matrix matrix)
        {
            this.matrix = matrix;
        }

        public void Assign(MatrixDiag matrixDiag)
        {
            if (!Matrix.AreComparable(this.matrix, matrixDiag.matrix))
                return;
            Parallel.For(0, NDiag, i => this[i] = matrixDiag[i]);
        }

        protected void Error(string Where, string What)
        {
        }

        public override bool Equals(object matrixDiag)
        {
            return this.matrix.Equals(((MatrixDiag)matrixDiag).matrix);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

