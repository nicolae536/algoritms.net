using System;
using algorithms.matrix;
using algorithms.dynamic_programming;

namespace algorithms
{
    class Program
    {
        public static string PROJECT_ROOT = @"D:\Workspace\algoritms.net\algorithms";

        static void Main(string[] args)
        {
            //SnackVendorFactory.RunProblem("astar");
            //SnackVendorFactory.RunProblem("dijstra");
            //MatrixImageProcessing.RunProblem();
            //RotateClockweise.RunProblem();
            //MaxMinAlgebricExpression.RunProblem();
            //Determinant.RunProblem();
            //HexagonalPath.RunProblem();
            //RangeAndUpdateChessboard.RunProblem();
            //MatrixModel<int>.TestMatrix();
            //PainterPartition.RunProblem();
            //ASubstringOfB.RunProblem();
            //LongTreePath.RunProblem();
            //MarsRover.RunProblem();
            AdictiveGameFlow.RunProblem();
            Console.ReadLine();
        }
    }
}
