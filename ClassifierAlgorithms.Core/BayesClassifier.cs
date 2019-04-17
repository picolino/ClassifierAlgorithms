using System;
using ClassifierAlgorithms.Core.Domain;
using ClassifierAlgorithms.Core.Services;

namespace ClassifierAlgorithms.Core
{
    public class BayesClassifier
    {
        private readonly Class firstClass;
        private readonly Class secondClass;
        private readonly double[,] covariationMatrix;
        private readonly MatrixService matrixService;

        public BayesClassifier(Class firstClass, Class secondClass, double[,] covariationMatrix)
        {
            this.firstClass = firstClass;
            this.secondClass = secondClass;
            this.covariationMatrix = covariationMatrix;

            matrixService = new MatrixService();
        }

        public void Calculate(double x, double y)
        {
            var probabilityX = BayesFunction(x, y, firstClass);
            var probabilityY = BayesFunction(x, y, secondClass);
        }

        private double BayesFunction(double x, double y, Class data)
        {
            (1 / (Math.Pow(2 * Math.PI, 1 / 2d) * (Math.Pow(matrixService.GetDeterminant(covariationMatrix), 1 / 2d))))
                * Math.Exp(-0.5 * ((x - data.Expectation) / (/*Ei(x-ui)*/)));
        }

        //TODO:
    }
}