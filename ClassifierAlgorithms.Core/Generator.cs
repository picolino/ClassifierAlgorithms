using System;
using Accord.Statistics.Distributions.Multivariate;
using ClassifierAlgorithms.Core.Domain;
using ClassifierAlgorithms.Core.Extensions;

namespace ClassifierAlgorithms.Core
{
    public class Generator
    {
        private readonly Random random;

        public Generator()
        {
            random = new Random();
        }

        public Class GenerateClassByGaussian(int vectorSize, double expectationX, double expectationY, double[,] covarian)
        {
            var vector = new double[vectorSize, 2];
            var distribution = new MultivariateNormalDistribution(new[] {expectationX, expectationY}, covarian);
            var generatedPoints = distribution.Generate(vectorSize);
            
            for (int i = 0; i < vectorSize; i++)
            {
                vector[i, 0] = generatedPoints[i][0];
                vector[i, 1] = generatedPoints[i][1];
            }

            return new Class(vector, expectationX, expectationY);
        }
    }
}