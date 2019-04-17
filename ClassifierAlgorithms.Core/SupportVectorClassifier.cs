using ClassifierAlgorithms.Core.Domain;

namespace ClassifierAlgorithms.Core
{
    public class SupportVectorClassifier
    {
        private readonly Class firstClass;
        private readonly Class secondClass;

        public SupportVectorClassifier(Class firstClass, Class secondClass)
        {
            this.firstClass = firstClass;
            this.secondClass = secondClass;
        }

        public void Classify()
        {
            // Ищем ближайшую к 2 классу точку в 1 классе.
            var firstClassBestDot = (double.MaxValue, double.MaxValue);
            for (var i = 0; i < firstClass.Vector.GetLength(0); i++)
            {
                var (x, y) = (firstClass.Vector[i, 0], firstClass.Vector[i, 1]);
                var isXBest = x - secondClass.Expectation < firstClassBestDot.Item1;
                var isYBest = x - secondClass.Expectation < firstClassBestDot.Item2;
                if (isXBest && isYBest)
                {
                    firstClassBestDot = (x, y);
                }
            }

            // Ищем ближайшую к 1 классу точку во 2 классе.
            var secondClassBestDot = (double.MaxValue, double.MaxValue);
            for (var i = 0; i < secondClass.Vector.GetLength(0); i++)
            {
                var (x, y) = (secondClass.Vector[i, 0], secondClass.Vector[i, 1]);
                var isXBest = x - firstClass.Expectation < secondClassBestDot.Item1;
                var isYBest = x - firstClass.Expectation < secondClassBestDot.Item2;
                if (isXBest && isYBest)
                {
                    secondClassBestDot = (x, y);
                }
            }

            //TODO: Задача Лагранжа?
        }
    }
}