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

        //public Class Classify(double x, double y)
        //{
        //    var firstClassProbabilityX = probabilityService.CalculateGaussianProbability(x, firstClass.ExpectationX, firstClass.DispersionX) * (firstClassProbability);
        //    var firstClassProbabilityY = probabilityService.CalculateGaussianProbability(y, firstClass.ExpectationY, firstClass.DispersionY) * (firstClassProbability);

            
        //    var secondClassProbabilityX = probabilityService.CalculateGaussianProbability(x, secondClass.ExpectationX, secondClass.DispersionX) * (secondClassProbability);
        //    var secondClassProbabilityY = probabilityService.CalculateGaussianProbability(y, secondClass.ExpectationY, secondClass.DispersionY) * (secondClassProbability);

        //    return firstClassProbabilityX * firstClassProbabilityY > secondClassProbabilityX * secondClassProbabilityY
        //               ? firstClass
        //               : secondClass;
        //}

        public Class ClassifyByCorrelation(double x, double y, Matrix correlation)
        {
            var parameters = new Vector(new [,] {{x},{y}});

            var resultFirstClass = probabilityService.CalculateReverseProbabilityByCorrelationMatrix(parameters, 
                                                                                          new Vector(new [,] {{firstClass.ExpectationX}, {firstClass.ExpectationY}}), 
                                                                                          correlation, 
                                                                                          firstClassProbability);
            var resultSecondClass = probabilityService.CalculateReverseProbabilityByCorrelationMatrix(parameters, 
                                                                                          new Vector(new [,] {{secondClass.ExpectationX}, {secondClass.ExpectationY}}),
                                                                                          correlation, 
                                                                                          secondClassProbability);

            return resultFirstClass < resultSecondClass
                       ? firstClass
                       : secondClass;
        }

        //public double[,] GetPointsOnDivideLine(double min, double max, double count = 100, double step = 0.1)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
                
        //    }
        //}
    }
}