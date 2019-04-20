using System;
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

        public Class GenerateClassByGaussian(int vectorSize, double expectationX, double expectationY, double dispersionX, double dispersionY)
        {
            var vector = new double[vectorSize, 2];
            for (var i = 0; i < vectorSize; i++)
            {
                vector[i, 0] = random.NextGaussian(expectationX, dispersionX);
                vector[i, 1] = random.NextGaussian(expectationY, dispersionY);
            }

            return new Class(vector, expectationX, expectationY, dispersionX, dispersionY);
        }
    }
}