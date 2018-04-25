using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.net.matrix
{
    public class MatrixModel<TModel>
    {
        /**
         * Matrix will be used as example
         * 1 2 3 4 5
         * 6 7 8 9 10
         * 11 12 13 14 15
         * 16 17 18 19 20
         * 21 22 23 24 25
         **/

        public List<List<TModel>> matrixData = new List<List<TModel>>();
        public int height;
        public int width;

        /**
         * Output 
         * 1 6 11 16 21
         * 2 7 12 17 22
         * 3 8 13 18 23
         * 4 9 14 19 24
         * 5 10 15 20 25
         **/
        public MatrixModel<TModel> TransposeMatrix()
        {
            MatrixModel<TModel> newM = new MatrixModel<TModel>();

            for (int i = 0; i < height; i++)
            {
                newM.matrixData.Add(new List<TModel>());
                for (int j = 0; j < width; j++)
                {
                    newM.matrixData[i].Add(matrixData[j][i]);
                }
            }

            return newM;
        }

        /**
         * Output
         * 1 2 3 4 5
         * 10 9 8 7 6
         * 11 12 13 14 15
         * 20 19 18 17 16
         * 21 22 23 24 25
         **/
        public MatrixModel<TModel> Serpentine()
        {
            MatrixModel<TModel> newM = new MatrixModel<TModel>();

            for (int i = 0; i < height; i++)
            {
                newM.matrixData.Add(new List<TModel>());

                if (i % 2 == 0)
                {
                    for (int j = 0; j < width; j++)
                    {
                        newM.matrixData[i].Add(matrixData[i][j]);
                    }
                }
                else
                {
                    for (int j = width - 1; j > -1; j--)
                    {
                        newM.matrixData[i].Add(matrixData[i][j]);
                    }
                }


            }

            return newM;
        }

        /**
         * Output
         * 1 10 11 20 21
         * 2 9 12 19 22
         * 3 8 13 18 23
         * 4 7 14 17 24
         * 5 6 15 16 25
         **/
        public MatrixModel<TModel> ZigZag()
        {
            MatrixModel<TModel> newM = new MatrixModel<TModel>();

            int pivotJ = 0;
            for (int i = 0; i < height; i++)
            {
                newM.matrixData.Add(new List<TModel>());

                for (int j = 0; j < width; j++)
                {

                    if (j % 2 == 0)
                    {
                        pivotJ = 0 + i;
                    }
                    else
                    {
                        pivotJ = width - i - 1;
                    }

                    newM.matrixData[i].Add(matrixData[j][pivotJ]);
                }

            }

            return newM;
        }

        /**
         * Output
         * 1 2 3 4 5
         * 16 6 7 8 9
         * 17 18 10 11 12
         * 19 20 21 13 14
         * 22 23 24 25 15
         **/
        public MatrixModel<TModel> SubdiagonalDiference()
        {
            MatrixModel<TModel> newM = new MatrixModel<TModel>();
            int mI = 0;
            int mJ = 0;
            int kI = (height % 2 == 0) ? height / 2 : height / 2 + 1;
            int kJ = 0;

            for (int i = 0; i < height; i++)
            {
                newM.matrixData.Add(new List<TModel>());

                for (int j = 0; j < width; j++)
                {

                    if (j < i)
                    {
                        newM.matrixData[i].Add(matrixData[kI][kJ]);
                        kJ++;

                        if (kJ == width)
                        {
                            kI = kI + 1;
                            kJ = 0;
                        }
                    }
                    else
                    {
                        newM.matrixData[i].Add(matrixData[mI][mJ]);
                        mJ++;

                        if (mJ == width)
                        {
                            mI = mI + 1;
                            mJ = 0;
                        }
                    }

                }

            }

            return newM;
        }

        /**
         * Output
         * 1 3 6 10 15
         * 2 5 9 14 19
         * 4 8 13 18 22
         * 7 12 17 21 24
         * 11 16 20 23 25
         **/
        public MatrixModel<TModel> ZigZagInverted()
        {
            MatrixModel<TModel> newM = new MatrixModel<TModel>();
            int mI = 0;
            int min = 0;
            int max = 1;
            bool foreward = true;

            for (int i = 0; i < height; i++)
            {
                //newM.matrixData.Add(new List<TModel>());
                for (int j = 0; j < width; j++)
                {
                    if (newM.matrixData.Count <= mI)
                    {
                        newM.matrixData.Add(new List<TModel>());
                    }

                    newM.matrixData[mI].Add(matrixData[i][j]);

                    if (mI == max)
                    {
                        mI = min;
                        if (max < height)
                        {
                            max++;
                        }
                        else if (max == height)
                        {
                            min++;
                        }
                        foreward = true;
                    }
                    else if (mI == min)
                    {
                        mI = max == height ? max - 1 : max;
                        if (max < height)
                        {
                            max++;
                        }
                        else if (max == height)
                        {
                            min++;
                        }
                        foreward = false;
                    }
                    else
                    {
                        if (foreward)
                        {
                            mI++;
                        }
                        else
                        {
                            mI--;
                        }
                    }
                }
            }

            return newM;
        }

        public static MatrixModel<int> MagicSqare(int size)
        {
            if (size % 2 == 0)
            {
                throw new FormatException("Only with odd numbers I work here");
            }

            MatrixModel<int> sqare = new MatrixModel<int>();
            for (int i = 0; i < size; i++)
            {
                sqare.matrixData.Add(new List<int>());
                for (int j = 0; j < size; j++)
                {
                    sqare.matrixData[i].Add(0);
                }
            }

            sqare.height = size;
            sqare.width = size;

            int mI = 0;
            int mJ = size / 2;
            for (int k = 1; k <= size * size; k++)
            {
                sqare.matrixData[mI][mJ] = k;

                mI--;
                mJ++;

                if (mI < 0)
                {
                    mI = size - 1;
                }
                if (mJ >= size)
                {
                    mJ = 0;
                }

                if (sqare.matrixData[mI][mJ] != 0)
                {
                    mI++;
                }
            }

            return sqare;
        }

        public static MatrixModel<TModel> ReadMatrix(string input, Func<string, TModel> tranform)
        {
            string[] data = input.Split(' ');
            MatrixModel<TModel> model = new MatrixModel<TModel>();
            model.width = Int32.Parse(data[0]);
            model.height = Int32.Parse(data[1]);
            int index = 2;

            for (int i = 0; i < model.height; i++)
            {
                model.matrixData.Add(new List<TModel>());
                for (int j = 0; j < model.width; j++)
                {
                    model.matrixData[i].Add(tranform(data[index]));
                    index++;
                }
            }

            return model;
        }

        public static MatrixModel<TModel> SubstractLineColumn(MatrixModel<TModel> input, int column)
        {
            MatrixModel<TModel> newM = new MatrixModel<TModel>();

            for (int i = 0; i < input.height; i++)
            {
                if (i == column)
                {
                    continue;
                }

                newM.matrixData.Add(new List<TModel>());
                for (int j = 0; j < input.height; j++)
                {
                    if (j == column)
                    {
                        continue;
                    }

                    newM.matrixData[newM.matrixData.Count - 1].Add(input.matrixData[i][j]);
                }
            }

            newM.width = input.width - 1;
            newM.height = input.height - 1;

            return newM;
        }

        public static void LogOnScreen(MatrixModel<TModel> model)
        {
            Console.WriteLine();
            for (int i = 0; i < model.matrixData.Count; i++)
            {
                Console.WriteLine(string.Join(" ", model.matrixData[i].Select(it => it.ToString())));
            }
            Console.WriteLine();
        }

        public static void TestMatrix()
        {
            MatrixModel<int> model = new MatrixModel<int>();
            model.height = 5;
            model.width = 5;
            model.matrixData = new List<List<int>>
            {
                new List<int> {1 , 2, 3, 4, 5},
                new List<int> {6 , 7, 8, 9, 10},
                new List<int> {11 , 12, 13, 14, 15},
                new List<int> {16 , 17, 18, 19, 20},
                new List<int> {21 , 22, 23, 24, 25},
            };
            Console.WriteLine("Start testing, initial model");
            MatrixModel<int>.LogOnScreen(model);

            Console.WriteLine("Transpose");
            MatrixModel<int>.LogOnScreen(model.TransposeMatrix());

            Console.WriteLine("Serpentine movement");
            MatrixModel<int>.LogOnScreen(model.Serpentine());

            Console.WriteLine("ZigZag movement");
            MatrixModel<int>.LogOnScreen(model.ZigZag());

            Console.WriteLine("Subdiagonal difference");
            MatrixModel<int>.LogOnScreen(model.SubdiagonalDiference());

            Console.WriteLine("ZigZag inverted");
            MatrixModel<int>.LogOnScreen(model.ZigZagInverted());

            Console.WriteLine("Magic sqare");
            MatrixModel<int>.LogOnScreen(MatrixModel<int>.MagicSqare(5));
        }
    }
}
