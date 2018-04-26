using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.net.dynamic_programming
{
    class PaperCutMinimumNumberOfSqares
    {
        int width;
        int height;
        int minValue;
        int[,] sqares;


        public PaperCutMinimumNumberOfSqares(int width, int height)
        {
            this.height = height;
            this.width = width;
            minValue = int.MaxValue;
            sqares = new int[300, 300];

            int maximumDepth = GetMaximumSquares();
            Console.WriteLine("Minimum square cut: " + MinimumSquare(width, height));
        }

        private int MinimumSquare(int width, int height)
        {
            int vMin = int.MaxValue;
            int hMin = int.MaxValue;

            if (width == height)
            {
                return 1;
            }

            if (sqares[width, height] != 0)
            {
                return sqares[width, height];
            }

            for (int i = 1; i <= width / 2; i++)
            {
                hMin = Math.Min(MinimumSquare(i, height) + MinimumSquare(width - i, height), hMin);
            }

            for (int i = 1; i <= height / 2; i++)
            {
                vMin = Math.Min(MinimumSquare(width, i) + MinimumSquare(width, height - i), vMin);
            }

            sqares[width, height] = Math.Min(hMin, vMin);

            return sqares[width, height];
        }

        private int GetMaximumSquares()
        {
            int currentWidht = width;
            int currentHeight = height;
            int i = 0;

            while (currentHeight > 0 || currentHeight > 0)
            {
                if (currentWidht > currentHeight)
                {
                    currentWidht = currentWidht - currentHeight;
                }
                else
                {
                    currentHeight = currentHeight - currentWidht;
                }

                i++;
            }

            return i;
        }

        public static void RunProblem()
        {
            new PaperCutMinimumNumberOfSqares(36, 30);
            new PaperCutMinimumNumberOfSqares(4, 5);
            new PaperCutMinimumNumberOfSqares(13, 29);
        }
    }
}
