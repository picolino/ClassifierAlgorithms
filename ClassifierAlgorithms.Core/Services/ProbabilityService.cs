using System;

namespace ClassifierAlgorithms.Core.Services
{
    public class ProbabilityService
    {
        public double CalculateAnalogProbability(double x, double expectation, double dispersion)
        {
            return (1 / (Math.Sqrt(2 * Math.PI * dispersion))) * Math.Pow(Math.E, -((Math.Pow(x - expectation, 2)) / (2 * dispersion)));
        }
    }
}