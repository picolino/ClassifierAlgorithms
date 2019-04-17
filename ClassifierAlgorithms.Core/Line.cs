namespace ClassifierAlgorithms.Core
{
    public class Line
    {
        private readonly double a;
        private readonly double b;
        private readonly double c;

        public Line(double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Line(double x1, double x2, double y1, double y2)
        {
            a = y1 - y2;
            b = x2 - x1;
            c = x1 * y2 - x2 * y1;
        }

        public double Calculate(double x, double y)
        {
            return a * x + b * y + c;
        }
    }
}