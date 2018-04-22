using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.matrix
{
    class MaxMinAlgebricExpression
    {
        /*
         * Maximum and Minimum Values of an Algebraic Expression
         * Given an algebraic expression of the form (x1 + x2 + x3 + . . . + xn) * (y1 + y2 + . . . + ym) and
         * (n + m) integers. Find the maximum and minimum value of the expression using the given
         * integers.
         */
        int m;
        int n;
        string expression;
        List<int> numbers;

        public MaxMinAlgebricExpression()
        {

        }

        public void PermuteNumbers(List<int> now, int size, List<List<int>> data)
        {
            if (size == 1)
            {
                int[] newArr = new int[now.Count];
                now.CopyTo(newArr);
                data.Add(newArr.ToList());
                Console.WriteLine(string.Join(' ', now));
            }

            for (int i = 0; i < size; i++)
            {
                PermuteNumbers(now, size - 1, data);
                // if size is odd, swap first and last
                // element
                if (size % 2 == 1)
                {
                    int k = now[0];
                    now[0] = now[size - 1];
                    now[size - 1] = k;
                    //swap(a[0], a[size - 1]);

                }

                // If size is even, swap ith and last
                // element
                else
                {
                    int k = now[i];
                    now[i] = now[size - 1];
                    now[size - 1] = k;
                }
            }
        }

        //public int GetExpression(List<int> array)
        //{
        //    int s = 0;

        //    for (int i = 0; i < n; i++)
        //    {
        //        s += array[i];
        //    }

        //    int d = 0;
        //    for (int i = n; i < n + m; i++)
        //    {
        //        d +=
        //    }
        //}

        public static void RunProblem()
        {
            MaxMinAlgebricExpression data = new MaxMinAlgebricExpression();
            List<List<int>> outD = new List<List<int>>();
            data.PermuteNumbers(new List<int> { 1, 2, 3 }, 3, outD);
        }
    }
}
