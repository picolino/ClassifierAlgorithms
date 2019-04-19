using System;
using ClassifierAlgorithms.Core.Domain;
using ClassifierAlgorithms.Core.Services;

namespace ClassifierAlgorithms.Core
{
    public class BayesClassifier
    {
        private readonly Class firstClass;
        private readonly Class secondClass;
        private readonly ProbabilityService probabilityService;

        private readonly double firstClassProbability;
        private readonly double secondClassProbability;

        public BayesClassifier(Class firstClass, Class secondClass)
        {
            this.firstClass = firstClass;
            this.secondClass = secondClass;

            var allPoints = firstClass.Vector.GetLength(0) + secondClass.Vector.GetLength(0);
            firstClassProbability = firstClass.Vector.GetLength(0) / (double)allPoints;
            secondClassProbability = secondClass.Vector.GetLength(0) / (double)allPoints;

            probabilityService = new ProbabilityService();
        }

        public Class Classify(double x, double y)
        {
            var firstClassProbabilityX = probabilityService.CalculateGaussianProbability(x, firstClass.Expectation, firstClass.Dispersion) * (firstClassProbability);
            var firstClassProbabilityY = probabilityService.CalculateGaussianProbability(y, firstClass.Expectation, firstClass.Dispersion) * (firstClassProbability);

            
            var secondClassProbabilityX = probabilityService.CalculateGaussianProbability(x, secondClass.Expectation, secondClass.Dispersion) * (secondClassProbability);
            var secondClassProbabilityY = probabilityService.CalculateGaussianProbability(y, secondClass.Expectation, secondClass.Dispersion) * (secondClassProbability);

            return firstClassProbabilityX * firstClassProbabilityY > secondClassProbabilityX * secondClassProbabilityY
                       ? firstClass
                       : secondClass;
        }

        public Class ClassifyByCorrelation(double x, double y)
        {
            var correlationArray = new [,]
                             {
                                 {firstClass.Dispersion, 0.3},
                                 {0, secondClass.Dispersion}
                             };
            var correlation = new Matrix(correlationArray);
            var parameters = new Vector(new [,] {{x, y}});

            var resultFirstClass = probabilityService.CalculateLognProbabilityByCorrelationMatrix(parameters, 
                                                                                          new Vector(new [,] {{firstClass.Expectation, firstClass.Expectation}}), 
                                                                                          correlation, 
                                                                                          firstClassProbability);
            var resultSecondClass = probabilityService.CalculateLognProbabilityByCorrelationMatrix(parameters, 
                                                                                          new Vector(new [,] {{secondClass.Expectation, secondClass.Expectation}}),
                                                                                          correlation, 
                                                                                          secondClassProbability);

            return resultFirstClass > resultSecondClass
                       ? firstClass
                       : secondClass;
        }
    }
}