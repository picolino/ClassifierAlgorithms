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
    }
}