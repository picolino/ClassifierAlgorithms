using System;

namespace ClassifierAlgorithms.Core.Domain
{
    public class Class
    {
        public Class(double[,] vector, double expectationX, double expectationY, double dispersionX, double dispersionY)
        {
            Id = Guid.NewGuid().ToString();
            Vector = vector;
            ExpectationX = expectationX;
            ExpectationY = expectationY;
            DispersionX = dispersionX;
            DispersionY = dispersionY;
        }

        public double[,] Vector { get; }
        public double ExpectationX { get; }
        public double ExpectationY { get; }
        public double DispersionX { get; }
        public double DispersionY { get; }
        public string Id { get; }
    }
}