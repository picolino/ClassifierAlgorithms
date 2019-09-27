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

        public void Train(double minError)
        {
            var averageError = double.MaxValue;

            while (Math.Abs(averageError) > minError)
            {
                averageError = 0d;

                for (var i = 0; i < firstCLass.Vector.GetLength(0); i++)
                {
                    var x = firstCLass.Vector[i, 0];
                    var y = firstCLass.Vector[i, 1];

                    var result = Classify(x, y);
                    var error = Math.Pow((result - 1) * x + ((result - 1) * y), 2) / 2;
                    averageError += error;

                    zeroCoefficient -= trainSpeed * (result - 1);
                    firstCoefficient -= trainSpeed * (result - 1) * x;
                    secondCoefficient -= trainSpeed * (result - 1) * y;
                }

                for (var i = 0; i < secondClass.Vector.GetLength(0); i++)
                {
                    var x = secondClass.Vector[i, 0];
                    var y = secondClass.Vector[i, 1];

                    var result = Classify(x, y);
                    var error = Math.Pow((result) * x + ((result) * y), 2) / 2; ;
                    averageError += error;
                    
                    zeroCoefficient -= trainSpeed * result;
                    firstCoefficient -= trainSpeed * (result) * x;
                    secondCoefficient -= trainSpeed * (result) * y;
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
            var classifyResult = chanceDifference / (1 + chanceDifference);
            return classifyResult;
        }
    }
}