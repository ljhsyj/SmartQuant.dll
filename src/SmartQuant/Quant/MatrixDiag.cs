using System;

namespace SmartQuant.Quant
{
    public class MatrixDiag
    {
        private Matrix matrix;

        public double this[int i]
        {
            get
            {
                if (i >= 0 && i < this.NDiag)
                    return this.matrix.Elements[i, i];
                this.Error("this[]", "Out of boundry");
                return 0.0;
            }
            set
            {
                if (i >= 0 && i < this.NDiag)
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
            for (int index = 0; index < this.NDiag; ++index)
                this[index] = matrixDiag[index];
        }

        protected void Error(string Where, string What)
        {
        }

        public override bool Equals(object matrixDiag)
        {
            return this.matrix.Equals((object) ((MatrixDiag) matrixDiag).matrix);
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

