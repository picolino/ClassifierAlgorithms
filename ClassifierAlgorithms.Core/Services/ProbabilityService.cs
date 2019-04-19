using ClassifierAlgorithms.Core.Domain;
using Math = System.Math;

namespace ClassifierAlgorithms.Core.Services
{
    public class ProbabilityService
    {
        public double CalculateGaussianProbability(double x, double expectation, double dispersion)
        {
            return (1 / (Math.Sqrt(2 * Math.PI * dispersion))) * Math.Pow(Math.E, -((Math.Pow(x - expectation, 2)) / (2 * dispersion)));
        }

        public double CalculateLognProbabilityByCorrelationMatrix(Vector parameters, Vector expectations, Matrix correlationMatrix, double classProbability)
        {
            var parametersInclusiveExpectations = parameters - expectations;
            return -0.5 * (parametersInclusiveExpectations * correlationMatrix.Inverse() * parametersInclusiveExpectations.Transpose()).ToNumber()
                   + Math.Log(classProbability) - Math.Log(2 * Math.PI) + 0.5 * Math.Log(correlationMatrix.Inverse().Determinant());
        }
    }
}