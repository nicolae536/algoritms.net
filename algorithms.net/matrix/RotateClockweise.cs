using System;
using System.Collections.Generic;


namespace algorithms.net.matrix
{
    // Print a given matrix in counter-clock wise spiral form
    public class RotateClockweise
    {
        HashSet<int> visited = new HashSet<int>();
        List<List<int>> matrixData = new List<List<int>>();
        int width;
        int height;


        public RotateClockweise(string input)
        {
            MatrixModel<int> matrix = MatrixModel<int>.ReadMatrix(input, it => Int32.Parse(it));
            matrixData = matrix.matrixData;
            width = matrix.width;
            height = matrix.height;

            StartRotation();
        }

        private void StartRotation()
        {
            List<int> data = new List<int>();
            string movinDirection = "column-down"; // right, up, left
            int borderBottom = height;
            int borderRight = width;
            int borderTop = -1;
            int borderLeft = 0;
            int visitedCount = 0;

            int i = 0;
            int j = 0;

            while (visitedCount < width * height)
            {
                data.Add(matrixData[i][j]);
                visitedCount++;

                if (movinDirection == "column-down")
                {
                    if (i + 1 == borderBottom)
                    {
                        movinDirection = "line-right";
                        borderBottom--;
                        j = borderLeft + 1;
                    }
                    else
                    {
                        i++;
                    }
                }
                else if (movinDirection == "line-right")
                {
                    if (j + 1 == borderRight)
                    {
                        movinDirection = "column-up";
                        borderRight--;
                        i = borderBottom - 1;
                    }
                    else
                    {
                        j++;
                    }
                }

                else if (movinDirection == "column-up")
                {
                    if (i - 1 == borderTop)
                    {
                        movinDirection = "line-left";
                        borderTop++;
                        j = borderRight - 1;
                    }
                    else
                    {
                        i--;
                    }
                }

                else if (movinDirection == "line-left")
                {
                    if (j - 1 == borderLeft)
                    {
                        movinDirection = "column-down";
                        borderLeft++;
                        i = borderTop + 1;
                    }
                    else
                    {
                        j--;
                    }
                }
            }

            Console.WriteLine(string.Join(" ", data));
        }

        public static void RunProblem()
        {
            string[] inputs =
            {
                "4 4 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16",
                "6 3 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"
            };

            foreach (string item in inputs)
            {
                new RotateClockweise(item);
            }
        }

    }
}

