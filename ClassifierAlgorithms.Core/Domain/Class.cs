using System;

namespace ClassifierAlgorithms.Core.Domain
{
    public class Class
    {
        public Class(double[,] vector, double expectationX, double expectationY)
        {
            Id = Guid.NewGuid().ToString();
            Vector = vector;
            ExpectationX = expectationX;
            ExpectationY = expectationY;
        }

        public double[,] Vector { get; }
        public double ExpectationX { get; }
        public double ExpectationY { get; }
        public string Id { get; }
    }
}