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

        public LogisticRegression(Class firstCLass, Class secondClass)
        {
            this.firstCLass = firstCLass;
            this.secondClass = secondClass;
        }

        private void Train()
        {
            
        }

        private double LogFunction(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        private double LogDerivativeFunction(double x)
        {
            var logResult = LogFunction(x);
            return logResult * (1 - logResult);
        }

        private double Likehood(double x, double y)
        {
            if (y == 1.0)
            {
                return Math.Log(LogFunction(x));
            }
            else
            {
                return Math.Log(1 - LogFunction(x));
            }
        }

        public double Run(double[] input)
        {
            return LogFunction(zeroCoefficient + firstCoefficient * input[0] + secondCoefficient * input[1]);
        }
    }
}