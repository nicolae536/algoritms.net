using System;
using System.Collections.Generic;
using System.Text;

namespace algorithms.net.matrix
{
    /**
     * 
     * Given N pieces of chessboard all being ‘white’ and a number of queries Q. There are two types of queries :
     * Update : Given indices of a range [L, R]. Paint all the pieces with their respective opposite color between L and R (i.e. white pieces should be painted with black color and black pieces should be painted with white color).
     * Get : Given indices of a range [L, R]. Find out the number of black pieces between L and R.
     * Let us represent ‘white’ pieces with ‘0’ and ‘black’ pieces with ‘1’.
     * 
     **/
    public class RangeAndUpdateChessboard
    {
        List<int> chessPieces;

        public RangeAndUpdateChessboard(string inputs)
        {
            string[] data = inputs.Split(' ');
            chessPieces = new List<int>();
            List<int> outData = new List<int>();

            for (int i = 0; i < Int32.Parse(data[0]); i++)
            {
                chessPieces.Add(0);
            }

            for (int i = 2; i < Int32.Parse(data[1]) * 5+ 2; i += 5)
            {
                int min = Int32.Parse(data[i + 2]);
                int max = Int32.Parse(data[i + 4]);
                int sum = 0;
                bool wasGet = false;
                for (int k = min; k < max + 1; k++)
                {
                    if (data[i] == "Get")
                    {
                        if (chessPieces[k] == 1)
                        {
                            sum++;
                        }
                        wasGet = true;
                    }
                    else if (data[i] == "Update")
                    {
                        chessPieces[k] = chessPieces[k] == 0 ? 1 : 0;
                    }
                }


                if (wasGet)
                {
                    outData.Add(sum);
                }
            }

            Console.WriteLine(string.Join(" ", outData));
        }

        public static void RunProblem()
        {
            string[] inputs =
            {
                "4 5 Get L 1 R 2 Update L 1 R 2 Get L 0 R 1 Update L 0 R 3 Get L 0 R 3"
            };

            foreach (string item in inputs)
            {
                new RangeAndUpdateChessboard(item);
            }
        }
    }
}
