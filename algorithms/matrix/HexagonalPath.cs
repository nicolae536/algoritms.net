using System;
using System.Collections.Generic;
using System.Text;
using algorithms.generic_algorithms;

namespace algorithms.matrix
{
    public class HoneyComb : IComparable
    {
        public int i;
        public int j;

        public double Distance(HoneyComb it)
        {
            return Math.Sqrt(Math.Pow(it.i - i, 2) + Math.Pow(it.j - j, 2));
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return -1;
            }

            HoneyComb hc = (HoneyComb)obj;

            if (i < hc.i)
            {
                return 1;
            }

            if (i > hc.i)
            {
                return -1;
            }
            if (j < hc.j)
            {
                return 1;
            }

            if (j > hc.j)
            {
                return -1;
            }

            return 0;
        }
    }

    public class HexagonalPath
    {
        int numberOfsteps;

        public HexagonalPath()
        {
        }

        public int CuntPaths(int number)
        {
            numberOfsteps = number;
            return CountPathsInHexagon(number, 0, 0);
        }

        private int CountPathsInHexagon(int currentSteps, int currentI, int currentJ)
        {
            if (numberOfsteps <= 1 ||
                (currentSteps <= 0 && (currentI != 0 || currentJ != 0)))
            {
                return 0;
            }

            if (currentSteps == 0 && currentI == 0 && currentJ == 0)
            {
                return 1;
            }

            int[] xPath = { 1, 1, 0, -1, -1, 0 };
            int[] yPath = { 0, -1, -1, 0, 1, 1 };

            int sum = 0;

            for (int i = 0; i < 6; i++)
            {
                int nextI = xPath[i] + currentI;
                int nextJ = yPath[i] + currentJ;
                sum += CountPathsInHexagon(currentSteps - 1, nextI, nextJ);
            }

            return sum;
        }

        public static void RunProblem()
        {
            HexagonalPath hp = new HexagonalPath();
            //Console.WriteLine(hp.CuntPaths(1));
            Console.WriteLine(hp.CuntPaths(2));
            Console.WriteLine(hp.CuntPaths(4));
        }
    }
}
