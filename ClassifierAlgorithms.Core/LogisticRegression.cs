using System;
using ClassifierAlgorithms.Core.Domain;

namespace ClassifierAlgorithms.Core
{
    public class LogisticRegression
    {
        private readonly Class firstCLass;
        private readonly Class secondClass;

        private double zeroCoefficient = 0.1;
        private double firstCoefficient = 0.5;
        private double secondCoefficient = 0.5;

        private double trainSpeed = 0.001;

        public LogisticRegression(Class firstCLass, Class secondClass)
        {
            this.firstCLass = firstCLass;
            this.secondClass = secondClass;
        }

        public void Train()
        {
            var averageError = double.MaxValue;

            while (Math.Abs(averageError) > 0.000018)
            {
                averageError = 0d;

                for (var i = 0; i < firstCLass.Vector.GetLength(0); i++)
                {
                    var x = firstCLass.Vector[i, 0];
                    var y = firstCLass.Vector[i, 1];

                    var error = 1 - Classify(x, y);
                    zeroCoefficient += trainSpeed * error;
                    firstCoefficient += trainSpeed * error * x;
                    secondCoefficient += trainSpeed * error * y;
                    averageError += error;
                }

                for (var i = 0; i < secondClass.Vector.GetLength(0); i++)
                {
                    var x = secondClass.Vector[i, 0];
                    var y = secondClass.Vector[i, 1];

                    var error = - Classify(x, y);
                    zeroCoefficient += trainSpeed * error;
                    firstCoefficient += trainSpeed * error * x;
                    secondCoefficient += trainSpeed * error * y;
                    averageError += error;
                }

                averageError /= firstCLass.Vector.GetLength(0) + secondClass.Vector.GetLength(0);
            }
        }

        private double CalculateBorderFunction(double x, double y)
        {
            return zeroCoefficient + firstCoefficient * x + secondCoefficient * y;
        }

        public double Classify(double x, double y)
        {
            var chanceDifference = Math.Exp(CalculateBorderFunction(x, y));
            return chanceDifference / (1 + chanceDifference);
        }
    }
}