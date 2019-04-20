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
            var firstClassProbabilityX = probabilityService.CalculateGaussianProbability(x, firstClass.ExpectationX, firstClass.DispersionX) * (firstClassProbability);
            var firstClassProbabilityY = probabilityService.CalculateGaussianProbability(y, firstClass.ExpectationY, firstClass.DispersionY) * (firstClassProbability);

            
            var secondClassProbabilityX = probabilityService.CalculateGaussianProbability(x, secondClass.ExpectationX, secondClass.DispersionX) * (secondClassProbability);
            var secondClassProbabilityY = probabilityService.CalculateGaussianProbability(y, secondClass.ExpectationY, secondClass.DispersionY) * (secondClassProbability);

            return firstClassProbabilityX * firstClassProbabilityY > secondClassProbabilityX * secondClassProbabilityY
                       ? firstClass
                       : secondClass;
        }

        public Class ClassifyByCorrelation(double x, double y)
        {
            var correlation = GetCorrelationMatrix();
            var parameters = new Vector(new [,] {{x, y}});

            var resultFirstClass = probabilityService.CalculateLognProbabilityByCorrelationMatrix(parameters, 
                                                                                          new Vector(new [,] {{firstClass.ExpectationX, firstClass.ExpectationY}}), 
                                                                                          correlation, 
                                                                                          firstClassProbability);
            var resultSecondClass = probabilityService.CalculateLognProbabilityByCorrelationMatrix(parameters, 
                                                                                          new Vector(new [,] {{secondClass.ExpectationX, secondClass.ExpectationY}}),
                                                                                          correlation, 
                                                                                          secondClassProbability);

            return resultFirstClass > resultSecondClass
                       ? firstClass
                       : secondClass;
        }

        private Matrix GetCorrelationMatrix()
        {
            var normalizedVectorFirstClass = new double[firstClass.Vector.GetLength(0), firstClass.Vector.GetLength(1)];
            for (int i = 0; i < firstClass.Vector.GetLength(0); i++)
            {
                normalizedVectorFirstClass[i, 0] = firstClass.Vector[i, 0] - firstClass.DispersionX;
                normalizedVectorFirstClass[i, 1] = firstClass.Vector[i, 1] - firstClass.DispersionY;
            }

            var normalizedVectorSecondClass = new double[secondClass.Vector.GetLength(0), secondClass.Vector.GetLength(1)];
            for (int i = 0; i < secondClass.Vector.GetLength(0); i++)
            {
                normalizedVectorSecondClass[i, 0] = secondClass.Vector[i, 0] - secondClass.DispersionX;
                normalizedVectorSecondClass[i, 1] = secondClass.Vector[i, 1] - secondClass.DispersionY;
            }
            
            var xxAverage = 0d;
            var xyAverage = 0d;
            var yyAverage = 0d;
            for (int i = 0; i < normalizedVectorFirstClass.GetLength(0); i++)
            {
                xxAverage += normalizedVectorFirstClass[i, 0] * normalizedVectorFirstClass[i, 0];
                xyAverage += normalizedVectorFirstClass[i, 1] * normalizedVectorSecondClass[i, 1];
                yyAverage += normalizedVectorSecondClass[i, 1] * normalizedVectorSecondClass[i, 1];
            }

            xxAverage /= normalizedVectorFirstClass.GetLength(0);
            xyAverage /= normalizedVectorFirstClass.GetLength(0);
            yyAverage /= normalizedVectorFirstClass.GetLength(0);

            var result = new [,]
                         {
                             {xxAverage, xyAverage},
                             {xyAverage, yyAverage }
                         };

            return new Matrix(result);
        }
    }
}