namespace ClassifierAlgorithms.Core.Domain
{
    public class Class
    {
        public Class(double[,] vector)
        {
            Vector = vector;
        }

        public double[,] Vector { get; }
    }
}