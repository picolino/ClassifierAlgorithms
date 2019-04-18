using System;
using ClassifierAlgorithms.Core.Domain;
using ClassifierAlgorithms.Core.Services;

namespace ClassifierAlgorithms.Core
{
    public class BayesClassifier
    {
        private readonly Class firstClass;
        private readonly Class secondClass;
        private readonly MatrixService matrixService;
        private readonly ProbabilityService probabilityService;

        public BayesClassifier(Class firstClass, Class secondClass)
        {
            this.firstClass = firstClass;
            this.secondClass = secondClass;

            matrixService = new MatrixService();
            probabilityService = new ProbabilityService();
        }

        public Class Calculate(double x, double y)
        {
            var allPoints = firstClass.Vector.GetLength(0) + secondClass.Vector.GetLength(0);
            var firstClassProbabilityX = probabilityService.CalculateGaussianProbability(x, firstClass.Expectation, firstClass.Dispersion) * (firstClass.Vector.GetLength(0) / (double)allPoints);
            var firstClassProbabilityY = probabilityService.CalculateGaussianProbability(y, firstClass.Expectation, firstClass.Dispersion) * (firstClass.Vector.GetLength(0) / (double)allPoints);

            
            var secondClassProbabilityX = probabilityService.CalculateGaussianProbability(x, secondClass.Expectation, secondClass.Dispersion) * (secondClass.Vector.GetLength(0) / (double)allPoints);
            var secondClassProbabilityY = probabilityService.CalculateGaussianProbability(y, secondClass.Expectation, secondClass.Dispersion) * (secondClass.Vector.GetLength(0) / (double)allPoints);

            return firstClassProbabilityX * firstClassProbabilityY > secondClassProbabilityX * secondClassProbabilityY
                       ? firstClass
                       : secondClass;
        }

        //TODO:
    }
}