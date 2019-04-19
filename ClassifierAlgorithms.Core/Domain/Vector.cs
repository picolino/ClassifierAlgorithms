using System;

namespace ClassifierAlgorithms.Core.Domain
{
    public class Vector : Matrix
    {
        public Vector(double[,] input) : base(input)
        {
            if (input.GetLength(0) != 1)
            {
                throw new Exception("Вектор должен быть одномерным массивом");
            }
        }

        public Vector(int column) : base(1, column)
        {
        }
    }
}