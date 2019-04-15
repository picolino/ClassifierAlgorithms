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

        public Class GenerateClassByGaussian(int vectorSize, double expectation, double dispersion)
        {
            var vector = new double[vectorSize, 2];
            for (var i = 0; i < vectorSize; i++)
            {
                vector[i, 0] = random.NextGaussian(expectation, dispersion);
                vector[i, 1] = random.NextGaussian(expectation, dispersion);
            }
            return new Class(vector);
        }
    }
}