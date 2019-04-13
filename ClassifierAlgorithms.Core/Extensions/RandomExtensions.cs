using System;

namespace ClassifierAlgorithms.Core.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        ///   Generates normally distributed numbers. Each operation makes two Gaussians 
        ///   for the price of one, 
        ///   and apparently they can be cached or something for better performance, 
        ///   but who cares.
        /// </summary>
        /// <param name="r"></param>
        /// <param name = "mu">Mean of the distribution</param>
        /// <param name = "sigma">Standard deviation</param>
        /// <returns></returns>
        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = 1.0 - r.NextDouble();
            var u2 = 1.0 - r.NextDouble();

            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                  Math.Sin(2.0 * Math.PI * u2);

            var randGaussian = mu + sigma * randStdNormal;

            return randGaussian;
        }
    }
}