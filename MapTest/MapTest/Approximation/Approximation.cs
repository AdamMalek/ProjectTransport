using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapTest.Approximation
{
    public class Approximation
    {
        private static double sumX(List<Point> pList, double arg)
        {
            double summed = 0;
            foreach (Point p in pList)
            {
                summed += Math.Pow(p.X, arg);
            }
            return summed;
        }
        private static double sumY(List<Point> pList, double arg)
        {
            double summed = 0;
            foreach (Point p in pList)
            {
                summed += p.Y * Math.Pow(p.X, arg);
            }
            return summed;
        }


        private static double[] gaussElimination(double[,] A, double[] B, int n)
        {
            double[,] tmpA = new double[n, n + 1];
            double[] result = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    tmpA[i, j] = A[i, j];
                }
                tmpA[i, n] = B[i];
            }
            double tmp = 0;
            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    tmp = tmpA[i, k] / tmpA[k, k];
                    for (int j = k; j < n + 1; j++)
                    {
                        tmpA[i, j] -= tmp * tmpA[k, j];
                    }
                }
            }
            for (int k = n - 1; k >= 0; k--)
            {
                tmp = 0;
                for (int j = k + 1; j < n; j++)
                {
                    tmp += tmpA[k, j] * result[j];
                }
                result[k] = (tmpA[k, n] - tmp) / tmpA[k, k];
            }
            return result;
        }


        private static double[] Approx(List<Point> pList, int deg)
        {
            double[,] A = new Double[deg + 1, deg + 1];
            double[] B = new Double[deg + 1];
            for (int i = 0; i <= deg; i++)
            {
                B[i] = sumY(pList, Convert.ToDouble(i));
                for (int j = 0; j <= deg; j++)
                {
                    A[i, j] = sumX(pList, Convert.ToDouble(i + j));
                }
            }
            return gaussElimination(A, B, deg + 1);
        }

        public static double[] Approximate(List<Point> points, List<double> n)
        {
            int quantity = 0;
            for (int i = 0; i < n.Count; i++)
            {
                quantity += Convert.ToInt32(n[i]);
            }
            double[] result = new double[quantity];

            double[] gaussResult = Approx(points, 2);
            int k = 0;
            double interval = 0;


            for (int i = 0; i < n.Count; i++)
            {
                for (double s = 0; s < n[i]; s++)
                {
                    for (int j = 0; j <= 2; j++)
                    {
                        result[k] += gaussResult[j] * Math.Pow(s, j);
                    }
                    k += 1;
                    interval = (points[i + 1].X - points[i].X) / n[i];

                }
            }

            return result;
        }
        

    }
}
