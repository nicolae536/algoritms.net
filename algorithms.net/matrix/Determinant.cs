using System;
using System.Linq;

namespace algorithms.net.matrix
{
    public class Determinant
    {
        MatrixModel<int> matrix;
        double determinant = 0;

        public Determinant(string input)
        {
            matrix = MatrixModel<int>.ReadMatrix(input, it => Int32.Parse(it));
        }

        public bool IsSingular()
        {
            return GetDeterminant() == 0;
        }

        public double GetDeterminant(bool recompute = false)
        {
            if (determinant == 0 || recompute)
            {
                determinant = RecomputeDeterminat(matrix);
            }

            return determinant;
        }

        private double RecomputeDeterminat(MatrixModel<int> matrix)
        {
            int width = matrix.width;
            if (width == 1)
            {
                return matrix.matrixData[width][width];
            }

            if (width == 2)
            {
                return matrix.matrixData[0][0] * matrix.matrixData[1][1] - (matrix.matrixData[0][1] * matrix.matrixData[1][0]);
            }

            double matrixDet = 0;

            for (int i = 0; i < width; i++)
            {
                MatrixModel<int> submatrix = MatrixModel<int>.SubstractLineColumn(matrix, i);
                matrixDet += (matrix.matrixData[0][i] * Math.Pow(-1, i) * RecomputeDeterminat(submatrix));
            }

            return matrixDet;
        }

        public static void RunProblem()
        {
            string[] inputs =
            {
                "4 4 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16",
                "3 3 0 0 0 4 5 6 1 2 3",
                "3 3 1 0 0 4 5 6 1 2 3",
            };

            foreach (string item in inputs)
            {
                Determinant d = new Determinant(item);
                Console.WriteLine($"Is singular {d.IsSingular()} determinant={d.GetDeterminant()}");
            }
        }
    }
}
