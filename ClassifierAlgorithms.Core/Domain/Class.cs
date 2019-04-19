using System;

namespace ClassifierAlgorithms.Core.Domain
{
    public class Class
    {
        public Class(double[,] vector, double expectation, double dispersion)
        {
            Id = Guid.NewGuid().ToString();
            Vector = vector;
            Expectation = expectation;
            Dispersion = dispersion;
        }

        public double[,] Vector { get; }
        public double Expectation { get; }
        public double Dispersion { get; }
        public double Dependency { get; set; }
        public string Id { get; }
    }
}