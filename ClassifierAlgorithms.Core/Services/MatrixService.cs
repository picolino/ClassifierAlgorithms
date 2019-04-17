namespace ClassifierAlgorithms.Core.Services
{
    public class MatrixService
    {
        public double[,] TransparentDoubleMatrix(double[,] matrix)
        {
            var n = matrix.GetLength(0);
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    var temp = matrix[i, j];
                    matrix[i, j] = matrix[j, i];
                    matrix[j, i] = temp;
                }
            }
            return matrix;
        }

        public double GetDeterminant(double[,] matrix)
        {
            if (matrix.Length == 4)
            {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            double sign = 1, result = 0;
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                var minor = GetMinor(matrix, i);
                result += sign * matrix[0, i] * GetDeterminant(minor);
                sign = -sign;
            }
            return result;
        }

        private double[,] GetMinor(double[,] matrix, int n)
        {
            var result = new double[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (var i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 0, col = 0; j < matrix.GetLength(1); j++)
                {
                    if (j == n)
                        continue;
                    result[i - 1, col] = matrix[i, j];
                    col++;
                }
            }
            return result;
        }
    }
}